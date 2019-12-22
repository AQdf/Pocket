import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment'
import { BaseService } from './base.service';
import { BankAccount } from '../models/bank-account';

const bankSyncApiUrl = environment.baseApiUrl + 'bank-sync/';

@Injectable({
  providedIn: 'root'
})
export class BankSyncService extends BaseService {

  constructor(private http: HttpClient) { 
    super();
  }

  getAssetBankSyncData(assetId: string) {
    return this.http.get(bankSyncApiUrl + assetId, this.getDefaultOptions());
  }

  getBanksList() {
    return this.http.get(bankSyncApiUrl + 'banks-lookup/', this.getDefaultOptions());
  }

  submitBankClientAuthData(bankName: string, token: string) {
    let body = JSON.stringify({
      bankName: bankName,
      token: token
    });
    return this.http.post(bankSyncApiUrl + 'auth/', body, this.getDefaultOptions());
  }

  connectAccount(assetId: string, bankName: string, account: BankAccount) {
    let body = JSON.stringify({
      assetId: assetId,
      bankName: bankName,
      bankAccountId: account.id,
      accountName: account.name
    });

    return this.http.post(bankSyncApiUrl + 'connect-account/', body, this.getDefaultOptions());
  }

  disconnectAccount(assetId: string) {
    return this.http.post(bankSyncApiUrl + 'disconnect-account/' + assetId, null, this.getDefaultOptions());
  }
}