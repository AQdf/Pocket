CREATE TABLE [dbo].[InvItemImage]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [InvItemId] UNIQUEIDENTIFIER NOT NULL, 
    [FileName] NVARCHAR(250) NOT NULL, 
    CONSTRAINT [FK_InvItemImage_InventoryItem] FOREIGN KEY ([InvItemId]) REFERENCES [InventoryItem]([Id])
)
