import { Component, OnInit, ChangeDetectorRef  } from '@angular/core';
import { NgForm } from '@angular/forms'
import { ToastrService } from 'ngx-toastr';   

import { AssetService } from '../../../services/asset.service';

@Component({
  selector: 'app-asset',
  templateUrl: './asset.component.html',
  styleUrls: ['./asset.component.css']
})
export class AssetComponent implements OnInit {

  constructor(
    public assetService : AssetService,
    private toastr : ToastrService,
    protected changeDetectorRef: ChangeDetectorRef) { }

  ngOnInit() {
    this.assetService.getCurrenciesList();
    this.resetForm();
  }

  resetForm(form?: NgForm) {
    if (form != null) {
      form.reset();
      this.changeDetectorRef.detectChanges();
    }

    this.assetService.selectedAsset = {
      id: null,
      name: '',
      currencyId: '',
      currencyName: '',
      isActive: true,
    }
  }
 
  onSubmit(form: NgForm) {
    if (form.value.id == null) {
      this.assetService.postAsset(form.value)
        .subscribe(asset => {
          this.assetService.assetList.unshift(asset);
          this.toastr.success('New Record Added Succcessfully', 'Asset');
          this.resetForm(form);
        });
    }
    else {
      this.assetService.putAsset(form.value.id, form.value)
      .subscribe(asset => {
        if(asset) {
          var listIndex = this.assetService.assetList.findIndex(a => a.id === asset.id);
          this.assetService.assetList[listIndex] = asset;
          this.toastr.success('Updated Successfully!', 'Asset');
          this.resetForm(form);
        } else {
          this.toastr.error('Update failed! Possibly, cannot update asset currency because balance for asset exists', 'Asset');
        }
      });
    }
  }
  
}