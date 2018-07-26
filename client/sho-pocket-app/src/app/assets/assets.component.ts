import { Component, OnInit } from '@angular/core';
import { AssetService } from './shared/asset.service';

@Component({
  selector: 'app-assets',
  templateUrl: './assets.component.html',
  styleUrls: ['./assets.component.css']
})
export class AssetsComponent implements OnInit {

  constructor(public assetService : AssetService) { }

  ngOnInit() {
  }

}
