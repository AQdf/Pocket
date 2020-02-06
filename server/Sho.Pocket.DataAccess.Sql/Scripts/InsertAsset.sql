declare @id uniqueidentifier = NEWID()

insert into Asset([Id], [Name], [Currency], [IsActive], [UserOpenId]) values (
	@id,
	@name,
	@currency,
	@isActive,
	@userOpenId
)

SELECT [Asset].[Id] AS [ID]
      ,[Asset].[Name] AS [Name]
      ,[Asset].[IsActive] AS [IsActive]
      ,[Asset].[Currency] AS [Currency]
FROM [dbo].[Asset]
WHERE [Asset].[Id] = @id