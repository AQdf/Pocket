select EffectiveDate from Balance
where UserOpenId = @userOpenId
group by EffectiveDate
order by EffectiveDate desc