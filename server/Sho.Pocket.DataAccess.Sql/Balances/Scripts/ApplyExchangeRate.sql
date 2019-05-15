update Balance
set ExchangeRateId = @exchangeRateId
where AssetId in (
	select Id from Asset 
	where Currency = @currency and EffectiveDate = @effectiveDate
)