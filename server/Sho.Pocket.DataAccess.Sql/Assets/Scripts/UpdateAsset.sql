update Asset
set [Name] = @name,
	CurrencyId = @currencyId,
	TypeId = @typeId
where Id = @id