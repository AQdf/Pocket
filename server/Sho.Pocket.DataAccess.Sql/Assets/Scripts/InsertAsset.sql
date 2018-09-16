declare @id uniqueidentifier = NEWID()

insert into Asset(Id, [Name], CurrencyId, TypeId) values (
	@id,
	@name,
	@currencyId,
	@typeId
)

select * from Asset where Id = @id