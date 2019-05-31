import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

import { BalanceService } from '../../services/balance.service';
import { ExchangeRateService } from '../../services/exchange-rate.service';

import { ExchangeRate } from '../../models/exchange-rate.model';

@Component({
  selector: 'app-balances',
  templateUrl: './balances.component.html',
  styleUrls: ['./balances.component.css']
})
export class BalancesComponent implements OnInit {

  constructor(public balanceService: BalanceService, private exchangeRateService: ExchangeRateService, private toastr: ToastrService) { }

  ngOnInit() {
  }

  onDateChange(value) {
    this.balanceService.getBalanceList(value);
  }

  addBalances() {
    this.balanceService.addBalancesByTemplate().subscribe(success => {
      if (success) {
        this.balanceService.reload();
        this.toastr.success('Current date balances created by template', 'Balance');
      }
    });
  }

  applyExchangeRate(model: ExchangeRate) {
    this.exchangeRateService.applyExchangeRate(model).subscribe(success => {
      if (success) {
        this.balanceService.reload();
        this.toastr.success('Exchange rate applied', 'Balance');
      }
    });
  }
  
  downloadCsv() {
    this.balanceService.downloadCsv();
  }
}
