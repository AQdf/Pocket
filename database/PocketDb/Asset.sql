﻿CREATE TABLE [dbo].[Asset]
(
	[Id] UNIQUEIDENTIFIER DEFAULT NEWID() NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(200) NOT NULL, 
    [Currency] VARCHAR(3) NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1, 
    [UserOpenId] UNIQUEIDENTIFIER NULL, 
	CONSTRAINT [FK_Asset_CurrencyId] FOREIGN KEY ([Currency]) REFERENCES [Currency]([Name])
)
