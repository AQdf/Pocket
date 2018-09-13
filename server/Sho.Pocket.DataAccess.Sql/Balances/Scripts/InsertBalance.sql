insert into Balance([Id], [AssetId], [Value], [ExchangeRateId], [EffectiveDate]) values (
	NEWID(),
	@assetId,
	@value,
	@exchangeRateId,
	@effectiveDate
)