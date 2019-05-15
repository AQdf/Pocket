import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestMethod, ResponseContentType } from '@angular/http';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';

import { map } from 'rxjs/operators';
import { saveAs } from 'file-saver';

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

  getHeaders() {
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let authToken = localStorage.getItem('auth_token');
    headers.append('Authorization', `Bearer ${authToken}`);

    return headers;
  }

  getCurrentTotalBalance() {
    let headers = this.getHeaders();
    let requestOptions = new RequestOptions({method : RequestMethod.Get, headers : headers});

    return this.http.get(balancesApiUrl + 'total', requestOptions).pipe(
      map(data => {
        return data.json( ) as BalanceTotal[];
      })
    )
  }

  getBalanceList(effectiveDate: string) {
    let headers = this.getHeaders();
    let requestOptions = new RequestOptions({method : RequestMethod.Get, headers : headers});

    this.http.get(balancesApiUrl + effectiveDate, requestOptions).pipe(
      map(data => {
        return data.json() as Balances;
      })
    ).subscribe(balances => {
      this.balances = balances.items;
      this.totalBalance = balances.totalBalance;
      this.exchangeRates = balances.exchangeRates;
      this.createBalanceChart(this.balances);
    });
  }

  getBalance(id: string) {
    let headers = this.getHeaders();
    let requestOptions = new RequestOptions({method : RequestMethod.Get, headers : headers});

    return this.http.get(balancesApiUrl + id, requestOptions).pipe(
      map(data => {
        return data.json() as Balance
      })
    );
  }

  postBalance(emp : Balance) {
    var body = JSON.stringify(emp);
    var headers = this.getHeaders();
    var requestOptions = new RequestOptions({method : RequestMethod.Post,headers : headers});

    return this.http.post(balancesApiUrl, body, requestOptions).pipe(
      map(x => x.json())
    );
  }
 
  putBalance(id, balance) {
    var updateModel = {
      value: balance.Value
    };
    var body = JSON.stringify(updateModel);
    var headers = this.getHeaders();
    var requestOptions = new RequestOptions({ method: RequestMethod.Put, headers: headers });

    return this.http.put(balancesApiUrl + id, body, requestOptions).pipe(
        map(res => res.json())
      );
  }

  deleteBalance(id: string) {
    var headers = this.getHeaders();
    var requestOptions = new RequestOptions({method : RequestMethod.Delete, headers : headers});

    return this.http.delete(balancesApiUrl + id, requestOptions).pipe(
        map(res => res.json())
      );
  }

  getEffectiveDatesList(){
    var headers = this.getHeaders();
    var requestOptions = new RequestOptions({method : RequestMethod.Get, headers : headers});

    this.http.get(balancesApiUrl + 'effective-dates', requestOptions).pipe(
      map(data => {
        return data.json() as string[];
      }),
    ).subscribe(x => {
      this.effectiveDatesList = x;

      if (this.effectiveDatesList.length > 0)
      {
        this.getBalanceList(this.effectiveDatesList[0]);
        this.selectedEffectiveDate = this.effectiveDatesList[0];
      } else {
        this.balances = null;
      }
    });
  }

  addBalancesByTemplate() {
    var emptyBody = JSON.stringify('');
    var headers = this.getHeaders();

    this.http.post(balancesApiUrl + 'template', emptyBody, {headers}).pipe(
      map(data => {
        return data.json() as boolean;
      })
    ).subscribe(success => {
      if (success) {
        this.reload();
      }
    });
  }

  downloadCsv() {
    let headers = new Headers({ 'Content-Type': 'blob' });
    let authToken = localStorage.getItem('auth_token');
    headers.append('Authorization', `Bearer ${authToken}`);
    var requestOptions = new RequestOptions({method : RequestMethod.Get, headers : headers, responseType: ResponseContentType.Blob});
    
    const monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

    this.http.get(balancesApiUrl + 'csv', requestOptions).subscribe(response => {
      if (response) {
        var currentDate = new Date(this.selectedEffectiveDate);
        var day = currentDate.getDate();
        var month = monthNames[currentDate.getMonth()];
        var year = currentDate.getFullYear();
        let name = 'Balances_' + day + '_' + month + '_' + year + '.csv';
        saveAs(response.blob(), name);
        return response;
      }
    });
  }

  applyExchangeRate(model: ExchangeRate) {
    var body = JSON.stringify(model);
    var headers = this.getHeaders();
    var requestOptions = new RequestOptions({method : RequestMethod.Put, headers : headers});

    this.http.put(balancesApiUrl + 'exchange-rate', body, requestOptions).pipe(
      map(data => {
        return data.json() as boolean;
      })
    ).subscribe(success => {
      if (success) {
        this.reload();
      }
    });
  }

  reload() {
    this.getEffectiveDatesList();
    this.setBalanceCurrencyTotal("UAH");
    this.setBalanceCurrencyTotal("USD");
  }

  setBalanceCurrencyTotal(currency: string) {
    var headers = this.getHeaders();
    let queryParams = new HttpParams().set('count', '100');
    var requestOptions = new RequestOptions({method : RequestMethod.Get, headers : headers, params: queryParams});

    this.http.get(balancesApiUrl + 'currency-totals/' + currency, requestOptions).pipe(
      map(data => {
        return data.json() as BalanceTotal[];
      })
    ).subscribe(totals => {
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
