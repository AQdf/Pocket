import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestMethod } from '@angular/http';
import { HttpClient, HttpParams } from '@angular/common/http';
import { of } from 'rxjs';
import { map, } from 'rxjs/operators';
 
import {Asset} from'../models/asset.model'
import { AssetType } from '../models/asset-type.model';
import { Currency } from '../models/currency.model';

const baseUrl = 'http://localhost:58192/api/assets/';

@Injectable({
  providedIn: 'root'
})
export class AssetService {

  selectedAsset: Asset;
  assetList: Asset[];
  activeAssets: Asset[];

  assetTypesList: AssetType[];
  currenciesList: Currency[];

  constructor(public http: Http, public client: HttpClient) {}

  postAsset(emp : Asset) {
    var body = JSON.stringify(emp);
    var headerOptions = new Headers({'Content-Type':'application/json'});
    var requestOptions = new RequestOptions({method : RequestMethod.Post,headers : headerOptions});
    return this.http.post('http://localhost:58192/api/assets',body,requestOptions).pipe(
      map(x => x.json())
    );
  }
 
  putAsset(id, emp) {
    var body = JSON.stringify(emp);
    var headerOptions = new Headers({ 'Content-Type': 'application/json' });
    var requestOptions = new RequestOptions({ method: RequestMethod.Put, headers: headerOptions });
    return this.http.put('http://localhost:58192/api/assets/' + id,
      body,
      requestOptions).pipe(
        map(res => res.json())
      );
  }
 
  getAssetList() {
    this.client.get<Asset[]>(baseUrl).pipe(
      map((data : Asset[]) => {
        return data;
      })
    ).subscribe(assets => {
        this.assetList = assets;

        of(assets).pipe(
          map((result: Asset[]) =>
            result.filter(asset => asset.isActive)
          )
        ).subscribe(result => this.activeAssets = result);
    });
  }
 
  deleteAsset(id: string) {
    return this.http.delete('http://localhost:58192/api/assets/' + id).pipe(map(res => res.json()));
  }

  getAssetTypesList() {
    this.http.get('http://localhost:58192/api/assets/types').pipe(
      map((data : Response) =>{
        return data.json() as Asset[];
      })
    ).subscribe(x => {
      this.assetTypesList = x;
    });
  }

  getCurrenciesList() {
    this.http.get('http://localhost:58192/api/assets/currencies').pipe(
      map((data : Response) =>{
        return data.json() as Asset[];
      })
    ).subscribe(x => {
      this.currenciesList = x;
    });
  }
}