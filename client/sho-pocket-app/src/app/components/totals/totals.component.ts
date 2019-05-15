import { Component, OnInit } from '@angular/core';

import { BalanceTotal } from 'src/app/models/balance-total.model';
import { BalanceService } from '../../services/balance.service';

@Component({
  selector: 'app-totals',
  templateUrl: './totals.component.html',
  styleUrls: ['./totals.component.css']
})
export class TotalsComponent implements OnInit {
  totalBalance: BalanceTotal[];

  constructor(public balanceService : BalanceService) { }

  ngOnInit() {
    this.getCurrentTotalBalance();
  }

  getCurrentTotalBalance()
  {
    this.balanceService.getCurrentTotalBalance().subscribe(response => {
      this.totalBalance = response;
    });
  }
}