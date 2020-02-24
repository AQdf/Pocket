select * from Balance
join Asset on Asset.Id = Balance.AssetId
left join ExchangeRate on ExchangeRate.Id = Balance.ExchangeRateId
where Balance.UserId = @userId
order by Balance.EffectiveDate desc