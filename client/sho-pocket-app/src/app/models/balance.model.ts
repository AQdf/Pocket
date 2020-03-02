import {Asset} from'../models/asset.model'

export class Balance {
  id: string;
  assetId: string;
  effectiveDate: string;
  value: number;
  asset: Asset;
  isBankAccount: boolean;
}