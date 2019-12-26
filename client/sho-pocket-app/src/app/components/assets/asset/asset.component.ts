import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

import { AssetService } from '../../../services/asset.service';
import { BankSyncService } from '../../../services/bank-sync.service';
import { Asset } from '../../../models/asset.model';
import { ActivatedRoute } from '@angular/router';
import { BankAccount } from '../../../models/bank-account';
import { AssetBankAccount } from '../../../models/asset-bank-account';

@Component({
  selector: 'app-asset',
  templateUrl: './asset.component.html',
  styleUrls: ['./asset.component.css']
})
export class AssetComponent implements OnInit {

  constructor(
    private assetService : AssetService,
    private bankSyncService : BankSyncService,
    private activeRouter : ActivatedRoute,
    private toastr : ToastrService) { }

    id: string = null;
    asset: Asset;
    banksList: string[];
    selectedBank: string;
    token: string;
    bankClientId: string;
    cardNumber: string;
    authDataSubmitted: boolean = false;
    isConnected: boolean = false;
    bankAccountsList: BankAccount[];
    selectedAccount: BankAccount;

  ngOnInit() {
    this.id = this.activeRouter.snapshot.params['id'];
    this.initBanksLookup();
    this.initAsset();
    this.initAssetBankAccount();
  }

  initAsset() {
    this.assetService.getAsset(this.id).subscribe((result: Asset) => {
      this.asset = result;
    })
  }

  initBanksLookup() {
    this.bankSyncService.getBanksList().subscribe((response: string[]) => {
      this.banksList = response;
    });
  }

  initAssetBankAccount() {
    this.bankSyncService.getAssetBankSyncData(this.id).subscribe((account: AssetBankAccount) => {
      if (account) {
        this.authDataSubmitted = true;
        this.selectedBank = account.bankName;
        this.token = account.tokenMask;
        this.selectedAccount = new BankAccount();
        this.selectedAccount.name = account.bankAccountName;
        this.bankAccountsList = [
          this.selectedAccount
        ];
        this.isConnected = true;
      }
    });
  }

  onBankFormSubmit(form: any) {
    this.bankSyncService.submitBankClientAuthData(this.selectedBank, this.token, this.bankClientId, this.cardNumber).subscribe((response: BankAccount[]) => {
      console.log(response[0].name);
      this.authDataSubmitted = true;
      this.bankAccountsList = response;
    });
  }

  onAccountFormSubmit(form: any) {
    this.bankSyncService.connectAccount(this.id, this.selectedBank, this.selectedAccount).subscribe(success => {
      if (success) {
        this.isConnected = true;
        this.toastr.success('Asset was successfully connected to bank account.', 'Bank sync.');
      }
    });
  }

  disconnectAccount() {
    this.bankSyncService.disconnectAccount(this.id).subscribe(success => {
      if (success) {
        this.selectedBank = null;
        this.token = null;
        this.bankClientId = null;
        this.cardNumber = null;
        this.authDataSubmitted = false;
        this.isConnected = false
        this.bankAccountsList = null;
        this.selectedAccount = null;
        this.toastr.success('Asset was disconnected from bank account.', 'Bank sync.');
      }
    });
  }
}