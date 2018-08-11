import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms'
import { ToastrService } from 'ngx-toastr';   

import { AssetHistoryService } from '../shared/asset-history.service';
import { getLocaleDateTimeFormat } from '../../../../node_modules/@angular/common';

@Component({
  selector: 'app-asset-history',
  templateUrl: './asset-history.component.html',
  styleUrls: ['./asset-history.component.css']
})
export class AssetHistoryComponent implements OnInit {

  constructor(public assetHistoryService : AssetHistoryService, private toastr : ToastrService) { }

  ngOnInit() {
    this.resetForm();
  }

  resetForm(form?: NgForm) {
    if (form != null)
      form.reset();
    
    this.assetHistoryService.selectedAssetHistory = {
      id: null,
      assetName: '',
      effectiveDate: null,
      balance: 0.0
    }
  }
 
  onSubmit(form: NgForm) {
    if (form.value.id == null) {
      this.assetHistoryService.postAssetHistory(form.value)
        .subscribe(data => {
          this.resetForm(form);
          this.assetHistoryService.getAssetHistoryList();
          this.toastr.success('New Record Added Succcessfully', 'Asset History');
        })
    }
    else {
      this.assetHistoryService.putAssetHistory(form.value.id, form.value)
      .subscribe(data => {
        this.resetForm(form);
        this.assetHistoryService.getAssetHistoryList();
        this.toastr.info('Record Updated Successfully!', 'Asset History');
      });
    }
  }
}
