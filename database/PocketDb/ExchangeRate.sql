CREATE TABLE [dbo].[ExchangeRate]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [EffectiveDate] DATETIME2 NOT NULL, 
    [BaseCurrencyId] UNIQUEIDENTIFIER NOT NULL, 
    [CounterCurrencyId] UNIQUEIDENTIFIER NOT NULL, 
    [Rate] MONEY NOT NULL DEFAULT 1.0, 
    CONSTRAINT [FK_ExchangeRate_BaseCurrency] FOREIGN KEY ([BaseCurrencyId]) REFERENCES [Currency]([Id]), 
    CONSTRAINT [FK_ExchangeRate_CounterCurrency] FOREIGN KEY ([CounterCurrencyId]) REFERENCES [Currency]([Id])
)
