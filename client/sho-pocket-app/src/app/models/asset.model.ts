import { AssetType } from "./asset-type.model";
import { Currency } from "./currency.model";

export class Asset {
  id : string;
  name : string;
  typeId : string;
  currencyId : string;
  type: AssetType;
  currency: Currency;
}