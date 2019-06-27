import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms'
import { ToastrService } from 'ngx-toastr';

import { InventoryService } from '../../../services/inventory.service';
import { InventoryItem } from '../../../models/inventory-item.model'

@Component({
  selector: 'app-inventory-items',
  templateUrl: './inventory-items.component.html',
  styleUrls: ['./inventory-items.component.css']
})
export class InventoryItemsComponent implements OnInit {

  selectedItem: InventoryItem;
  items: InventoryItem[];
  categories: string[];
  currentEditRecordId: string;
  isAddMode: boolean;

  constructor(
    public inventoryService : InventoryService,
    private toastr : ToastrService) { }

  ngOnInit() {
    this.loadInventoryItems();
  }

  loadInventoryItems() {
    this.inventoryService.getInventoryItems().subscribe((result: InventoryItem[]) => {
      this.items = result;
    });
  }

  showForEdit(item: InventoryItem) {
    if (this.currentEditRecordId === item.id) {
      this.currentEditRecordId = null;
    } else {
      this.currentEditRecordId = item.id;
    }
  }

  resetRecord(id: string) {
    this.inventoryService.getInventoryItem(id).subscribe((result: InventoryItem) => {
      let index = this.items.findIndex(f => f.id === id)
      this.items[index] = result;
    })
  }

  onDelete(id: string) {
    if (id === null) {
      this.items.shift();
      this.isAddMode = false;
      return;
    }

    if (confirm('Are you sure to delete this record ?') == true) {
      this.inventoryService.deleteInventoryItem(id)
      .subscribe(x => {
        this.loadInventoryItems();
        this.toastr.success("Deleted Successfully", "Inventory");
      })
    }
  }

  onSubmit(form: NgForm) {
    if (form.value.id === null) {
      this.inventoryService.postInventoryItem(this.selectedItem).subscribe(() => {
        this.loadInventoryItems();
        this.toastr.success('New Record Added Succcessfully', 'Inventory');
        this.currentEditRecordId = null;
        this.isAddMode = false;
      });
    } else {
      this.inventoryService.putInventoryItem(form.value.id, form.value).subscribe(() => {
        this.loadInventoryItems();
        this.toastr.info('Record Updated Successfully!', 'Balance');
        this.currentEditRecordId = null;
        this.isAddMode = false;
      });
    }
  }

  addItem() {
    let newItem: InventoryItem = {
      id: null,
      name: '',
      description: '',
      category: ''
    };

    this.items.unshift(newItem);
    this.selectedItem = newItem;
    this.currentEditRecordId = null;
    this.isAddMode = true;
  }
}
