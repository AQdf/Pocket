import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

import { environment } from '../../../environments/environment'

const balancesApiUrl = environment.baseApiUrl + 'balances/';

@Component({
  selector: 'app-totals',
  templateUrl: './totals.component.html',
  styleUrls: ['./totals.component.css']
})
export class TotalsComponent implements OnInit {
  totalBalance: number;

  constructor(public client : HttpClient) { }

  ngOnInit() {
    this.getCurrentTotalBalance();
  }

  getCurrentTotalBalance()
  {
    this.client.get<number>(balancesApiUrl + 'total').pipe(
      map((data : number) =>{
        return data;
      })
    ).subscribe(response => {
      this.totalBalance = response;
    });
  }
}