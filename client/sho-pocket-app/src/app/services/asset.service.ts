import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestMethod } from '@angular/http';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';
 
import { Asset } from'../models/asset.model'

import { environment } from '../../environments/environment'

const assetsApiUrl = environment.baseApiUrl + 'assets/';

@Injectable({
  providedIn: 'root'
})
export class AssetService {
  selectedAsset: Asset;
  assetList: Asset[];
  currenciesList: string[];

  constructor(public http: Http, public client: HttpClient) {
    this.getAssetList();
  }

  getHeaders() {
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let authToken = localStorage.getItem('auth_token');
    headers.append('Authorization', `Bearer ${authToken}`);

    return headers;
  }

  getAsset(id: string) {
    let headers = this.getHeaders();
    let requestOptions = new RequestOptions({method : RequestMethod.Get, headers : headers});

    return this.http.get(assetsApiUrl + id, requestOptions).pipe(
      map((data : Response) => {
        return data.json() as Asset
      })
    );
  }

  postAsset(emp : Asset) {
    var body = JSON.stringify(emp);
    var headers = this.getHeaders();
    var requestOptions = new RequestOptions({method : RequestMethod.Post, headers : headers});

    return this.http.post(assetsApiUrl, body, requestOptions).pipe(
      map((data : Response) =>{
        return data.json() as Asset;
      })
    );
  }
 
  putAsset(id, emp) {
    var body = JSON.stringify(emp);
    var headers = this.getHeaders();
    var requestOptions = new RequestOptions({ method: RequestMethod.Put, headers: headers });

    return this.http.put(assetsApiUrl + id, body, requestOptions).pipe(
      map(response => response.json())
    );
  }
 
  getAssetList() {
    var headers = this.getHeaders();
    var requestOptions = new RequestOptions({ method: RequestMethod.Get, headers: headers });

    this.http.get(assetsApiUrl, requestOptions).pipe(
      map(data => {
        return data.json() as Asset[];
      })
    ).subscribe(assets => {
        this.assetList = assets;
    });
  }
 
  deleteAsset(id: string) {
    var headers = this.getHeaders();
    var requestOptions = new RequestOptions({ method: RequestMethod.Get, headers: headers });

    return this.http.delete(assetsApiUrl + id, requestOptions).pipe(
      map(response => response.json())
    );
  }

  getCurrenciesList() {
    var headers = this.getHeaders();
    var requestOptions = new RequestOptions({ method: RequestMethod.Get, headers: headers });

    this.http.get(environment.baseApiUrl + 'currencies', requestOptions).pipe(
      map((data : Response) =>{
        return data.json() as string[];
      })
    ).subscribe(x => {
      this.currenciesList = x;
    });
  }
}