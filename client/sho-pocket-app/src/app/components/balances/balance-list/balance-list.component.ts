import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms'
import { ToastrService } from 'ngx-toastr';   

import { from } from 'rxjs';
import { first } from 'rxjs/operators';

import { AssetService } from '../../../services/asset.service';
import { BalanceService } from '../../../services/balance.service';
import { Balance } from '../../../models/balance.model'

@Component({
  selector: 'app-balance-list',
  templateUrl: './balance-list.component.html',
  styleUrls: ['./balance-list.component.css']
})
export class BalanceListComponent implements OnInit {
  currentEditRecordId: string;
  isAddMode: boolean;

  constructor(
    public balanceService : BalanceService,
    public assetService : AssetService,
    private toastr : ToastrService) { }

  ngOnInit() {
    this.assetService.getAssetList();
  }

  showForEdit(balance: Balance) {
    if (this.currentEditRecordId === balance.id) {
      this.currentEditRecordId = null;
    } else {
      this.currentEditRecordId = balance.id;
    }
  }

  resetRecord(id: string) {
    this.balanceService.getBalance(id).subscribe(result => {
      let index = this.balanceService.balances.findIndex(f => f.id === id)
      this.balanceService.balances[index] = result;
    })
  }

  onDelete(id: string) {
    if (id === null)
    {
      this.balanceService.balances.shift();
      this.isAddMode = false;
      return;
    }

    if (confirm('Are you sure to delete this record ?') == true) {
      this.balanceService.deleteBalance(id)
      .subscribe(x => {
        this.balanceService.reload();
        this.toastr.warning("Deleted Successfully", "Balance");
      })
    }
  }

  onSubmit(form: NgForm) {
    if (form.value.id === null) {
      this.balanceService.postBalance(this.balanceService.selectedBalance)
        .subscribe(() => {
          this.balanceService.reload();
          this.toastr.success('New Record Added Succcessfully', 'Balance');
          this.currentEditRecordId = null;
        });
    }
    else {
      this.balanceService.putBalance(form.value.id, form.value)
      .subscribe(() => {
        this.balanceService.reload();
        this.toastr.info('Record Updated Successfully!', 'Balance');
        this.currentEditRecordId = null;
        this.isAddMode = false;
      });
    }
  }

  addBalance() {
    var tzOffset = (new Date().getTimezoneOffset()) * 60000; //offset in milliseconds
    var localISOTime = (new Date(Date.now() - tzOffset)).toISOString().slice(0, -1);
    var formattedNow = localISOTime.substring(0, localISOTime.indexOf('T'));

    let newBalance =  {
      id: null,
      effectiveDate: formattedNow,
      value: 0.0,
      assetId: '',
      exchangeRateId: '',
      exchangeRateValue: 0.0,
      defaultCurrencyValue: 0.0,
      asset: null
    }

    this.balanceService.balances.unshift(newBalance);
    this.balanceService.selectedBalance = newBalance;
    this.currentEditRecordId = null;
    this.isAddMode = true;
  }

  onAssetAdded(value) {
    let asset = this.assetService.assetList.find(a => a.id == value);
    let exchangeRate = this.balanceService.exchangeRates.find(rate => rate.baseCurrencyName == asset.currencyName);
    this.balanceService.selectedBalance.assetId = asset.id;
    this.balanceService.selectedBalance.exchangeRateId = exchangeRate.id;
    this.balanceService.selectedBalance.asset = asset;
  }
}
