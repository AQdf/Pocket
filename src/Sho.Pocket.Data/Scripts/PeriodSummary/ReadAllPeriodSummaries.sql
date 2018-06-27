select 
	s.Id as Id,
	s.ReportedDate as ReportedDate,
	c.Id as CurrencyId,
	c.[Name] as CurrencyName,
	s.TotalBalanceUAH as TotalBalanceUAH,
	s.TotalBalanceUSD as TotalBalanceUSD,
	s.TotalBalanceEUR as TotalBalanceEUR,
	s.ExchangeRateUSDtoUAH as ExchangeRateUSDtoUAH,
	s.ExchangeRateEURtoUAH as ExchangeRateEURtoUAH
from PeriodSummary s
left join Currency c on c.Id = s.CurrencyId