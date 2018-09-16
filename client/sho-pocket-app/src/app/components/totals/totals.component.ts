import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-totals',
  templateUrl: './totals.component.html',
  styleUrls: ['./totals.component.css']
})
export class TotalsComponent implements OnInit {
  totalBalance: number;

  constructor(public client : HttpClient) { }

  ngOnInit() {
    this.getCurrentTotalBalance();
  }

  getCurrentTotalBalance()
  {
    this.client.get<number>('http://localhost:58192/api/balances/total').pipe(
      map((data : number) =>{
        return data;
      })
    ).subscribe(response => {
      this.totalBalance = response;
    });
  }
}