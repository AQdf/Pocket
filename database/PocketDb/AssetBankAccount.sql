CREATE TABLE [dbo].[AssetBankAccount]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [AssetId] UNIQUEIDENTIFIER NOT NULL UNIQUE, 
    [BankName] VARCHAR(50) NOT NULL, 
    [BankAccountId] VARCHAR(200) NOT NULL, 
    [LastSyncDateTime] DATETIME2 NULL, 
    [UserBankAuthDataId] UNIQUEIDENTIFIER NOT NULL, 
    [BankAccountName] NVARCHAR(200) NOT NULL, 
    CONSTRAINT [FK_AssetBankAccount_Bank] FOREIGN KEY ([BankName]) REFERENCES [Bank]([Name]), 
    CONSTRAINT [FK_AssetBankAccount_Asset] FOREIGN KEY ([AssetId]) REFERENCES [Asset]([Id]), 
    CONSTRAINT [FK_AssetBankAccount_UserBankAuthData] FOREIGN KEY ([UserBankAuthDataId]) REFERENCES [UserBankAuthData]([Id]) 
)
