import { Component, OnInit } from '@angular/core';

import { AssetService } from '../assets/shared/asset.service'

@Component({
  selector: 'app-totals',
  templateUrl: './totals.component.html',
  styleUrls: ['./totals.component.css']
})
export class TotalsComponent implements OnInit {

  constructor(public assetService : AssetService) { }

  ngOnInit() {
    this.assetService.getTotalBalance();
  }

}
