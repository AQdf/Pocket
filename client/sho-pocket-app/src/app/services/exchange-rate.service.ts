import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { ExchangeRate } from '../models/exchange-rate.model';
import { BaseService } from './base.service';

const exchangeRateApiUrl = environment.baseApiUrl + 'exchange-rates/';

@Injectable({
  providedIn: 'root'
})
export class ExchangeRateService extends BaseService {

  constructor(public http: HttpClient) {
    super();
  }

  getExchangeRates(effectiveDate: string) {
    return this.http.get(exchangeRateApiUrl + effectiveDate, this.getDefaultOptions());
  }

  applyExchangeRate(model: ExchangeRate) {
    let body = JSON.stringify(model);
    return this.http.put(exchangeRateApiUrl, body, this.getDefaultOptions());
  }
}
