import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms'
import { ToastrService } from 'ngx-toastr';   

import { BalanceService } from '../../../services/balance.service';

@Component({
  selector: 'app-balance',
  templateUrl: './balance.component.html',
  styleUrls: ['./balance.component.css']
})
export class BalanceComponent implements OnInit {

  constructor(public balanceService : BalanceService, private toastr : ToastrService) { }

  ngOnInit() {
    this.resetForm();
  }

  resetForm(form?: NgForm) {
    if (form != null)
      form.reset();
    this.balanceService.selectedBalance = {
      id: null,
      effectiveDate: new Date(),
      value: 0.0,
      assetId: '',
      assetCurrencyId: '',
      assetName: '',
      assetTypeId: '',
      exchangeRateId: '',
      exchangeRate: 0.0
    }
  }
 
  onSubmit(form: NgForm) {
    if (form.value.id == null) {
      this.balanceService.postBalance(form.value)
        .subscribe(data => {
          this.resetForm(form);
          this.balanceService.getBalanceList();
          this.toastr.success('New Record Added Succcessfully', 'Balance');
        });
    }
    else {
      this.balanceService.putBalance(form.value.id, form.value)
      .subscribe(data => {
        this.resetForm(form);
        this.balanceService.getBalanceList();
        this.toastr.info('Record Updated Successfully!', 'Balance');
      });
    }
  }

}
