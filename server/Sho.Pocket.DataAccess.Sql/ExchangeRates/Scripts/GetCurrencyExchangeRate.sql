select * from ExchangeRate
where BaseCurrency = @baseCurrency
	and EffectiveDate = @effectiveDate