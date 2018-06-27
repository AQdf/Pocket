declare @uah uniqueidentifier = (select Id from Currency where [Name] = 'UAH')
declare @usd uniqueidentifier = (select Id from Currency where [Name] = 'USD')
declare @eur uniqueidentifier = (select Id from Currency where [Name] = 'EUR')

update PeriodSummary
set TotalBalanceUAH = (
		iif(not exists(select top 1 1 from Asset where PeriodId = @periodId and CurrencyId = @uah), 0, (select sum(Balance) from Asset where PeriodId = @periodId and CurrencyId = @uah))
		+ iif(not exists(select top 1 1 from Asset where PeriodId = @periodId and CurrencyId = @usd), 0, ((select sum(Balance) from Asset where PeriodId = @periodId and CurrencyId = @usd) * PeriodSummary.ExchangeRateUSDtoUAH))
		+ iif(not exists(select top 1 1 from Asset where PeriodId = @periodId and CurrencyId = @eur), 0, ((select sum(Balance) from Asset where PeriodId = @periodId and CurrencyId = @eur) * PeriodSummary.ExchangeRateEURtoUAH))
	),
	TotalBalanceUSD = (
		iif(not exists(select top 1 1 from Asset where PeriodId = @periodId and CurrencyId = @uah), 0, ((select sum(Balance) from Asset where PeriodId = @periodId and CurrencyId = @uah) / PeriodSummary.ExchangeRateUSDtoUAH))
		+ iif(not exists(select top 1 1 from Asset where PeriodId = @periodId and CurrencyId = @usd), 0, (select sum(Balance) from Asset where PeriodId = @periodId and CurrencyId = @usd))
		+ iif(not exists(select top 1 1 from Asset where PeriodId = @periodId and CurrencyId = @eur), 0, ((select sum(Balance) from Asset where PeriodId = @periodId and CurrencyId = @eur) * PeriodSummary.ExchangeRateEURtoUAH/PeriodSummary.ExchangeRateUSDtoUAH))
	),
	TotalBalanceEUR = (
		iif (not exists(select top 1 1 from Asset where PeriodId = @periodId and CurrencyId = @uah), 0, ((select sum(Balance) from Asset where PeriodId = @periodId and CurrencyId = @uah) / PeriodSummary.ExchangeRateEURtoUAH))
		+ iif(not exists(select top 1 1 from Asset where PeriodId = @periodId and CurrencyId = @usd), 0, ((select sum(Balance) from Asset where PeriodId = @periodId and CurrencyId = @usd) * PeriodSummary.ExchangeRateUSDtoUAH/PeriodSummary.ExchangeRateEURtoUAH))
		+ iif(not exists(select top 1 1 from Asset where PeriodId = @periodId and CurrencyId = @eur), 0, (select sum(Balance) from Asset where PeriodId = @periodId and CurrencyId = @eur))
	)
where Id = @periodId