import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestMethod } from '@angular/http';
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
 
  putBalance(id, balance) {
    var updateModel = {
      value: balance.Value
    };
    var body = JSON.stringify(updateModel);
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

  downloadCsv() {
    const headers = new HttpHeaders().set('Content-Type', 'blob');
    const options: {
      headers?: HttpHeaders,
      observe?: 'body',
      params?: HttpParams,
      reportProgress?: boolean,
      responseType: 'blob',
      withCredentials?: boolean
    } = {
        headers: headers,
        responseType: 'blob'
    };

    const monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

    this.client.get(balancesApiUrl + 'csv', options).subscribe(response => {
      if (response) {
        debugger;
        var currentDate = new Date(this.selectedEffectiveDate);
        var day = currentDate.getDate();
        var month = monthNames[currentDate.getMonth()];
        var year = currentDate.getFullYear();
        let name = 'Balances_' + day + '_' + month + '_' + year + '.csv';
        saveAs(response, name);
        return response;
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

  reload() {
    this.getEffectiveDatesList();
    this.setBalanceCurrencyTotal("UAH");
    this.setBalanceCurrencyTotal("USD");
  }

  setBalanceCurrencyTotal(currencyName: string) {
    let queryParams = new HttpParams().set('count', '100');
    this.client.get<BalanceTotal[]>(balancesApiUrl + 'currency-totals/' + currencyName, { params: queryParams }).pipe(
      map((data : BalanceTotal[]) => {
        return data;
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
