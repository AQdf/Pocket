import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestMethod } from '@angular/http';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';

import { of, from } from 'rxjs';
import { map } from 'rxjs/operators';

import { Balance } from'../models/balance.model'
import { Balances } from'../models/balances.model'
import { ExchangeRate } from '../models/exchange-rate.model';

import { environment } from '../../environments/environment';
import { BalanceTotal } from '../models/balance-total.model';
import { Chart } from 'angular-highcharts';

const balancesApiUrl = environment.baseApiUrl + 'balances/';

@Injectable({
  providedIn: 'root'
})
export class BalanceService {
  selectedBalance: Balance;
  balances: Balance[];
  totalBalance: BalanceTotal[];
  effectiveDatesList: string[];
  selectedEffectiveDate: string;
  exchangeRates: ExchangeRate[];
  chart: Chart;

  uahTotalHistory: BalanceTotal[];
  uahBalanceChangeChart: Chart;
  usdBalanceChangeChart: Chart;

  constructor(public http: Http, public client: HttpClient) {
    this.getEffectiveDatesList();
  }

  getBalanceList(effectiveDate: string){
    //let params = new HttpParams().set('effectiveDate', effectiveDate || '');
    //this.client.get<Balances>(balancesApiUrl, { params }).pipe(

    this.client.get<Balances>(balancesApiUrl + effectiveDate).pipe(
      map((data : Balances) => {
        return data;
      })
    ).subscribe(balances => {
      this.balances = balances.items;
      this.totalBalance = balances.totalBalance;
      this.exchangeRates = balances.exchangeRates;
      this.createBalanceChart(this.balances);
    });
  }

  getBalance(id: string) {
    return this.client.get<Balance>(balancesApiUrl + id).pipe(
      map((data : Balance) => data)
    );
  }

  postBalance(emp : Balance) {
    var body = JSON.stringify(emp);
    var headerOptions = new Headers({'Content-Type':'application/json'});
    var requestOptions = new RequestOptions({method : RequestMethod.Post,headers : headerOptions});

    return this.http.post(balancesApiUrl, body, requestOptions).pipe(
        map(x => x.json())
      );
  }
 
  putBalance(id, emp) {
    var body = JSON.stringify(emp);
    var headerOptions = new Headers({ 'Content-Type': 'application/json' });
    var requestOptions = new RequestOptions({ method: RequestMethod.Put, headers: headerOptions });

    return this.http.put(balancesApiUrl + id, body, requestOptions).pipe(
        map(res => res.json())
      );
  }

  deleteBalance(id: string) {
    return this.http.delete(balancesApiUrl + id).pipe(
        map(res => res.json())
      );
  }

  getEffectiveDatesList(){
    this.client.get<string[]>(balancesApiUrl + 'effective-dates').pipe(
      map((data : string[]) => {
        return data;
      })
    ).subscribe(x => {
      this.effectiveDatesList = x;

      if (this.effectiveDatesList.length > 0)
      {
        this.getBalanceList(this.effectiveDatesList[0]);
        this.selectedEffectiveDate = this.effectiveDatesList[0];
      }
    });
  }

  addBalancesByTemplate() {
    var emptyBody = JSON.stringify('');
    const headers = new HttpHeaders().set('Content-Type', 'application/json');

    this.client.post(balancesApiUrl + 'template', emptyBody, {headers}).pipe(
      map((data : boolean) => {
        return data;
      })
    ).subscribe(success => {
      if (success) {
        this.reload();
      }
    });    
  }

  applyExchangeRate(model: ExchangeRate) {
    var body = JSON.stringify(model);
    const headers = new HttpHeaders().set('Content-Type', 'application/json');

    this.client.put(balancesApiUrl + 'exchange-rate', body, {headers}).pipe(
      map((data : boolean) => {
        return data;
      })
    ).subscribe(success => {
      if (success) {
        this.reload();
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

  setBalanceCurrencyTotal(currencyId: string) {
    let queryParams = new HttpParams().set('count', '100');
    this.client.get<BalanceTotal[]>(balancesApiUrl + 'currency-totals/' + currencyId, { params: queryParams }).pipe(
      map((data : BalanceTotal[]) => {
        return data;
      })
    ).subscribe(totals => {
      if (totals[0].currency === 'UAH') {
        this.uahBalanceChangeChart = this.createBalanceChangeChart(totals, "#E4D354");
      } else {
        this.usdBalanceChangeChart = this.createBalanceChangeChart(totals, "#90ED7D");
      }
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

  reload() {
    this.getEffectiveDatesList();
    this.setBalanceCurrencyTotal("5e28d490-961f-4251-816c-e467450a2499");
    this.setBalanceCurrencyTotal("8d51fd2c-1ab6-4f07-a2c0-1ea4ed5a12fc");
  }
}
