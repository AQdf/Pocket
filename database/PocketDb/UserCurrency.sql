CREATE TABLE [UserCurrency]
(
    [UserId] UNIQUEIDENTIFIER NOT NULL, 
    [Currency] VARCHAR(3) NOT NULL, 
    [IsPrimary] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_UserCurrency_Currency_Currency] FOREIGN KEY ([Currency]) REFERENCES [Currency]([Name]), 
    PRIMARY KEY ([Currency], [UserId]) 
)
