declare @Id uniqueidentifier = NEWID()
declare @currencyId uniqueidentifier = (select Id from Currency where [Name] = @currencyName)
declare @typeId uniqueidentifier = (select Id from AssetType where [Name] = @typeName)

insert into Asset(Id, [Name], CurrencyId, TypeId, Balance) values (
	@Id,
	@name,
	@currencyId,
	@typeId,
	@balance
)

select
	a.Id as Id,
	a.[Name] as [Name],
	t.Id as TypeId,
	t.[Name] as TypeName,
	c.Id as CurrencyId,
	c.[Name] as CurrencyName
from Asset a
left join AssetType t on t.Id = a.TypeId
left join Currency c on c.Id = a.CurrencyId
where a.Id = @Id