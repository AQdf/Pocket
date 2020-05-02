import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms'
import { ToastrService } from 'ngx-toastr';

import { AssetService } from '../../services/asset.service';
import { CurrencyService } from '../../services/currency.service';
import { Asset } from '../../models/asset.model'

@Component({
  selector: 'app-assets',
  templateUrl: './assets.component.html',
  styleUrls: ['./assets.component.css']
})
export class AssetsComponent implements OnInit {

  assetList: Asset[];
  selectedAsset: Asset;
  currenciesList: string[];
  isAddMode: boolean;

  constructor(
    private assetService : AssetService,
    private currencyService: CurrencyService,
    private toastr : ToastrService) { }

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
    this.selectedAsset = {
      id: asset.id,
      name: asset.name,
      currency: asset.currency,
      isActive: asset.isActive,
      value: asset.value,
      updatedOn: asset.updatedOn
    }
  }

  onDelete(id: string) {
    if (id === null) {
      this.assetList.shift();
      this.isAddMode = false;
      return;
    }

    if (confirm('Are you sure to delete this record?') == true) {
      this.assetService.deleteAsset(id).subscribe(result => {
        if (result) {
          var listIndex = this.assetList.findIndex(a => a.id === id);
          this.assetList.splice(listIndex, 1);
          this.toastr.success("Deleted Successfully","Asset");
        } else {
          this.toastr.error("Delete failed! Possibly, cannot delete asset because balance for asset exists","Asset");
        }
      })
    }
  }

  onSubmit(form: NgForm) {
    if (form.value.id == null) {
      this.assetService.postAsset(form.value).subscribe((result: Asset) => {
        if (result) {
          var listIndex = this.assetList.findIndex(a => a.id === null);
          this.assetList[listIndex] = result;
          this.selectedAsset = null;
          this.isAddMode = false;
          this.toastr.success('Added Succcessfully!', 'Asset');
        } else {
          this.toastr.error('Create failed!', 'Asset');
        }
      });
    }
    else {
      if (!this.assetHasChanged(form.value)) {
        this.selectedAsset = null;
        this.isAddMode = false;
      } else {
        this.assetService.putAsset(form.value.id, form.value).subscribe((result: Asset) => {
          if (result) {
            var listIndex = this.assetList.findIndex(a => a.id === result.id);
            this.assetList[listIndex] = result;
            this.selectedAsset = null;
            this.isAddMode = false;
            this.toastr.success('Updated Successfully!', 'Asset');
          } else {
            this.toastr.error('Update failed!', 'Asset');
          }
        });
      }
    }
  }

  assetHasChanged(changes: Asset) {
    return !(changes.name === this.selectedAsset.name
      && changes.value === this.selectedAsset.value
      && changes.currency === this.selectedAsset.currency 
      && changes.isActive === this.selectedAsset.isActive);
  }

  addAsset() {
    let newAsset: Asset = {
      id: null,
      name: '',
      currency: '',
      isActive: true,
      value: 0.00,
      updatedOn: ''
    }

    this.assetList.unshift(newAsset);
    this.selectedAsset = newAsset;
    this.isAddMode = true;
  }

  getCurrenciesList() {
    this.currencyService.getCurrenciesList().subscribe((response: string[]) => {
      this.currenciesList = response;
    });
  }
}
