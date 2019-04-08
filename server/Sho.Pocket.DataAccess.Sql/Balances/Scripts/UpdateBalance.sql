update Balance
set [Value] = @value
where Id = @id

select * from Balance
where Id = @id
