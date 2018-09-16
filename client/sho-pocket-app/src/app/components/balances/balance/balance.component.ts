import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms'
import { ToastrService } from 'ngx-toastr';   

import { BalanceService } from '../../../services/balance.service';
import { AssetService } from '../../../services/asset.service';

@Component({
  selector: 'app-balance',
  templateUrl: './balance.component.html',
  styleUrls: ['./balance.component.css']
})
export class BalanceComponent implements OnInit {

  constructor(public balanceService : BalanceService, public assetService : AssetService, private toastr : ToastrService) { 
  }

  ngOnInit() {
    this.assetService.getAssetList();
    this.resetForm();
  }

  resetForm(form?: NgForm) {
    if (form != null)
      form.reset();

      var now = new Date().toISOString();
      var formattedNow = now.substring(0, now.indexOf('T'));

      this.balanceService.selectedBalance = {
      id: null,
      effectiveDate: formattedNow,
      value: 0.0,
      assetId: '',
      exchangeRateId: '',
      exchangeRate: 0.0,
      asset: null
    }
  }
 
  onSubmit(form: NgForm) {
    let dateFilter = this.balanceService.effectiveDatesList.length > 0 ? this.balanceService.effectiveDatesList[0] : null;

    if (form.value.id == null) {
      this.balanceService.postBalance(form.value)
        .subscribe(() => {
          this.resetForm(form);
          this.balanceService.getBalanceList(dateFilter);
          this.toastr.success('New Record Added Succcessfully', 'Balance');
        });
    }
    else {
      this.balanceService.putBalance(form.value.id, form.value)
      .subscribe(() => {
        this.resetForm(form);
        this.balanceService.getBalanceList(dateFilter);
        this.toastr.info('Record Updated Successfully!', 'Balance');
      });
    }
  }

}