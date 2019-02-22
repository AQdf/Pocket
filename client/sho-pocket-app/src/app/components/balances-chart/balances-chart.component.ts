import { Component, OnInit } from '@angular/core';
import { Chart } from 'angular-highcharts';

import { BalanceService } from '../../services/balance.service';
import { Balance } from 'src/app/models/balance.model';

@Component({
  selector: 'app-balances-chart',
  templateUrl: './balances-chart.component.html',
  styleUrls: ['./balances-chart.component.css']
})
export class BalancesChartComponent implements OnInit {

  constructor(public balanceService : BalanceService) { }

  ngOnInit() {
    this.balanceService.reload();
  }
}
