import { Component, OnInit, Input, Output, EventEmitter, SimpleChange } from '@angular/core';

import { ExchangeRateService } from '../../services/exchange-rate.service';
import { ExchangeRate } from '../../models/exchange-rate.model';

@Component({
  selector: 'app-exchange-rates',
  templateUrl: './exchange-rates.component.html',
  styleUrls: ['./exchange-rates.component.css']
})
export class ExchangeRatesComponent implements OnInit {

  @Input() effectiveDate: string;
  effectiveDateFriendly: string;
  exchangeRates: ExchangeRate[];
  collapsed: boolean = false;

  constructor(private exchangeRateService: ExchangeRateService) { }

  ngOnInit() {
  }

  ngOnChanges(changes: {[propKey: string]: SimpleChange}) {
    this.effectiveDate = changes.effectiveDate.currentValue;
    if (this.effectiveDate) {
      this.effectiveDateFriendly = new Date(this.effectiveDate).toDateString();
      this.loadExchangeRates(this.effectiveDate);
    }
  }

  loadExchangeRates(effectiveDate: string) {
    this.exchangeRateService.getExchangeRates(effectiveDate).subscribe((response: ExchangeRate[]) => {
      this.exchangeRates = response;
    });
  }

  togglePanel() {
    this.collapsed = !this.collapsed;
  }
}
