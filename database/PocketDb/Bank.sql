CREATE TABLE [dbo].[Bank]
(
	[Name] VARCHAR(50) NOT NULL PRIMARY KEY, 
    [Country] VARCHAR(50) NOT NULL, 
    [Active] BIT NOT NULL, 
    [ApiUrl] VARCHAR(MAX) NULL, 
    [SyncFreqInSeconds] INT NULL
)
