import { Component, OnInit, HostListener } from '@angular/core';

import { BalancesTotalService } from '../../services/balances-total.service';

@Component({
  selector: 'app-totals',
  templateUrl: './totals.component.html',
  styleUrls: ['./totals.component.css']
})
export class TotalsComponent implements OnInit {

  constructor(public balancesTotalService : BalancesTotalService) { }

  ngOnInit() {
    this.loadCurrentTotalBalance();
  }

  loadCurrentTotalBalance() {
    this.balancesTotalService.loadCurrentTotalBalance();
  }
}