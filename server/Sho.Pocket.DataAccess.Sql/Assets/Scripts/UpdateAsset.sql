update [Asset]
set [Name] = @name,
	[CurrencyId] = @currencyId,
	[IsActive] = @isActive
where [Id] = @id

SELECT [Asset].[Id] AS [ID]
      ,[Asset].[Name] AS [Name]
      ,[Asset].[IsActive] AS [IsActive]
      ,[Asset].[CurrencyId] AS [CurrencyId]
	  ,[Currency].[Name] AS CurrencyName
FROM [dbo].[Asset]
JOIN [dbo].[Currency] ON [Currency].[Id] = [Asset].[CurrencyId]
WHERE [Asset].[Id] = @id