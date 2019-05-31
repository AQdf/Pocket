import { Component, OnInit } from '@angular/core';

import { BalanceTotal } from '../../models/balance-total.model';
import { BalancesTotalService } from '../../services/balances-total.service';

@Component({
  selector: 'app-totals',
  templateUrl: './totals.component.html',
  styleUrls: ['./totals.component.css']
})
export class TotalsComponent implements OnInit {
  totalBalance: BalanceTotal[];

  constructor(public balancesTotalService : BalancesTotalService) { }

  ngOnInit() {
    this.getCurrentTotalBalance();
  }

  getCurrentTotalBalance()
  {
    this.balancesTotalService.getCurrentTotalBalance().subscribe(response => {
      this.totalBalance = response;
    });
  }
}