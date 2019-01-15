select * from Balance
join Asset on Asset.Id = Balance.AssetId
left join ExchangeRate on ExchangeRate.Id = Balance.ExchangeRateId
left join Currency BaseCurrency on BaseCurrency.Id = ExchangeRate.BaseCurrencyId
left join Currency CounterCurrency on CounterCurrency.Id = ExchangeRate.CounterCurrencyId
order by Balance.EffectiveDate desc