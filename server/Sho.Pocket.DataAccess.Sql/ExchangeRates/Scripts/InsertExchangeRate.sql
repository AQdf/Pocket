DECLARE @id UNIQUEIDENTIFIER = NEWID()

INSERT INTO [dbo].[ExchangeRate] (
	[Id],
	[EffectiveDate],
	[BaseCurrencyId],
	[CounterCurrencyId],
	[Rate]
) VALUES (
	@id,
	@effectiveDate,
	@baseCurrencyId,
	@counterCurrencyId,
	@rate
)

SELECT * FROM [dbo].[ExchangeRate]
WHERE Id = @id