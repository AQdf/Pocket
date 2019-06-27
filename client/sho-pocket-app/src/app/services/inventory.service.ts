import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment'
import { InventoryItem } from'../models/inventory-item.model'
import { BaseService } from './base.service';

const inventoryApiUrl = environment.baseApiUrl + 'inventory/';

@Injectable({
  providedIn: 'root'
})
export class InventoryService extends BaseService {

  constructor(private http: HttpClient) {
    super();
  }

  getInventoryItems() {
    return this.http.get(inventoryApiUrl, this.getDefaultOptions());
  }

  getInventoryItem(id: string) {
    return this.http.get(inventoryApiUrl + id, this.getDefaultOptions());
  }

  postInventoryItem(item: InventoryItem) {
    let body = JSON.stringify(item);
    return this.http.post(inventoryApiUrl, body, this.getDefaultOptions());
  }
 
  putInventoryItem(id: string, item: InventoryItem) {
    let body = JSON.stringify(item);
    return this.http.put(inventoryApiUrl + id, body, this.getDefaultOptions());
  }

  deleteInventoryItem(id: string) {
    return this.http.delete(inventoryApiUrl + id, this.getDefaultOptions());
  }
}