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
where Currency.Id != @currencyUahId

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
where Currency.Id = @currencyUahId

create table #TempExchangeRate
(
	Id uniqueidentifier,
	AssetId uniqueidentifier,
	IsActive bit,
	BaseCurrencyId uniqueidentifier
)

insert into #TempExchangeRate (Id, AssetId, BaseCurrencyId, IsActive)
select ExchangeRate.Id, Asset.Id, Asset.CurrencyId, Asset.IsActive
from Asset
join ExchangeRate on Asset.CurrencyId = ExchangeRate.BaseCurrencyId
where ExchangeRate.EffectiveDate = @effectiveDate

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
		#TempExchangeRate.Id,
		@effectiveDate
	from [Balance]
	JOIN #TempExchangeRate on #TempExchangeRate.AssetId = Balance.AssetId
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
		[AssetId],
		0,
		#TempExchangeRate.Id,
		@effectiveDate
	from #TempExchangeRate
	where IsActive = 1
END
