import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestMethod } from '@angular/http';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

import { InventoryItem } from'../models/inventory-item.model'
import { environment } from '../../environments/environment'

const inventoryApiUrl = environment.baseApiUrl + 'inventory/';

@Injectable({
  providedIn: 'root'
})
export class InventoryService {
  selectedItem: InventoryItem;
  items: InventoryItem[];
  categories: string[];

  constructor(public http: Http, public client: HttpClient) {
    this.getInventoryItems();
  }

  getHeaders() {
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let authToken = localStorage.getItem('auth_token');
    headers.append('Authorization', `Bearer ${authToken}`);

    return headers;
  }

  getInventoryItems() {
    var headers = this.getHeaders();
    var requestOptions = new RequestOptions({ method: RequestMethod.Get, headers: headers });

    this.http.get(inventoryApiUrl, requestOptions).pipe(
      map(data => {
        return data.json() as InventoryItem[];
      })
    ).subscribe(items => {
        this.items = items;
    });
  }

  getInventoryItem(id: string) {
    let headers = this.getHeaders();
    let requestOptions = new RequestOptions({method : RequestMethod.Get, headers : headers});

    return this.http.get(inventoryApiUrl + id, requestOptions).pipe(
      map((data : Response) => {
        return data.json() as InventoryItem
      })
    );
  }

  postInventoryItem(emp : InventoryItem) {
    var body = JSON.stringify(emp);
    var headers = this.getHeaders();
    var requestOptions = new RequestOptions({method : RequestMethod.Post, headers : headers});

    return this.http.post(inventoryApiUrl, body, requestOptions).pipe(
      map((data : Response) =>{
        return data.json() as InventoryItem;
      })
    );
  }
 
  putInventoryItem(id, emp) {
    var body = JSON.stringify(emp);
    var headers = this.getHeaders();
    var requestOptions = new RequestOptions({ method: RequestMethod.Put, headers: headers });

    return this.http.put(inventoryApiUrl + id, body, requestOptions).pipe(
      map(response => response.json())
    );
  }

  deleteInventoryItem(id: string) {
    var headers = this.getHeaders();
    var requestOptions = new RequestOptions({ method: RequestMethod.Get, headers: headers });

    return this.http.delete(inventoryApiUrl + id, requestOptions).pipe(
      map(response => response.json())
    );
  }
}