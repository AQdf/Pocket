update Asset
set [Name] = @name,
	CurrencyId = @currencyId,
	IsActive = @isActive
where Id = @id