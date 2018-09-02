update Asset
set [Name] = @name,
	CurrencyId = @currencyId,
	TypeId = (select Id from AssetType where [Name] = @typeId)
where Id = @id