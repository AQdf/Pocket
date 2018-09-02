﻿select * from Balance b 
join Asset a on a.Id = b.AssetId
join ExchangeRate r on r.Id = b.ExchangeRateId
order by b.EffectiveDate desc