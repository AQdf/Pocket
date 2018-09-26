INSERT INTO [dbo].[Balance]
           ([Id]
           ,[AssetId]
           ,[Value]
           ,[ExchangeRateId]
           ,[EffectiveDate])
SELECT 
	NEWID(),
	[Asset].[Id],
	0,
	null,
	@effectiveDate
FROM [Asset]
where [IsActive] = 1