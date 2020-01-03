import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment'
import { BaseService } from './base.service';

const currenciesApiUrl = environment.baseApiUrl + 'currencies/';

@Injectable({
  providedIn: 'root'
})
export class CurrencyService extends BaseService {

  constructor(private http: HttpClient) {
    super();
  }

  getCurrenciesList() {
    return this.http.get(currenciesApiUrl, this.getDefaultOptions());
  }
}
