import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
 
import { Asset } from'../models/asset.model'

import { environment } from '../../environments/environment'
import { BaseService } from './base.service';

const assetsApiUrl = environment.baseApiUrl + 'assets/';

@Injectable({
  providedIn: 'root'
})
export class AssetService extends BaseService {

  // TODO: Remove this property
  currenciesList: string[];

  constructor(private http: HttpClient) {
    super();
  }

  getAsset(id: string) {
    return this.http.get(assetsApiUrl + id, this.getDefaultOptions());
  }

  postAsset(assetData : Asset) {
    var body = JSON.stringify(assetData);
    return this.http.post(assetsApiUrl, body, this.getDefaultOptions());
  }
 
  putAsset(id: string, assetData: Asset) {
    var body = JSON.stringify(assetData);
    return this.http.put(assetsApiUrl + id, body, this.getDefaultOptions());
  }
 
  getAssetList() {
    let includeInactive = true;
    return this.http.get(assetsApiUrl + "all/" + includeInactive, this.getDefaultOptions());
  }
 
  deleteAsset(id: string) {
    return this.http.delete(assetsApiUrl + id, this.getDefaultOptions());
  }

  getCurrenciesList() {
    this.http.get(environment.baseApiUrl + 'currencies', this.getDefaultOptions())
    .subscribe((response: string[]) => {
      this.currenciesList = response;
    });
  }
}