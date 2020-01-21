export class ExchangeRate {
  id: string;
  effectiveDate: number;
  baseCurrency: string;
  counterCurrency: string;
  buy: number;
  sell: number;
  provider: string;
}