DECLARE @id uniqueidentifier = (
	select top 1 Id from ExchangeRate
	where BaseCurrency = @baseCurrency
		and CounterCurrency = @counterCurrency
		and EffectiveDate = @effectiveDate)

IF @id IS NULL
	BEGIN
		SET @id = NEWID();
		insert into ExchangeRate([Id], [EffectiveDate], [BaseCurrency], [CounterCurrency], [Rate]) values (
			@id,
			@effectiveDate,
			@baseCurrency,
			@counterCurrency,
			@rate
		)
	END
ELSE
	BEGIN
		update ExchangeRate
		set Rate = @rate
		where Id = @id
	END

select * from ExchangeRate where Id = @id