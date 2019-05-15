SELECT [Asset].[Id] AS [ID]
      ,[Asset].[Name] AS [Name]
      ,[Asset].[IsActive] AS [IsActive]
      ,[Asset].[Currency] AS [Currency]
FROM [dbo].[Asset]
WHERE [Asset].[UserOpenId] = @userOpenId
ORDER BY [Asset].[Name] ASC
