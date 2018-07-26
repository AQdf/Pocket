import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestMethod } from '@angular/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
 
import {Asset} from'./asset.model'

@Injectable({
  providedIn: 'root'
})
export class AssetService {

  selectedAsset: Asset;
  assetList: Asset[];
  totalBalance: number;

  constructor(public http: Http) {}

  getTotalBalance() {
    this.http.get('http://localhost:58192/api/assets/total-balance').pipe(
      map((data : Response) =>{
        return data.json() as number;
      })
    ).subscribe(x => {
      this.totalBalance = x;
    });
  }

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
 
  getAssetList(){
    this.http.get('http://localhost:58192/api/assets').pipe(
      map((data : Response) =>{
        return data.json() as Asset[];
      })
    ).subscribe(x => {
      this.assetList = x;
      this.totalBalance = this.assetList.map(a => a.balance).reduce((sum, current) => sum + current);
    });
  }
 
  deleteAsset(id: string) {
    return this.http.delete('http://localhost:58192/api/assets/' + id).pipe(map(res => res.json()));
  }
}
