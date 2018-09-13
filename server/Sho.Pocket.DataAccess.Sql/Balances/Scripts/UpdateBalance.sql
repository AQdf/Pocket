update Balance
set AssetId = @assetId,
	EffectiveDate = @effectiveDate,
	[Value] = @value,
	ExchangeRateId = @exchangeRateId
where Id = @id