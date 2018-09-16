DECLARE @id uniqueidentifier = NEWID();

insert into Balance([Id], [AssetId], [Value], [ExchangeRateId], [EffectiveDate]) values (
	@id,
	@assetId,
	@value,
	@exchangeRateId,
	@effectiveDate
)

select * from Balance where Id = @id