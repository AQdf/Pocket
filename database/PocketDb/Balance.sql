CREATE TABLE [dbo].[Balance]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [AssetId] UNIQUEIDENTIFIER NOT NULL, 
    [Value] MONEY NOT NULL , 
    [ExchangeRateId] UNIQUEIDENTIFIER NULL, 
    [EffectiveDate] DATETIME2 NULL, 
    CONSTRAINT [FK_Balance_Asset] FOREIGN KEY ([AssetId]) REFERENCES [Asset]([Id]), 
    CONSTRAINT [FK_Balance_ExchangeRate] FOREIGN KEY ([ExchangeRateId]) REFERENCES [ExchangeRate]([Id])
)

GO
