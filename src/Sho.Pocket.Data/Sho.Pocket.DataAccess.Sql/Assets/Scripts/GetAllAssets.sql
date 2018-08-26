select * from Asset a
left join AssetType t on t.Id = a.TypeId
left join Currency c on c.Id = a.CurrencyId
where IsActive = 1
order by t.[Name] asc