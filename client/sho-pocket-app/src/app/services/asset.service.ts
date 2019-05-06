import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestMethod } from '@angular/http';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map, } from 'rxjs/operators';
 
import { Asset } from'../models/asset.model'
import { Currency } from '../models/currency.model';

import { environment } from '../../environments/environment'

const assetsApiUrl = environment.baseApiUrl + 'assets/';

@Injectable({
  providedIn: 'root'
})
export class AssetService {
  selectedAsset: Asset;
  assetList: Asset[];
  currenciesList: Currency[];

  constructor(public http: Http, public client: HttpClient) {
    this.getAssetList();
  }

  getAsset(id: string) {
    return this.client.get<Asset>(assetsApiUrl + id).pipe(
      map((data : Asset) => data)
    );
  }

  postAsset(emp : Asset) {
    var body = JSON.stringify(emp);
    var headerOptions = new Headers({'Content-Type':'application/json'});
    var requestOptions = new RequestOptions({method : RequestMethod.Post, headers : headerOptions});
    return this.http.post(assetsApiUrl, body, requestOptions).pipe(
      map((data : Response) =>{
        return data.json() as Asset;
      })
    );
  }
 
  putAsset(id, emp) {
    var body = JSON.stringify(emp);
    var headerOptions = new Headers({ 'Content-Type': 'application/json' });
    var requestOptions = new RequestOptions({ method: RequestMethod.Put, headers: headerOptions });
    return this.http.put(assetsApiUrl + id, body, requestOptions).pipe(
        map(response => response.json())
      );
  }
 
  getAssetList() {
    this.client.get<Asset[]>(assetsApiUrl).pipe(
      map((data : Asset[]) => {
        return data;
      })
    ).subscribe(assets => {
        this.assetList = assets;
    });
  }
 
  deleteAsset(id: string) {
    return this.http.delete(assetsApiUrl + id).pipe(
        map(response => response.json())
      );
  }

  getCurrenciesList() {
    this.http.get(assetsApiUrl + 'currencies').pipe(
      map((data : Response) =>{
        return data.json() as Currency[];
      })
    ).subscribe(x => {
      this.currenciesList = x;
    });
  }
}