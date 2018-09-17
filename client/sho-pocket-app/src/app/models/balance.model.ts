import {Asset} from'../models/asset.model'

export class Balance {
  id : string;
  assetId : string;
  effectiveDate : string;
  value: number;
  exchangeRateId : string;
  exchangeRateValue: number;
  asset: Asset;
}