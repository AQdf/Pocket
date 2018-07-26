CREATE TABLE [dbo].[Asset]
(
	[Id] UNIQUEIDENTIFIER DEFAULT NEWID() NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(200) NOT NULL, 
    [TypeId] UNIQUEIDENTIFIER NULL, 
    [CurrencyId] UNIQUEIDENTIFIER NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1, 
    [Balance] MONEY NOT NULL DEFAULT 0, 
    [ExchangeRateId] UNIQUEIDENTIFIER NULL, 
    CONSTRAINT [FK_Asset_AssetType] FOREIGN KEY ([TypeId]) REFERENCES [AssetType]([Id]), 
    CONSTRAINT [FK_Asset_Currency] FOREIGN KEY ([CurrencyId]) REFERENCES [Currency]([Id]), 
    CONSTRAINT [FK_Asset_ExchangeRate] FOREIGN KEY ([ExchangeRateId]) REFERENCES [ExchangeRate]([Id])
)
