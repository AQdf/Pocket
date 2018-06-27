declare @Id uniqueidentifier = NEWID();

insert into PeriodSummary(Id, ReportedDate, ExchangeRateUSDtoUAH, ExchangeRateEURtoUAH) values (
	@Id,
	@reportedDate,
	@xRateUSDtoUAH,
	@xRateEURtoUAH
)

select top 1
	s.Id as Id,
	s.ReportedDate as ReportedDate,
	s.ExchangeRateUSDtoUAH as ExchangeRateUSDtoUAH,
	s.ExchangeRateEURtoUAH as ExchangeRateEURtoUAH
from PeriodSummary s
left join Currency c on c.Id = s.CurrencyId
where s.Id = @Id