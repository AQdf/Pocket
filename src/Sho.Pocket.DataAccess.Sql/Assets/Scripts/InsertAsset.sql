insert into Asset(Id, [Name], CurrencyId, TypeId) values (
	NEWID(),
	@name,
	@currencyId,
	@typeId
)