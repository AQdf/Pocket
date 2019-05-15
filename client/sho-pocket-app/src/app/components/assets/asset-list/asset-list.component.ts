import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms'
import { ToastrService } from 'ngx-toastr';   

import { AssetService } from '../../../services/asset.service';
import { Asset } from '../../../models/asset.model'

@Component({
  selector: 'app-asset-list',
  templateUrl: './asset-list.component.html',
  styleUrls: ['./asset-list.component.css']
})
export class AssetListComponent implements OnInit {
  currentEditRecordId: string;
  isAddMode: boolean;

  constructor(public assetService : AssetService, private toastr : ToastrService) { }

  ngOnInit() {
    this.assetService.getCurrenciesList();
    this.assetService.getAssetList();
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
      this.assetService.assetList.shift();
      this.isAddMode = false;
      return;
    }

    if (confirm('Are you sure to delete this record?') == true) {
      this.assetService.deleteAsset(id)
      .subscribe(result => {
        if (result) {
          this.assetService.getAssetList();
          this.toastr.success("Deleted Successfully","Asset");
        } else {
          this.toastr.error("Delete failed! Possibly, cannot delete asset because balance for asset exists","Asset");
        }
      })
    }
  }

  onSubmit(form: NgForm) {
    if (form.value.id == null) {
      this.assetService.postAsset(this.assetService.selectedAsset)
        .subscribe(asset => {
          this.toastr.success('New Record Added Succcessfully', 'Asset');
          this.reload();
          this.currentEditRecordId = null;
          this.isAddMode = false;
        });
    }
    else {
      this.assetService.putAsset(form.value.id, form.value)
      .subscribe(asset => {
        if(asset) {
          var listIndex = this.assetService.assetList.findIndex(a => a.id === asset.id);
          this.assetService.assetList[listIndex] = asset;
          this.toastr.success('Updated Successfully!', 'Asset');
          this.currentEditRecordId = null;
          this.isAddMode = false;
        } else {
          this.toastr.error('Update failed! Possibly, cannot update asset currency because balance for asset exists', 'Asset');
        }
      });
    }
  }

  reload()
  {
    this.assetService.getAssetList();
  }

  resetRecord(id: string) {
    this.assetService.getAsset(id).subscribe(result => {
      let index = this.assetService.assetList.findIndex(f => f.id === id)
      this.assetService.assetList[index] = result;
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

    this.assetService.assetList.unshift(newAsset);
    this.assetService.selectedAsset = newAsset;
    this.currentEditRecordId = null;
    this.isAddMode = true;
  }

  onCurrencyAdded(value: string) {
    let currency = this.assetService.currenciesList.find(c => c == value);
    this.assetService.selectedAsset.currency = currency;
  }
}