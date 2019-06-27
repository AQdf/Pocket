import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { NgForm } from '@angular/forms'
import { ToastrService } from 'ngx-toastr';   

import { BalanceService } from '../../../services/balance.service';
import { AssetService } from '../../../services/asset.service';
import { Balance } from 'src/app/models/balance.model';

@Component({
  selector: 'app-balance',
  templateUrl: './balance.component.html',
  styleUrls: ['./balance.component.css']
})
export class BalanceComponent implements OnInit {

  constructor(
    public balanceService : BalanceService,
    public assetService : AssetService,
    private toastr : ToastrService,
    protected changeDetectorRef: ChangeDetectorRef) { 
  }

  selectedBalance: Balance;

  ngOnInit() {
    this.resetForm();
  }

  resetForm(form?: NgForm) {
    if (form != null) {
      form.reset();
      this.changeDetectorRef.detectChanges();
    }

      var tzOffset = (new Date()).getTimezoneOffset() * 60000; //offset in milliseconds
      var localISOTime = (new Date(Date.now() - tzOffset)).toISOString().slice(0, -1);
      var formattedNow = localISOTime.substring(0, localISOTime.indexOf('T'));

      this.selectedBalance = {
      id: null,
      effectiveDate: formattedNow,
      value: 0.0,
      assetId: '',
      exchangeRateId: '',
      exchangeRateValue: 0.0,
      defaultCurrencyValue: 0.0,
      asset: null
    }
  }
 
  onSubmit(form: NgForm) {
    if (form.value.id == null) {
      this.balanceService.postBalance(form.value)
        .subscribe(() => {
          this.resetForm(form);
          this.balanceService.loadEffectiveDatesList();
          this.toastr.success('New Record Added Succcessfully', 'Balance');
        });
    }
    else {
      this.balanceService.putBalance(form.value.id, form.value)
      .subscribe(() => {
        this.resetForm(form);
        this.balanceService.loadEffectiveDatesList();
        this.toastr.info('Record Updated Successfully!', 'Balance');
      });
    }
  }
}
