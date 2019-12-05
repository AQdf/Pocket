import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpEventType } from '@angular/common/http';
import { saveAs } from 'file-saver';

import { environment } from '../../environments/environment';
import { Balance } from'../models/balance.model'
import { BaseService } from './base.service';

const balancesApiUrl = environment.baseApiUrl + 'balances/';

@Injectable({
  providedIn: 'root'
})
export class BalanceService extends BaseService {

  constructor(public http: HttpClient) {
    super();
  }

  getLatestBalances() {
    return this.http.get(balancesApiUrl + 'latest', this.getDefaultOptions());
  }

  getBalanceList(effectiveDate: string) {
    return this.http.get(balancesApiUrl + 'date/' + effectiveDate, this.getDefaultOptions());
  }

  getBalance(id: string) {
    return this.http.get(balancesApiUrl + id, this.getDefaultOptions());
  }

  postBalance(balanceData : Balance) {
    var body = JSON.stringify(balanceData);
    return this.http.post(balancesApiUrl, body, this.getDefaultOptions());
  }
 
  putBalance(id: string, balanceData: Balance) {
    var updateModel = {
      assetId: balanceData.assetId,
      value: balanceData.value
    };
    var body = JSON.stringify(updateModel);
    return this.http.put(balancesApiUrl + id, body, this.getDefaultOptions());
  }

  deleteBalance(id: string) {
    return this.http.delete(balancesApiUrl + id, this.getDefaultOptions());
  }

  loadEffectiveDatesList() {
    return this.http.get(balancesApiUrl + 'effective-dates', this.getDefaultOptions());
  }

  addBalancesByTemplate() {
    var emptyBody = JSON.stringify('');
    return this.http.post(balancesApiUrl + 'template', emptyBody, this.getDefaultOptions());
  }

  exportAllCsv(effectiveDate: string) {
    let headers = new HttpHeaders({ 'Content-Type': 'blob' });
    const monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

    this.http.get(balancesApiUrl + 'export/csv', { headers, responseType: 'blob' })
    .subscribe(response => {
      if (response) {
        var currentDate = new Date(effectiveDate);
        var day = currentDate.getDate();
        var month = monthNames[currentDate.getMonth()];
        var year = currentDate.getFullYear();
        let name = 'Balances_' + day + '_' + month + '_' + year + '.csv';
        saveAs(response, name);
        return response;
      }
    });
  }

  exportJson(effectiveDate: string) {
    let headers = new HttpHeaders({ 'Content-Type': 'blob' });
    const monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

    this.http.get(balancesApiUrl + 'export/json/' + effectiveDate, { headers, responseType: 'blob' })
    .subscribe(response => {
      if (response) {
        var currentDate = new Date(effectiveDate);
        var day = currentDate.getDate();
        var month = monthNames[currentDate.getMonth()];
        var year = currentDate.getFullYear();
        let name = 'Balances_' + day + '_' + month + '_' + year + '.json';
        saveAs(response, name);
        return response;
      }
    });
  }

  importJson(files: File[]) {
    if (files.length === 0) {
      return;
    }
 
    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
 
    return this.http.post(balancesApiUrl + 'import/json', formData, {reportProgress: true, observe: 'events'});
  }

  reload() {
    this.loadEffectiveDatesList();
  }
}
