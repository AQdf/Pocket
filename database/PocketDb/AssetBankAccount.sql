CREATE TABLE [dbo].[AssetBankAccount]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [AssetId] UNIQUEIDENTIFIER NOT NULL UNIQUE, 
    [BankName] VARCHAR(50) NOT NULL, 
    [BankAccountId] VARCHAR(200) NULL, 
    [LastSyncDateTime] DATETIME2 NULL, 
    [BankAccountName] NVARCHAR(200) NULL, 
    [Token] NVARCHAR(MAX) NULL, 
    [BankClientId] VARCHAR(50) NULL, 
    CONSTRAINT [FK_AssetBankAccount_Bank] FOREIGN KEY ([BankName]) REFERENCES [Bank]([Name]), 
    CONSTRAINT [FK_AssetBankAccount_Asset] FOREIGN KEY ([AssetId]) REFERENCES [Asset]([Id])
)
