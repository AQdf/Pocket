import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestMethod } from '@angular/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import {Balance} from'../models/balance.model'

@Injectable({
  providedIn: 'root'
})
export class BalanceService {
  selectedBalance: Balance;
  balanceList: Balance[];
  totalBalance: number;

  constructor(public http: Http) {}

  getBalanceList(){
    this.http.get('http://localhost:58192/api/balances').pipe(
      map((data : Response) =>{
        return data.json() as Balance[];
      })
    ).subscribe(x => {
      this.balanceList = x;
      this.getTotalBalance();
    });
  }

  postBalance(emp : Balance) {
    var body = JSON.stringify(emp);
    var headerOptions = new Headers({'Content-Type':'application/json'});
    var requestOptions = new RequestOptions({method : RequestMethod.Post,headers : headerOptions});
    return this.http.post('http://localhost:58192/api/balances',body,requestOptions).pipe(
      map(x => x.json())
    );
  }
 
  putBalance(id, emp) {
    var body = JSON.stringify(emp);
    var headerOptions = new Headers({ 'Content-Type': 'application/json' });
    var requestOptions = new RequestOptions({ method: RequestMethod.Put, headers: headerOptions });
    return this.http.put('http://localhost:58192/api/balances/' + id,
      body,
      requestOptions).pipe(
        map(res => res.json())
      );
  }

  deleteBalance(id: string) {
    return this.http.delete('http://localhost:58192/api/balances/' + id).pipe(map(res => res.json()));
  }

  getTotalBalance()
  {
    this.http.get('http://localhost:58192/api/balances/total/').pipe(
      map((data : Response) =>{
        return data.json() as number;
      })
    ).subscribe(response => {
      this.totalBalance = response;
    });
  }
}
