﻿CREATE TABLE [ExchangeRate]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [EffectiveDate] DATETIME2 NOT NULL, 
    [BaseCurrency] VARCHAR(3) NOT NULL, 
    [CounterCurrency] VARCHAR(3) NOT NULL,
    [BuyRate] MONEY NOT NULL DEFAULT 1.0, 
    [SellRate] MONEY NOT NULL DEFAULT 1.0, 
    [Provider] VARCHAR(50) NULL, 
    CONSTRAINT [FK_ExchangeRate_Currency_BaseCurrency] FOREIGN KEY ([BaseCurrency]) REFERENCES [Currency]([Name]), 
    CONSTRAINT [FK_ExchangeRate_Currency_CounterCurrency] FOREIGN KEY ([CounterCurrency]) REFERENCES [Currency]([Name])
)
