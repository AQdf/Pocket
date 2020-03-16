import { Component, OnInit } from '@angular/core';
import * as Highcharts from 'highcharts';

import { BalancesTotalService } from '../../services/balances-total.service';
import { BalanceChanges } from '../../models/balance-changes.model';
import { BalancePrimaryCurrency } from '../../models/balance-primary-currency.model';

@Component({
  selector: 'app-balances-chart',
  templateUrl: './balances-chart.component.html',
  styleUrls: ['./balances-chart.component.css']
})
export class BalancesChartComponent implements OnInit {

  Highcharts: typeof Highcharts = Highcharts;
  pieChart: Highcharts.Options;
  selectedBalanceChangeChart: Highcharts.Options;
  
  balanceChanges: BalanceChanges[];

  constructor(private balancesTotalService: BalancesTotalService) { }

  ngOnInit() {
    this.initBalancePieChartData();
    this.initBalanceChangeLineCharts();
  }

  initBalancePieChartData() {
    this.balancesTotalService.getBalancesInUserPrimaryCurrency().subscribe((balances: BalancePrimaryCurrency[]) => {
      if (balances) {
        this.pieChart = this.createBalancePieChart(balances);
      }
    });
  }

  initBalanceChangeLineCharts() {
    this.balancesTotalService.getBalanceTotalChanges().subscribe((currenciesTotals: BalanceChanges[]) => {
      if (!currenciesTotals || currenciesTotals.length === 0) {
        return;
      }
      
      this.balanceChanges = currenciesTotals;
      this.selectedBalanceChangeChart = this.createBalanceChangeChart(currenciesTotals[0]);
    });
  }

  selectBalanceChangeChart(e) {
    let selectedCurrency = e.target.value;
    let data = this.balanceChanges.find((c: BalanceChanges) => c.currency === selectedCurrency);
    this.selectedBalanceChangeChart = this.createBalanceChangeChart(data);
  }

  createBalancePieChart(balances: BalancePrimaryCurrency[]) {
    let chartData = [];  
    for (var i = 0; i < balances.length; i++) {  
        chartData.push({  
            "name": balances[i].assetName,  
            "y": balances[i].primaryCurrencyValue,  
            sliced: true,  
            selected: true  
        })  
    }

    let effectiveDate = new Date(balances[0].effectiveDate).toDateString();
    let currency = balances[0].primaryCurrency;

    let options: Highcharts.Options = {  
        chart: {  
            plotBackgroundColor: null,  
            plotBorderWidth: null,  
            plotShadow: false,  
            type: 'pie',  
            backgroundColor: "#f9f9f9",  
            options3d: {  
                enabled: true,  
                alpha: 45,  
                beta: 0  
            },
            height: 700
        },  
        title: {  
            text: currency,
        },  
        subtitle: {  
            text: effectiveDate  
        },  
        tooltip: {  
            pointFormat: '{series.name}: <b>{point.y}</b>'  
        },  
        plotOptions: {  
            pie: {  
                allowPointSelect: true,  
                cursor: 'pointer',  
                depth: 35,  
                dataLabels: {  
                    enabled: true,  
                    format: '<b>{point.name}</b>: {point.percentage:.1f} %'  
                },
                showInLegend: true
            }  
        },  
        series: [{  
            name: 'Total balances',  
            type: "pie",
            data: chartData  
        }]  
    };

    return options;
  }

  createBalanceChangeChart(changes: BalanceChanges) {
    let chartData = [];
    let totals = changes.values;

    for (var i = 0; i < totals.length; i++) {
      var formattedDate = new Date(totals[i].effectiveDate);
      var ticks = Date.UTC(formattedDate.getUTCFullYear(), formattedDate.getUTCMonth(), formattedDate.getUTCDate())
      var formattedValue = Number.parseFloat(totals[i].value.toFixed(2));
      chartData.push([ticks, formattedValue]);
    }

    let options: Highcharts.Options = {
        chart: {
          backgroundColor: "#f9f9f9",
          type: 'line',
        },
        title: {
            text: changes.currency
        },
        subtitle: {
            text: ''
        },
        xAxis: {
            type: 'datetime',
        },
        plotOptions: {
            line: {
                dataLabels: {
                    enabled: true
                },
                color: "#E4D354",
                enableMouseTracking: true
            }
        },
        series: [{  
          name: 'Total balances',
          type: 'line',
          data: chartData
      }]  
    };

    return options;
  }
}
