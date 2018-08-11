import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';   

import { AssetHistoryService } from '../shared/asset-history.service';
import { AssetHistory } from '../shared/asset-history.model'

@Component({
  selector: 'app-asset-history-list',
  templateUrl: './asset-history-list.component.html',
  styleUrls: ['./asset-history-list.component.css']
})
export class AssetHistoryListComponent implements OnInit {

  constructor(public assetHistoryService : AssetHistoryService, private toastr : ToastrService) { }

  ngOnInit() {
    this.assetHistoryService.getAssetHistoryList();
  }

  showForEdit(assetHistory: AssetHistory) {
    this.assetHistoryService.selectedAssetHistory = Object.assign({}, assetHistory);;
  }

  onDelete(id: string) {
    if (confirm('Are you sure to delete this record ?') == true) {
      this.assetHistoryService.deleteAssetHistory(id)
      .subscribe(x => {
        this.assetHistoryService.getAssetHistoryList();
        this.toastr.warning("Deleted Successfully","Asset History");
      })
    }
  }
}
