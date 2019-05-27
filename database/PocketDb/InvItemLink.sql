﻿CREATE TABLE [dbo].[InvItemLink]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(250) NOT NULL, 
    [Url] VARCHAR(MAX) NOT NULL, 
    [InvItemId] UNIQUEIDENTIFIER NOT NULL, 
    CONSTRAINT [FK_InvItemLink_ToInventoryItem] FOREIGN KEY ([InvItemId]) REFERENCES [InventoryItem]([Id])
)
