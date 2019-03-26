update Asset
set [Name] = @name,
	IsActive = @isActive
where Id = @id