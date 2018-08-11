import { Component, OnInit } from '@angular/core';
import { AssetHistoryService } from './shared/asset-history.service';

@Component({
  selector: 'app-assets-history',
  templateUrl: './assets-history.component.html',
  styleUrls: ['./assets-history.component.css']
})
export class AssetsHistoryComponent implements OnInit {

  constructor(public assetService : AssetHistoryService) { }

  ngOnInit() {
  }

}
