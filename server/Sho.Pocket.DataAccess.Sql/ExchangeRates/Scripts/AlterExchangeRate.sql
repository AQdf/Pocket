DECLARE @baseCurrencyId uniqueidentifier = (select CurrencyId from Asset where Id = @assetId)
DECLARE @counterCurrencyId uniqueidentifier = (select Id from Currency where [Name] = @counterCurrencyName)
DECLARE @id uniqueidentifier = (select top 1 Id from ExchangeRate where BaseCurrencyId = @baseCurrencyId and CounterCurrencyId = @counterCurrencyId and EffectiveDate = @effectiveDate)

IF @id IS NULL
	BEGIN
		SET @id = NEWID();
		insert into ExchangeRate([Id], [EffectiveDate], [BaseCurrencyId], [CounterCurrencyId], [Rate]) values (
			@id,
			@effectiveDate,
			@baseCurrencyId,
			@counterCurrencyId,
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