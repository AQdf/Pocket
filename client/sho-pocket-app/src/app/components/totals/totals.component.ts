import { Component, OnInit } from '@angular/core';

import { BalanceService } from '../../services/balance.service'

@Component({
  selector: 'app-totals',
  templateUrl: './totals.component.html',
  styleUrls: ['./totals.component.css']
})
export class TotalsComponent implements OnInit {

  constructor(public balanceService : BalanceService) { }

  ngOnInit() {
    this.balanceService.getTotalBalance();
  }

}
