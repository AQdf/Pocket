import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestMethod } from '@angular/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
 
import { AssetHistory } from'./asset-history.model'

@Injectable({
  providedIn: 'root'
})
export class AssetHistoryService {

  selectedAssetHistory: AssetHistory;
  assetHistoryList: AssetHistory[];

  constructor(public http: Http) {}

  postAssetHistory(assetHistory : AssetHistory) {
    var body = JSON.stringify(assetHistory);
    var headerOptions = new Headers({'Content-Type':'application/json'});
    var requestOptions = new RequestOptions({method : RequestMethod.Post,headers : headerOptions});
    return this.http.post('http://localhost:58192/api/asset-history',body,requestOptions).pipe(
      map(x => x.json())
    );
  }
 
  putAssetHistory(id, assetHistory) {
    var body = JSON.stringify(assetHistory);
    var headerOptions = new Headers({ 'Content-Type': 'application/json' });
    var requestOptions = new RequestOptions({ method: RequestMethod.Put, headers: headerOptions });
    return this.http.put('http://localhost:58192/api/asset-history/' + id,
      body,
      requestOptions).pipe(
        map(res => res.json())
      );
  }
 
  getAssetHistoryList(){
    this.http.get('http://localhost:58192/api/asset-history').pipe(
      map((data : Response) =>{
        return data.json() as AssetHistory[];
      })
    ).subscribe(x => {
      this.assetHistoryList = x;
    });
  }
 
  deleteAssetHistory(id: string) {
    return this.http.delete('http://localhost:58192/api/asset-history/' + id).pipe(map(res => res.json()));
  }
}

export class ExchangeRate {
  currenciesPair : {
    val: number
  };
}
