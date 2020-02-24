CREATE TABLE [Balance]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [AssetId] UNIQUEIDENTIFIER NOT NULL, 
    [Value] MONEY NOT NULL , 
    [ExchangeRateId] UNIQUEIDENTIFIER NOT NULL, 
    [EffectiveDate] DATETIME2 NOT NULL, 
    [UserId] UNIQUEIDENTIFIER NOT NULL, 
    CONSTRAINT [FK_Balance_Asset_AssetId] FOREIGN KEY ([AssetId]) REFERENCES [Asset]([Id]), 
    CONSTRAINT [FK_Balance_ExchangeRate_ExchangeRateId] FOREIGN KEY ([ExchangeRateId]) REFERENCES [ExchangeRate]([Id])
)

GO
