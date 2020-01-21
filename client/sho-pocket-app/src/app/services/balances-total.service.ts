import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { BaseService } from './base.service';
import { BalanceTotal } from '../models/balance-total.model';

const balancesTotalApiUrl = environment.baseApiUrl + 'balances-total/';

@Injectable({
  providedIn: 'root'
})
export class BalancesTotalService extends BaseService {

  constructor(private http: HttpClient) {
    super();
  }

  balanceTotals: BalanceTotal[];

  loadCurrentTotalBalance() {
    this.http.get(balancesTotalApiUrl, this.getDefaultOptions())
      .subscribe((totals: BalanceTotal[]) => this.balanceTotals = totals);
  }

  getBalanceTotalChanges() {
    let options = this.getDefaultOptions();
    options.params.set('count', '100');
    return this.http.get(balancesTotalApiUrl + 'changes', options);
  }

  getBalancesInUserPrimaryCurrency() {
    return this.http.get(balancesTotalApiUrl + 'primary-currency-balances', this.getDefaultOptions());
  }
}
