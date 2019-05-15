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
        this.balanceService.getEffectiveDatesList();
        this.toastr.success("Deleted Successfully", "Balance");
      })
    }
  }

  removeItemFromBalancesListById(id: string)
  {
    let index = this.balanceService.balances.findIndex(f => f.id === id)
    if (index > -1) {
      this.balanceService.balances.splice(index, 1);
    }
  }

  onSubmit(form: NgForm) {
    if (form.value.id === null) {
      this.balanceService.postBalance(this.balanceService.selectedBalance)
        .subscribe(() => {
          this.balanceService.reload();
          this.toastr.success('New Record Added Succcessfully', 'Balance');
          this.currentEditRecordId = null;
          this.isAddMode = false;
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
    var balanceDate = this.balanceService.selectedEffectiveDate;
    var formattedDate = balanceDate.substring(0, balanceDate.indexOf('T'));

    let newBalance =  {
      id: null,
      effectiveDate: formattedDate,
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
    let exchangeRate = this.balanceService.exchangeRates.find(rate => rate.baseCurrency == asset.currency);
    this.balanceService.selectedBalance.assetId = asset.id;
    this.balanceService.selectedBalance.exchangeRateId = exchangeRate.id;
    this.balanceService.selectedBalance.asset = asset;
  }
}
