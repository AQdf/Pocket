update [Asset]
set [Name] = @name,
	[Currency] = @currency,
	[IsActive] = @isActive
where [Id] = @id AND [UserOpenId] = @userOpenId

SELECT [Asset].[Id] AS [ID]
      ,[Asset].[Name] AS [Name]
      ,[Asset].[IsActive] AS [IsActive]
      ,[Asset].[Currency] AS [Currency]
FROM [dbo].[Asset]
WHERE [Asset].[Id] = @id