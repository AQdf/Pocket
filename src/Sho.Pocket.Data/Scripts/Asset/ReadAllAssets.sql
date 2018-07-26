select
	a.Id as Id,
	a.[Name] as [Name],
	t.Id as TypeId,
	t.[Name] as TypeName,
	c.Id as CurrencyId,
	c.[Name] as CurrencyName,
	a.Balance as Balance
from Asset a
left join AssetType t on t.Id = a.TypeId
left join Currency c on c.Id = a.CurrencyId