import { Component, OnInit } from '@angular/core';
import { Chart } from 'angular-highcharts';

import { BalanceService } from '../../services/balance.service'
import { BalancesTotalService } from '../../services/balances-total.service'

import { Balance } from'../../models/balance.model'
import { BalanceTotal } from '../../models/balance-total.model';

@Component({
  selector: 'app-balances-chart',
  templateUrl: './balances-chart.component.html',
  styleUrls: ['./balances-chart.component.css']
})
export class BalancesChartComponent implements OnInit {

  balances: Balance[];
  chart: Chart;
  uahBalanceChangeChart: Chart;
  usdBalanceChangeChart: Chart;

  constructor(private balanceService : BalanceService, private balancesTotalService: BalancesTotalService) { }

  ngOnInit() {
    this.initBalancePieChartData();
    this.initUahBalanceLineChart();
    this.initUsdBalanceLineChart();
  }

  initBalancePieChartData()
  {
    this.balanceService.getLatestBalances().subscribe(balances => {
      this.balances = balances.items;
      this.createBalanceChart(this.balances);
    });
  }

  initUahBalanceLineChart()
  {
    this.balancesTotalService.getBalanceCurrencyTotal('UAH').subscribe(totals => {
      if (!totals || totals.length === 0) {
        return;
      }

      if (totals[0].currency === 'UAH') {
        this.uahBalanceChangeChart = this.createBalanceChangeChart(totals, "#E4D354");
      } else {
        this.usdBalanceChangeChart = this.createBalanceChangeChart(totals, "#90ED7D");
      }
    });
  }

  initUsdBalanceLineChart()
  {
    this.balancesTotalService.getBalanceCurrencyTotal('USD').subscribe(totals => {
      if (!totals || totals.length === 0) {
        return;
      }

      if (totals[0].currency === 'UAH') {
        this.uahBalanceChangeChart = this.createBalanceChangeChart(totals, "#E4D354");
      } else {
        this.usdBalanceChangeChart = this.createBalanceChangeChart(totals, "#90ED7D");
      }
    });
  }

  createBalanceChart(balances: Balance[])
  {
    let chartData = [];  
    for (var i = 0; i < balances.length; i++) {  
        chartData.push({  
            "name": balances[i].asset.name,  
            "y": balances[i].defaultCurrencyValue,  
            sliced: true,  
            selected: true  
        })  
    }

    let effectiveDate = balances[0].effectiveDate;

    this.chart = new Chart({  
        chart: {  
            plotBackgroundColor: null,  
            plotBorderWidth: null,  
            plotShadow: false,  
            type: 'pie',  
            backgroundColor: null,  
            options3d: {  
                enabled: true,  
                alpha: 45,  
                beta: 0  
            },
            height: 700
        },  
        title: {  
            text: 'Latest balances',  
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
    });
  }

  createBalanceChangeChart(totals: BalanceTotal[], currencyColor: string)
  {
    let chartData = [];  
    for (var i = 0; i < totals.length; i++) {
      var formattedDate = new Date(totals[i].effectiveDate);
      var ticks = Date.UTC(formattedDate.getUTCFullYear(), formattedDate.getUTCMonth(), formattedDate.getUTCDate())
      var formattedValue = Number.parseFloat(totals[i].value.toFixed(2));
      chartData.push([ticks, formattedValue]);
    }

    let currency = totals[0].currency;

    return new Chart({  
        chart: {
          type: 'line',
        },
        title: {
            text: 'Balances change'
        },
        subtitle: {
            text: currency
        },
        xAxis: {
            type: 'datetime',
        },
        plotOptions: {
            line: {
                dataLabels: {
                    enabled: true
                },
                color: currencyColor,
                enableMouseTracking: true
            }
        },
        series: [{  
          name: 'Total balances',
          type: 'line',
          data: chartData
      }]  
    })
  }
}
