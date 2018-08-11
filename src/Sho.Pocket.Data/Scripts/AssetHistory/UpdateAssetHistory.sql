declare @assetId uniqueidentifier = (select [Id] from [Asset] where [Name] = @assetName)

update [dbo].[AssetHistory]
set [AssetId] = @assetId,
    [EffectiveDate] = @effectiveDate,
    [ExchangeRateId] = @exchangeRateId,
    [Balance] = @balance
where Id = @id