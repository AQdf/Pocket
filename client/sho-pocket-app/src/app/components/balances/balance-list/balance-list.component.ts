import { Component, OnInit, Input, EventEmitter, OnChanges, SimpleChange, Output } from '@angular/core';
import { NgForm } from '@angular/forms'
import { ToastrService } from 'ngx-toastr';

import { AssetService } from '../../../services/asset.service';
import { BalanceService } from '../../../services/balance.service';
import { Balance } from '../../../models/balance.model'
import { Asset } from '../../../models/asset.model';
import { BalancesTotalService } from '../../../services/balances-total.service';
import { BalanceTotal } from '../../../models/balance-total.model';
import { Balances } from '../../../models/balances.model';

@Component({
  selector: 'app-balance-list',
  templateUrl: './balance-list.component.html',
  styleUrls: ['./balance-list.component.css']
})
export class BalanceListComponent implements OnInit, OnChanges {

  constructor(
    private balanceService : BalanceService,
    private assetService : AssetService,
    private balanceTotalService : BalancesTotalService,
    private toastr : ToastrService) { }

  ngOnInit() {
    this.initAssets();
  }

  @Input() effectiveDate: string;
  @Output() shouldReload = new EventEmitter<boolean>();

  selectedBalance: Balance;
  balances: Balance[];
  totalBalance: BalanceTotal[];
  assetList: Asset[];
  isAddMode: boolean;

  reloadEffectiveDates(shouldReload: boolean) {
    this.shouldReload.emit(shouldReload);
  }

  ngOnChanges(changes: {[propKey: string]: SimpleChange}) {
    this.effectiveDate = changes.effectiveDate.currentValue;
    if (this.effectiveDate) {
      this.reloadBalances();
    }
  }

  reloadBalances() {
    this.balanceService.getBalanceList(this.effectiveDate).subscribe((balances: Balances) => {
      this.balances = balances.items;
      this.totalBalance = balances.totalBalance;
      if (this.balances.length === 0) {
        this.reloadEffectiveDates(true);
      }
    });
  }

  initAssets() {
    this.assetService.getAssetList().subscribe((assets: Asset[]) => {
      this.assetList = assets;
    });
  }

  showForEdit(balance: Balance) {
    if (this.selectedBalance && this.selectedBalance.id === balance.id) {
      this.selectedBalance = null;
    } else {
      this.selectedBalance = balance;
    }
  }

  resetRecord(id: string) {
    this.balanceService.getBalance(id).subscribe((result: Balance) => {
      let index = this.balances.findIndex(f => f.id === id)
      this.balances[index] = result;
    })
  }

  onDelete(id: string) {
    if (id === null) {
      this.balances.shift();
      this.isAddMode = false;
      return;
    }

    if (confirm('Are you sure to delete this record ?') == true) {
      this.balanceService.deleteBalance(id).subscribe(x => {
        this.reloadBalances();
        this.balanceTotalService.loadCurrentTotalBalance();
        this.toastr.success("Record Deleted.", "Balance");
      })
    }
  }

  removeItemFromBalancesListById(id: string) {
    let index = this.balances.findIndex(f => f.id === id)
    if (index > -1) {
      this.balances.splice(index, 1);
    }
  }

  onSubmit(form: NgForm) {
    if (form.value.id === null) {
      this.balanceService.postBalance(this.selectedBalance).subscribe(() => {
          this.afterSubmit();
          this.toastr.success('New Record Added.', 'Balance');
        });
    }
    else {
      this.balanceService.putBalance(form.value.id, this.selectedBalance).subscribe(() => {
        this.afterSubmit();
        this.toastr.info('Record Updated.', 'Balance');
      });
    }
  }

  afterSubmit() {
    this.reloadBalances();
    this.balanceTotalService.loadCurrentTotalBalance();
    this.selectedBalance = null;
    this.isAddMode = false;
  }

  addBalance() {
    var balanceDate = this.effectiveDate;
    var formattedDate = balanceDate.substring(0, balanceDate.indexOf('T'));

    let newBalance: Balance =  {
      id: null,
      effectiveDate: formattedDate,
      value: 0.0,
      assetId: '',
      exchangeRateId: '',
      asset: null,
      isBankAccount: false
    }

    this.balances.unshift(newBalance);
    this.selectedBalance = newBalance;
    this.isAddMode = true;
  }

  onAssetChanged(assetId: string) {
    let asset = this.assetList.find(a => a.id == assetId);
    this.selectedBalance.assetId = asset.id;
    this.selectedBalance.asset = asset;
  }

  onBankAccountSync(balanceId: string) {
    this.balanceService.syncBankAccountBalance(balanceId).subscribe(() => {
      this.afterSubmit();
      this.toastr.info('Record Synced Successfully!', 'Balance');
    });
  }
}
