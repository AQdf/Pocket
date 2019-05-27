import { Component, OnInit } from '@angular/core';

import { InventoryService } from '../../services/inventory.service';

@Component({
  selector: 'app-inventory',
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.css']
})
export class InventoryComponent implements OnInit {

  constructor(public inventoryService : InventoryService) { }

  ngOnInit() {
  }

}
