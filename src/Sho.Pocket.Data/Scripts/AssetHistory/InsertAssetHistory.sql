declare @Id uniqueidentifier = NEWID()

insert into AssetHistory ([Id], [AssetId], [EffectiveDate], [ExchangeRateId], [Balance]) values (
	@Id,
	@assetId,
	@effectiveDate,
	@exchangeRateId,
	@balance
)

select [Id], [AssetId], [EffectiveDate], [ExchangeRateId], [Balance] from [AssetHistory]
where [Id] = @Id