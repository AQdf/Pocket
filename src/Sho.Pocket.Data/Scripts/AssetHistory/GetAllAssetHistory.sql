select
	ah.[Id] as Id,
	ah.[AssetId] as AssetId,
    a.[Name] as AssetName,
	a.[IsActive] as AssetIsActive,
	ah.[EffectiveDate] as EffectiveDate,
    ah.[ExchangeRateId] as ExchangeRateId,
    ah.[Balance] as Balance
from [AssetHistory] ah
join [Asset] a on a.Id = ah.AssetId