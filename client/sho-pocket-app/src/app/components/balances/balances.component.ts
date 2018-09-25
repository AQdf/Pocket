import { Component, OnInit } from '@angular/core';
import { BalanceService } from '../../services/balance.service';

@Component({
  selector: 'app-balances',
  templateUrl: './balances.component.html',
  styleUrls: ['./balances.component.css']
})
export class BalancesComponent implements OnInit {
  selectedEffectiveDate: string;

  constructor(public balanceService : BalanceService) { }

  ngOnInit() {
  }

  onDateChange(value) {
    this.balanceService.getBalanceList(value);
  }
}
