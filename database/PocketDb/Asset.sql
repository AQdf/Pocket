﻿CREATE TABLE [Asset]
(
	[Id] UNIQUEIDENTIFIER DEFAULT NEWID() NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(200) NOT NULL, 
    [Currency] VARCHAR(3) NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1, 
    [UserId] UNIQUEIDENTIFIER NOT NULL, 
	CONSTRAINT [FK_Asset_Currency_Currency] FOREIGN KEY ([Currency]) REFERENCES [Currency]([Name])
)
