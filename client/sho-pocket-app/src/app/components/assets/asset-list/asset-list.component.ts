import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms'
import { ToastrService } from 'ngx-toastr';

import { AssetService } from '../../../services/asset.service';
import { CurrencyService } from '../../../services/currency.service';
import { Asset } from '../../../models/asset.model'

@Component({
  selector: 'app-asset-list',
  templateUrl: './asset-list.component.html',
  styleUrls: ['./asset-list.component.css']
})
export class AssetListComponent implements OnInit {

  constructor(private assetService : AssetService, private currencyService: CurrencyService, private toastr : ToastrService) { }
  
  assetList: Asset[];
  selectedAsset: Asset;
  currentEditRecordId: string;
  currenciesList: string[];
  isAddMode: boolean;

  ngOnInit() {
    this.getCurrenciesList();
    this.loadAssetList();
  }

  loadAssetList() {
    this.assetService.getAssetList().subscribe((assets: Asset[]) => {
      this.assetList = assets;
    });
  }

  showForEdit(asset: Asset) {
    if (this.currentEditRecordId === asset.id) {
      this.currentEditRecordId = null;
    } else {
      this.currentEditRecordId = asset.id;
    }
  }

  onDelete(id: string) {
    if (id === null)
    {
      this.assetList.shift();
      this.isAddMode = false;
      return;
    }

    if (confirm('Are you sure to delete this record?') == true) {
      this.assetService.deleteAsset(id)
      .subscribe(result => {
        if (result) {
          this.loadAssetList();
          this.toastr.success("Deleted Successfully","Asset");
        } else {
          this.toastr.error("Delete failed! Possibly, cannot delete asset because balance for asset exists","Asset");
        }
      })
    }
  }

  onSubmit(form: NgForm) {
    if (form.value.id == null) {
      this.assetService.postAsset(this.selectedAsset)
        .subscribe(asset => {
          this.toastr.success('New Record Added Succcessfully', 'Asset');
          this.loadAssetList();
          this.currentEditRecordId = null;
          this.isAddMode = false;
        });
    }
    else {
      this.assetService.putAsset(form.value.id, form.value)
      .subscribe((asset: Asset) => {
        if(asset) {
          var listIndex = this.assetList.findIndex(a => a.id === asset.id);
          this.assetList[listIndex] = asset;
          this.toastr.success('Updated Successfully!', 'Asset');
          this.currentEditRecordId = null;
          this.isAddMode = false;
        } else {
          this.toastr.error('Update failed! Possibly, cannot update asset currency because balance for asset exists', 'Asset');
        }
      });
    }
  }

  resetRecord(id: string) {
    this.assetService.getAsset(id).subscribe((result: Asset) => {
      let index = this.assetList.findIndex(f => f.id === id)
      this.assetList[index] = result;
    })
  }

  addAsset() {
    let newAsset = {
      id: null,
      name: '',
      value: 0.0,
      currency: '',
      isActive: true
    }

    this.assetList.unshift(newAsset);
    this.selectedAsset = newAsset;
    this.currentEditRecordId = null;
    this.isAddMode = true;
  }

  onCurrencyAdded(value: string) {
    let currency = this.assetService.currenciesList.find(c => c == value);
    this.selectedAsset.currency = currency;
  }

  getCurrenciesList() {
    this.currencyService.getCurrenciesList().subscribe((response: string[]) => {
      this.currenciesList = response;
    });
  }
}
