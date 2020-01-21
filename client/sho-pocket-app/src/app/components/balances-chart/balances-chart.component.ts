import { Component, OnInit } from '@angular/core';
import { Chart } from 'angular-highcharts';

import { BalancesTotalService } from '../../services/balances-total.service';
import { BalanceChanges } from '../../models/balance-changes.model';
import { BalancePrimaryCurrency } from '../../models/balance-primary-currency.model';

@Component({
  selector: 'app-balances-chart',
  templateUrl: './balances-chart.component.html',
  styleUrls: ['./balances-chart.component.css']
})
export class BalancesChartComponent implements OnInit {

  chart: Chart;
  balanceChangeCharts: Chart[];

  constructor(private balancesTotalService: BalancesTotalService) { }

  ngOnInit() {
    this.initBalancePieChartData();
    this.initBalanceChangeLineCharts();
  }

  initBalancePieChartData()
  {
    this.balancesTotalService.getBalancesInUserPrimaryCurrency().subscribe((balances: BalancePrimaryCurrency[]) => {
      if (balances) {
        this.createBalancePieChart(balances);
      }
    });
  }

  initBalanceChangeLineCharts() {
    this.balancesTotalService.getBalanceTotalChanges().subscribe((currenciesTotals:any) => {
      if (!currenciesTotals || currenciesTotals.length === 0) {
        return;
      }

      this.balanceChangeCharts = new Array();
      currenciesTotals.forEach(c => this.balanceChangeCharts.push(this.createBalanceChangeChart(c)));
    });
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

    this.chart = new Chart({  
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
    });
  }

  createBalanceChangeChart(changes: BalanceChanges)
  {
    let chartData = [];
    let totals = changes.values;
    for (var i = 0; i < totals.length; i++) {
      var formattedDate = new Date(totals[i].effectiveDate);
      var ticks = Date.UTC(formattedDate.getUTCFullYear(), formattedDate.getUTCMonth(), formattedDate.getUTCDate())
      var formattedValue = Number.parseFloat(totals[i].value.toFixed(2));
      chartData.push([ticks, formattedValue]);
    }

    let currency = changes.currency;

    return new Chart({  
        chart: {
          backgroundColor: "#f9f9f9",
          type: 'line',
        },
        title: {
            text: currency
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
    })
  }
}
