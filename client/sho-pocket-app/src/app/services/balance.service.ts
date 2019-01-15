import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestMethod } from '@angular/http';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';

import { of, from } from 'rxjs';
import { map } from 'rxjs/operators';

import { Balance } from'../models/balance.model'
import { Balances } from'../models/balances.model'
import { ExchangeRate } from '../models/exchange-rate.model';

const baseUrl = 'http://localhost:58192/api/balances/';

@Injectable({
  providedIn: 'root'
})
export class BalanceService {
  selectedBalance: Balance;
  balances: Balance[];
  totalBalance: number;
  effectiveDatesList: string[];
  selectedEffectiveDate: string;
  exchangeRates: ExchangeRate[];

  constructor(public http: Http, public client: HttpClient) {
    this.getEffectiveDatesList();
  }

  getBalanceList(effectiveDate: string){
    let params = new HttpParams().set('effectiveDate', effectiveDate || '');

    this.client.get<Balances>(baseUrl, {params}).pipe(
      map((data : Balances) => {
        return data;
      })
    ).subscribe(balances => {
      this.balances = balances.items;
      this.totalBalance = balances.totalBalance;
      this.exchangeRates = balances.exchangeRates;
    });
  }

  getBalance(id: string) {
    return this.client.get<Balance>(baseUrl + id).pipe(
      map((data : Balance) => data)
    );
  }

  postBalance(emp : Balance) {
    var body = JSON.stringify(emp);
    var headerOptions = new Headers({'Content-Type':'application/json'});
    var requestOptions = new RequestOptions({method : RequestMethod.Post,headers : headerOptions});

    return this.http.post(baseUrl, body, requestOptions).pipe(
        map(x => x.json())
      );
  }
 
  putBalance(id, emp) {
    var body = JSON.stringify(emp);
    var headerOptions = new Headers({ 'Content-Type': 'application/json' });
    var requestOptions = new RequestOptions({ method: RequestMethod.Put, headers: headerOptions });

    return this.http.put(baseUrl + id, body, requestOptions).pipe(
        map(res => res.json())
      );
  }

  deleteBalance(id: string) {
    return this.http.delete(baseUrl + id).pipe(
        map(res => res.json())
      );
  }

  getEffectiveDatesList(){
    this.client.get<string[]>(baseUrl + 'effective-dates').pipe(
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

    this.client.post(baseUrl + 'template', emptyBody, {headers}).pipe(
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

    this.client.put(baseUrl + 'exchange-rate', body, {headers}).pipe(
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
  }
}
