import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, RequestMethod } from '@angular/http';
import { HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { BalanceTotal } from '../models/balance-total.model';

const balancesTotalApiUrl = environment.baseApiUrl + 'balances-total/';

@Injectable({
  providedIn: 'root'
})
export class BalancesTotalService {

  constructor(private http: Http) { }

  getHeaders() {
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let authToken = localStorage.getItem('auth_token');
    headers.append('Authorization', `Bearer ${authToken}`);

    return headers;
  }

  getCurrentTotalBalance() {
    let headers = this.getHeaders();
    let requestOptions = new RequestOptions({method : RequestMethod.Get, headers : headers});

    return this.http.get(balancesTotalApiUrl, requestOptions).pipe(
      map(data => {
        return data.json( ) as BalanceTotal[];
      })
    )
  }

  getBalanceCurrencyTotal(currency: string) {
    var headers = this.getHeaders();
    let queryParams = new HttpParams().set('count', '100');
    var requestOptions = new RequestOptions({method : RequestMethod.Get, headers : headers, params: queryParams});

    return this.http.get(balancesTotalApiUrl + currency, requestOptions).pipe(
      map(data => {
        return data.json() as BalanceTotal[];
      })
    );
  }
}
