using Sho.Pocket.Core.Abstractions;
using Sho.Pocket.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Sho.Pocket.Desktop
{
    public partial class Dashboard : Window
    {
        private readonly ISummaryService _summaryService;

        private List<PeriodSummary> Periods { get; set; }
        
        public Dashboard(ISummaryService summaryService)
        {
            InitializeComponent();

            _summaryService = summaryService;

            InitPeriods();
        }

        private void InitPeriods()
        {
            Periods = _summaryService.GetPeriods();

            PeriodsListView.ItemsSource = Periods.OrderByDescending(p => p.ReportedDate);

            foreach (PeriodSummary period in Periods)
            {
                AssetsListBox.ItemsSource = period.Assets;
                SummaryViewbox.Text = period.ToString();
            }

            if (Periods.Any())
            {
                SetDefaultPeriodItem();
            }

            UpdateAssetPanel.Visibility = Visibility.Collapsed;
        }

        private void SetDefaultPeriodItem()
        {
            PeriodsListView.SelectedIndex = 0;
            PeriodsListView_MouseUp(null, null);
        }

        private void PeriodsListView_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedItem = PeriodsListView.SelectedItem as PeriodSummary;
            var period = Periods.FirstOrDefault(p => p.Id == selectedItem?.Id);

            SummaryViewbox.Text = period?.ToString();
            AssetsListBox.ItemsSource = period.Assets;

            UpdateAssetPanel.Visibility = Visibility.Collapsed;
        }
        
        private void SavePeriodButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ReportedDatePicker.SelectedDate.HasValue
                || !Decimal.TryParse(xRateUSDToUAHTextBox.Text, out decimal xRateUSD) 
                || !Decimal.TryParse(xRateEURToUAHTextBox.Text, out decimal xRateEUR))
            {
                return;
            }

            PeriodSummary newPeriodSummary = new PeriodSummary(ReportedDatePicker.SelectedDate.Value, xRateUSD, xRateEUR);

            PeriodSummary result = _summaryService.AddPeriod(newPeriodSummary);

            Periods.Add(result);
            PeriodsListView.ItemsSource = Periods.OrderByDescending(p => p.ReportedDate);
        }

        private void SaveAssetButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedPeriod = PeriodsListView.SelectedItem as PeriodSummary;

            if (selectedPeriod == null)
            {
                return;
            }

            Asset result = _summaryService.AddAssetToPeriod(
                new Asset
                {
                    Name = AssetNameTextBox.Text,
                    CurrencyName = AssetCurrencyTextBox.Text,
                    TypeName = AssetTypeTextBox.Text,
                    Balance = decimal.Parse(AssetBalanceTextBox.Text),
                    PeriodId = selectedPeriod.Id
                }
            );

            InitPeriods();
        }

        private void UpdateAssetButton_Click(object sender, RoutedEventArgs e)
        {
            if (_assetId == null)
            {
                return;
            }

            Asset selectedAsset = new Asset
            {
                Id = _assetId,
                PeriodId = _periodId,
                Name = UpdateAssetNameTextBox.Text,
                CurrencyName = UpdateAssetCurrencyTextBox.Text,
                TypeName = UpdateAssetTypeTextBox.Text,
                Balance = Decimal.Parse(UpdateAssetBalanceTextBox.Text)
            };

            _summaryService.UpdateAsset(selectedAsset);

            InitPeriods();
        }

        private void DeleteAssetButton_Click(object sender, RoutedEventArgs e)
        {
            Asset selectedAsset = AssetsListBox.SelectedItem as Asset;
            
            if (selectedAsset == null)
            {
                return;
            }

            _summaryService.RemoveAsset(selectedAsset.Id, selectedAsset.PeriodId);

            InitPeriods();
        }

        private Guid _assetId;
        private Guid _periodId;

        private void AssetsListBox_MouseUp(object sender, RoutedEventArgs e)
        {
            Asset selectedAsset = AssetsListBox.SelectedItem as Asset;

            if (selectedAsset == null)
            {
                return;
            }

            UpdateAssetPanel.Visibility = Visibility.Visible;

            _assetId = selectedAsset.Id;
            _periodId = selectedAsset.PeriodId;
            UpdateAssetNameTextBox.Text = selectedAsset.Name;
            UpdateAssetCurrencyTextBox.Text = selectedAsset.CurrencyName;
            UpdateAssetTypeTextBox.Text = selectedAsset.TypeName;
            UpdateAssetBalanceTextBox.Text = selectedAsset.Balance.ToString();
        }

        private void DeletePeriodButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedPeriod = PeriodsListView.SelectedItem as PeriodSummary;

            if (selectedPeriod == null)
            {
                return;
            }

            _summaryService.DeletePeriod(selectedPeriod.Id);

            InitPeriods();
        }
    }
}
