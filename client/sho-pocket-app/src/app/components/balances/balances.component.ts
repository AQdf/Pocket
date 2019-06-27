import { Component, OnInit } from '@angular/core';

import { BalanceService } from '../../services/balance.service';

@Component({
  selector: 'app-balances',
  templateUrl: './balances.component.html',
  styleUrls: ['./balances.component.css']
})
export class BalancesComponent implements OnInit {

  constructor(public balanceService: BalanceService) { }

  selectedEffectiveDate: string;
  effectiveDatesList: string[];

  ngOnInit() {
    this.loadEffectiveDates();
  }

  loadEffectiveDates() {
    this.balanceService.loadEffectiveDatesList().subscribe((result: string[]) => {
      if(result.length > 0) {
        this.effectiveDatesList = result;
        this.selectedEffectiveDate = this.effectiveDatesList[0];
      } else {
        this.effectiveDatesList = null;
        this.selectedEffectiveDate = null;
      }
    });
  }
  
  onDateChange(effectiveDate: string) {
    this.selectedEffectiveDate = effectiveDate;
  }

  reload(event: any) {
    this.loadEffectiveDates();
  }
  
  downloadCsv() {
    this.balanceService.downloadCsv(this.selectedEffectiveDate);
  }
}
