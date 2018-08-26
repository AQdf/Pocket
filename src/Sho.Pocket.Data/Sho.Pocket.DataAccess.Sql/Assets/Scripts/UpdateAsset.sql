update Asset
set [Name] = @name,
	CurrencyId = (select Id from Currency where [Name] = @currencyName),
	TypeId = (select Id from AssetType where [Name] = @typeName),
	Balance = @balance
where Id = @id