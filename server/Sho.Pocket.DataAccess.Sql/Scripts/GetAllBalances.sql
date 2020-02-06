select * from Balance
join Asset on Asset.Id = Balance.AssetId
left join ExchangeRate on ExchangeRate.Id = Balance.ExchangeRateId
where Balance.UserOpenId = @userOpenId
order by Balance.EffectiveDate desc