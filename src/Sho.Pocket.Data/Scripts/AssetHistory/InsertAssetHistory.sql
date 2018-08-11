declare @Id uniqueidentifier = NEWID()
declare @assetId uniqueidentifier = (select [Id] from [Asset] where [Name] = @assetName)

insert into AssetHistory ([Id], [AssetId], [EffectiveDate], [ExchangeRateId], [Balance]) values (
	@Id,
	@assetId,
	@effectiveDate,
	@exchangeRateId,
	@balance
)

select [Id], [AssetId], [EffectiveDate], [ExchangeRateId], [Balance] from [AssetHistory]
where [Id] = @Id