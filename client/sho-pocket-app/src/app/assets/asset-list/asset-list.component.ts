import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';   

import { AssetService } from '../shared/asset.service';
import { Asset } from '../shared/asset.model'

@Component({
  selector: 'app-asset-list',
  templateUrl: './asset-list.component.html',
  styleUrls: ['./asset-list.component.css']
})
export class AssetListComponent implements OnInit {

  constructor(public assetService : AssetService, private toastr : ToastrService) { }

  ngOnInit() {
    this.assetService.getAssetList();
  }

  showForEdit(asset: Asset) {
    this.assetService.selectedAsset = Object.assign({}, asset);;
  }

  onDelete(id: string) {
    if (confirm('Are you sure to delete this record ?') == true) {
      this.assetService.deleteAsset(id)
      .subscribe(x => {
        this.assetService.getAssetList();
        this.toastr.warning("Deleted Successfully","Asset");
      })
    }
  }
}
