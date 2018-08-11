import { Component, OnInit } from '@angular/core';

import { AssetService } from '../assets/shared/asset.service'

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  constructor(public assetService : AssetService) { }

  ngOnInit() {
    this.assetService.getAssetList();
  }
  
}
