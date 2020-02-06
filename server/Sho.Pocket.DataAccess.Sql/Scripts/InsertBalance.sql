DECLARE @id uniqueidentifier = NEWID();

insert into Balance([Id], [AssetId], [Value], [ExchangeRateId], [EffectiveDate], [UserOpenId]) values (
	@id,
	@assetId,
	@value,
	@exchangeRateId,
	@effectiveDate,
	@userOpenId
)

select * from Balance where Id = @id