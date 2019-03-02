SELECT [Asset].[Id] AS [ID]
      ,[Asset].[Name] AS [Name]
      ,[Asset].[IsActive] AS [IsActive]
      ,[Asset].[CurrencyId] AS [CurrencyId]
	  ,[Currency].[Name] AS CurrencyName
FROM [dbo].[Asset]
JOIN [dbo].[Currency] ON [Currency].[Id] = [Asset].[CurrencyId]
ORDER BY [Asset].[Name] ASC