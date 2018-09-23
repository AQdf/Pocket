update Asset
set [Name] = @name,
	CurrencyId = @currencyId,
	TypeId = @typeId,
	IsActive = @isActive
where Id = @id