import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, RequestMethod } from '@angular/http';

import { map } from 'rxjs/operators';

import { ExchangeRate } from '../models/exchange-rate.model';

import { environment } from '../../environments/environment';

const exchangeRateApiUrl = environment.baseApiUrl + 'exchange-rates/';

@Injectable({
  providedIn: 'root'
})
export class ExchangeRateService {

  constructor(public http: Http) { }

  getHeaders() {
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let authToken = localStorage.getItem('auth_token');
    headers.append('Authorization', `Bearer ${authToken}`);

    return headers;
  }

  applyExchangeRate(model: ExchangeRate) {
    var body = JSON.stringify(model);
    var headers = this.getHeaders();
    var requestOptions = new RequestOptions({method : RequestMethod.Put, headers : headers});

    return this.http.put(exchangeRateApiUrl, body, requestOptions).pipe(
      map(data => {
        return data.json() as boolean;
      })
    );
  }
}
