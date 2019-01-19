import { Balance } from'../models/balance.model'
import { Currency } from './currency.model';
import { ExchangeRate } from './exchange-rate.model';
import { BalanceTotal } from './balance-total.model';

export class Balances {
  items: Balance[];
  totalBalance: BalanceTotal[];
  count: number;
  exchangeRates: ExchangeRate[];
}
