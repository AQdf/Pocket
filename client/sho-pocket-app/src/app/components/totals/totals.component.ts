import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

import { environment } from '../../../environments/environment'
import { BalanceTotal } from 'src/app/models/balance-total.model';

const balancesApiUrl = environment.baseApiUrl + 'balances/';

@Component({
  selector: 'app-totals',
  templateUrl: './totals.component.html',
  styleUrls: ['./totals.component.css']
})
export class TotalsComponent implements OnInit {
  totalBalance: BalanceTotal[];

  constructor(public client : HttpClient) { }

  ngOnInit() {
    this.getCurrentTotalBalance();
  }

  getCurrentTotalBalance()
  {
    this.client.get<BalanceTotal[]>(balancesApiUrl + 'total').pipe(
      map((data : BalanceTotal[]) =>{
        return data;
      })
    ).subscribe(response => {
      this.totalBalance = response;
    });
  }
}