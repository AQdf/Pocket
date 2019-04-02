update Asset
set [Name] = @name,
	IsActive = @isActive
where Id = @id

select * from Asset where Id = @id