select * from Asset a
left join AssetType t on t.Id = a.TypeId
left join Currency c on c.Id = a.CurrencyId
order by t.[Name] asc