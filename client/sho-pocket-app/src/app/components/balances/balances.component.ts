import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

import { BalanceService } from '../../services/balance.service';
import { BalancesTotalService } from '../../services/balances-total.service';
import { EffectiveDateService } from '../../services/effective-date.service';

import { ResponseError } from '../../models/response-error.model';
import { HttpEventType } from '@angular/common/http';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-balances',
  templateUrl: './balances.component.html',
  styleUrls: ['./balances.component.css']
})
export class BalancesComponent implements OnInit {

  subscription: Subscription;
  selectedEffectiveDate: string;

  constructor(
    private balanceService: BalanceService,
    private balanceTotalService: BalancesTotalService,
    private effectiveDateService: EffectiveDateService,
    private toastr: ToastrService) {
      this.subscription = effectiveDateService.effectiveDateSelected$.subscribe(
        effectiveDate => { 
          this.selectedEffectiveDate = effectiveDate;
        });
    }

  ngOnInit() {
  }

  onDateChange(effectiveDate: string) {
    this.selectedEffectiveDate = effectiveDate;
  }

  addBalances() {
    this.balanceService.addBalancesByTemplate().subscribe(success => {
      if (success) {
        this.toastr.success('Current date balances created by template', 'Balance');
        this.reload();
      }
    }, (errors: ResponseError[]) => {
      for (let error of errors) {
        this.toastr.error(error.description, 'Balances')
      }
    });
  }

  reload() {
    this.balanceTotalService.loadCurrentTotalBalance();
    this.effectiveDateService.notifyReload();
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
      }
    });
  }
}
