update ExchangeRate
set Rate = @rate
where Id = @id

select * from ExchangeRate
where Id = @id
