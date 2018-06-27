declare @Id uniqueidentifier = NEWID();
declare @currencyId uniqueidentifier = (select Id from Currency where [Name] = @currencyName)

insert into Asset(Id, [Name], CurrencyId, TypeId, Balance, PeriodId) values (
	@Id,
	@name,
	@currencyId,
	(select Id from AssetType where [Name] = @typeName),
	@balance,
	@periodId
)

select
	a.Id as Id,
	a.[Name] as [Name],
	t.Id as TypeId,
	t.[Name] as TypeName,
	c.Id as CurrencyId,
	c.[Name] as CurrencyName,
	a.Balance as Balance,
	a.PeriodId as PeriodId,
	a.AssetTemplateId as AssetTemplateId
from Asset a
left join AssetType t on t.Id = a.TypeId
left join Currency c on c.Id = a.CurrencyId
where a.Id = @Id