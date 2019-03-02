import { Currency } from "./currency.model";

export class Asset {
  id : string;
  name : string;
  currencyId : string;
  currencyName: string;
  isActive: boolean;
}