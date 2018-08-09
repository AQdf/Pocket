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
      this.totalBalance = this.getTotalBalance(this.assetList);
    });
  }
 
  deleteAsset(id: string) {
    return this.http.delete('http://localhost:58192/api/assets/' + id).pipe(map(res => res.json()));
  }

  private getTotalBalance(assetList: Asset[])
  {
    var total = 0;

    assetList.forEach(asset => {
      this.http.get('http://free.currencyconverterapi.com/api/v5/convert?q=' + asset.currencyName + '_UAH&compact=y').pipe(
        map((data : Response) =>{
          return data.json() as ExchangeRate;
        })
      ).subscribe(x => {
        asset.exchangeRate = x[asset.currencyName + '_UAH'].val;
        asset.baseCurrencyBalance = asset.balance * asset.exchangeRate;

        this.totalBalance += asset.baseCurrencyBalance;
      });
    });

    return total;
    //return this.assetList.map(a => a.balance).reduce((sum, current) => sum + current);
  }
}

export class ExchangeRate {
  currenciesPair : {
    val: number
  };
}
