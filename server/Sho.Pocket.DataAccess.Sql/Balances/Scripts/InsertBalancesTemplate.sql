
declare @currencyUahId uniqueidentifier = (select Id from Currency where [Name] = 'UAH')

insert into [dbo].[ExchangeRate]
           ([Id]
           ,[EffectiveDate]
           ,[BaseCurrencyId]
           ,[CounterCurrencyId]
           ,[Rate])
select	NEWID(),
		@effectiveDate,
		Currency.Id,
		@currencyUahId,
		1
from Currency

IF EXISTS (SELECT TOP 1 1 FROM [dbo].[Balance])
BEGIN
	insert into [dbo].[Balance]
			   ([Id]
			   ,[AssetId]
			   ,[Value]
			   ,[ExchangeRateId]
			   ,[EffectiveDate])
	select 
		NEWID(),
		[Balance].[AssetId],
		[Balance].[Value],
		AssetExchangeRate.Id,
		@effectiveDate
	from [Balance]
	outer apply (
		select ExchangeRate.Id
		from Asset
		join ExchangeRate on Asset.CurrencyId = ExchangeRate.BaseCurrencyId
		where ExchangeRate.EffectiveDate = @effectiveDate
	) as AssetExchangeRate
	where [Balance].EffectiveDate = (
		select top 1 EffectiveDate 
		from [Balance]
		order by EffectiveDate desc
	)
END
ELSE BEGIN
	insert into [dbo].[Balance]
			   ([Id]
			   ,[AssetId]
			   ,[Value]
			   ,[ExchangeRateId]
			   ,[EffectiveDate])
	select 
		NEWID(),
		[Asset].[Id],
		0,
		ExchangeRate.Id,
		@effectiveDate
	from [Asset]
	join ExchangeRate on Asset.CurrencyId = ExchangeRate.BaseCurrencyId
	where IsActive = 1 and ExchangeRate.EffectiveDate = @effectiveDate
END

select * from Balance where EffectiveDate = @effectiveDate
