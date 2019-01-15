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
		0
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
	BaseCurrencyId uniqueidentifier
)

insert into #TempExchangeRate (Id, AssetId, BaseCurrencyId)
select ExchangeRate.Id, Asset.Id, Asset.CurrencyId
from Asset
join ExchangeRate on Asset.CurrencyId = ExchangeRate.BaseCurrencyId
where ExchangeRate.EffectiveDate = @effectiveDate

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