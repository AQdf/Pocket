update Balance
set [Value] = @value
where Id = @id and UserOpenId = @userOpenId

select * from Balance
where Id = @id
