import { Balance } from'../models/balance.model'
import { Currency } from './currency.model';
import { ExchangeRate } from './exchange-rate.model';

export class Balances {
  items: Balance[];
  totalBalance: number;
  count: number;
  exchangeRates: ExchangeRate[];
}
