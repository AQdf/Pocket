import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

import { BalanceService } from '../../services/balance.service';
import { BalancesTotalService } from '../../services/balances-total.service';
import { ResponseError } from '../../models/response-error.model';
import { HttpEventType } from '@angular/common/http';

@Component({
  selector: 'app-balances',
  templateUrl: './balances.component.html',
  styleUrls: ['./balances.component.css']
})
export class BalancesComponent implements OnInit {

  constructor(
    private balanceService: BalanceService,
    private balanceTotalService: BalancesTotalService,
    private toastr: ToastrService) { }

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

  reload() {
    this.loadEffectiveDates();
  }

  addBalances() {
    this.balanceService.addBalancesByTemplate().subscribe(success => {
      if (success) {
        this.loadEffectiveDates();
        this.balanceTotalService.loadCurrentTotalBalance();
        this.toastr.success('Current date balances created by template', 'Balance');
      }
    }, (errors: ResponseError[]) => {
      for (let error of errors) {
        this.toastr.error(error.description, 'Balances')
      }
    });
  }

  exportToCsv() {
    this.balanceService.exportAllCsv(this.selectedEffectiveDate);
  }

  exportToJson() {
    this.balanceService.exportJson(this.selectedEffectiveDate);
  }

  importJson(files: File[]) {
    this.balanceService.importJson(files).subscribe(event => {
      if (event.type === HttpEventType.UploadProgress) {
        //this.progress = Math.round(100 * event.loaded / event.total);
      }
      else if (event.type === HttpEventType.Response) {
        this.toastr.success('Balances successfully imported.', 'Balance');
        this.reload();
      }
    });
  }
}
