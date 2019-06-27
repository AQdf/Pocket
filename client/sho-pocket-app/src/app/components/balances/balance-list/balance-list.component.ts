import { Component, OnInit, Input, EventEmitter, OnChanges, SimpleChange, Output } from '@angular/core';
import { NgForm } from '@angular/forms'
import { ToastrService } from 'ngx-toastr';

import { AssetService } from '../../../services/asset.service';
import { BalanceService } from '../../../services/balance.service';
import { Balance } from '../../../models/balance.model'
import { ResponseError } from 'src/app/models/response-error.model';
import { Asset } from 'src/app/models/asset.model';
import { BalancesTotalService } from 'src/app/services/balances-total.service';
import { BalanceTotal } from 'src/app/models/balance-total.model';
import { ExchangeRate } from 'src/app/models/exchange-rate.model';
import { Balances } from 'src/app/models/balances.model';
import { ExchangeRateService } from 'src/app/services/exchange-rate.service';

@Component({
  selector: 'app-balance-list',
  templateUrl: './balance-list.component.html',
  styleUrls: ['./balance-list.component.css']
})
export class BalanceListComponent implements OnInit, OnChanges {

  constructor(
    public balanceService : BalanceService,
    public assetService : AssetService,
    private balanceTotalService : BalancesTotalService,
    private exchangeRateService: ExchangeRateService,
    private toastr : ToastrService) { }

  @Input() effectiveDate: string;
  @Output() shouldReload = new EventEmitter<boolean>();
 
  reloadEffectiveDates(shouldReload: boolean) {
    this.shouldReload.emit(shouldReload);
  }
  
  selectedBalance: Balance;
  balances: Balance[];
  totalBalance: BalanceTotal[];
  exchangeRates: ExchangeRate[];
  assetList: Asset[];
  currentEditRecordId: string;
  isAddMode: boolean;

  ngOnInit() {
    this.initAssets();
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
      this.exchangeRates = balances.exchangeRates;
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
  
  addBalances() {
    this.balanceService.addBalancesByTemplate().subscribe(success => {
      if (success) {
        this.reloadEffectiveDates(true);
        this.balanceTotalService.loadCurrentTotalBalance();
        this.toastr.success('Current date balances created by template', 'Balance');
      }
    }, (errors: ResponseError[]) => {
      for (let error of errors) {
        this.toastr.error(error.description, 'Balances')
      }
    });
  }

  showForEdit(balance: Balance) {
    if (this.currentEditRecordId === balance.id) {
      this.currentEditRecordId = null;
    } else {
      this.currentEditRecordId = balance.id;
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
        this.toastr.success("Deleted Successfully", "Balance");
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
      this.balanceService.postBalance(this.selectedBalance)
        .subscribe(() => {
          this.reloadBalances();
          this.balanceTotalService.loadCurrentTotalBalance();
          this.toastr.success('New Record Added Succcessfully', 'Balance');
          this.currentEditRecordId = null;
          this.isAddMode = false;
        });
    }
    else {
      this.balanceService.putBalance(form.value.id, form.value)
      .subscribe(() => {
        this.reloadBalances();
        this.balanceTotalService.loadCurrentTotalBalance();
        this.toastr.info('Record Updated Successfully!', 'Balance');
        this.currentEditRecordId = null;
        this.isAddMode = false;
      });
    }
  }

  addBalance() {
    var balanceDate = this.effectiveDate;
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

    this.balances.unshift(newBalance);
    this.selectedBalance = newBalance;
    this.currentEditRecordId = null;
    this.isAddMode = true;
  }

  onAssetAdded(value) {
    let asset = this.assetList.find(a => a.id == value);
    let exchangeRate = this.exchangeRates.find(rate => rate.baseCurrency == asset.currency);
    this.selectedBalance.assetId = asset.id;
    this.selectedBalance.exchangeRateId = exchangeRate.id;
    this.selectedBalance.asset = asset;
  }

  applyExchangeRate(model: ExchangeRate) {
    this.exchangeRateService.applyExchangeRate(model).subscribe(success => {
      if (success) {
        this.reloadBalances();
        this.balanceTotalService.loadCurrentTotalBalance();
        this.toastr.success('Exchange rate applied', 'Balance');
      }
    });
  }
}
