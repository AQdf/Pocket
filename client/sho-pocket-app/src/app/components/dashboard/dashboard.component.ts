import { Component, OnInit } from '@angular/core';

import { BalanceService } from '../../services/balance.service'

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  constructor(public balanceService : BalanceService) { }

  ngOnInit() {
  }
  
}