import { Component, OnInit } from '@angular/core';
import { Chart } from 'angular-highcharts';

import { BalanceService } from '../../services/balance.service'
import { BalancesTotalService } from '../../services/balances-total.service'

import { Balance } from'../../models/balance.model'
import { BalanceChanges } from '../../models/balance-changes.model';

@Component({
  selector: 'app-balances-chart',
  templateUrl: './balances-chart.component.html',
  styleUrls: ['./balances-chart.component.css']
})
export class BalancesChartComponent implements OnInit {

  balances: Balance[];
  chart: Chart;
  balanceChangeCharts: Chart[];

  constructor(private balanceService : BalanceService, private balancesTotalService: BalancesTotalService) { }

  ngOnInit() {
    this.initBalancePieChartData();
    this.initBalanceChangeLineCharts();
  }

  initBalancePieChartData()
  {
    this.balanceService.getLatestBalances()
    .subscribe((balances: any) => {
      if (balances) {
        this.balances = balances.items;
        this.createBalanceChart(this.balances);
      }
    });
  }

  initBalanceChangeLineCharts() {
    this.balancesTotalService.getBalanceTotalChanges().subscribe((currenciesTotals:any) => {
      if (!currenciesTotals || currenciesTotals.length === 0) {
        return;
      }

      this.balanceChangeCharts = new Array(currenciesTotals.length);
      currenciesTotals.forEach(c => this.balanceChangeCharts.push(this.createBalanceChangeChart(c)));
    });
  }

  createBalanceChart(balances: Balance[]) {
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
