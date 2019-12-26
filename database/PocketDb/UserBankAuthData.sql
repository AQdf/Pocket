CREATE TABLE [dbo].[UserBankAuthData]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [UserId] UNIQUEIDENTIFIER NOT NULL, 
    [BankName] VARCHAR(50) NOT NULL, 
    [Token] NVARCHAR(MAX) NULL, 
    [BankClientId] VARCHAR(50) NULL, 
    CONSTRAINT [FK_UserBankAuthData_Bank] FOREIGN KEY ([BankName]) REFERENCES [Bank]([Name])
)
