import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, RequestMethod, ResponseContentType } from '@angular/http';

import { map } from 'rxjs/operators';
import { saveAs } from 'file-saver';

import { Balance } from'../models/balance.model'
import { Balances } from'../models/balances.model'
import { ExchangeRate } from '../models/exchange-rate.model';

import { environment } from '../../environments/environment';
import { BalanceTotal } from '../models/balance-total.model';

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
  uahTotalHistory: BalanceTotal[];

  constructor(public http: Http) {
    this.getEffectiveDatesList();
  }

  getHeaders() {
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let authToken = localStorage.getItem('auth_token');
    headers.append('Authorization', `Bearer ${authToken}`);

    return headers;
  }

  getLatestBalances() {
    let headers = this.getHeaders();
    let requestOptions = new RequestOptions({method : RequestMethod.Get, headers : headers});

    return this.http.get(balancesApiUrl + 'latest', requestOptions).pipe(
      map(data => {
        return data.json() as Balances;
      })
    );
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

    return this.http.post(balancesApiUrl + 'template', emptyBody, {headers}).pipe(
      map(res => res.json())
    );
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

  reload() {
    this.getEffectiveDatesList();
  }
}
