﻿CREATE TABLE [BalanceNote]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [EffectiveDate] DATETIME2 NOT NULL, 
    [Content] NVARCHAR(MAX) NULL, 
    [UserId] UNIQUEIDENTIFIER NOT NULL
)
