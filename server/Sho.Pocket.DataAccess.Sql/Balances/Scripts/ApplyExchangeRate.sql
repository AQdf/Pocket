update Balance
set ExchangeRateId = @exchangeRateId
where AssetId in (
	select Id from Asset 
	where CurrencyId = @currencyId and EffectiveDate = @effectiveDate
)