use InvoiceSystem

declare @v_ObjectName nvarchar(4000);

declare @v_SchemaId int =	(
							select 
								schema_id
							from sys.schemas
							where name = 'inv'
						);
	
declare cursorTabNames cursor local fast_forward for
(
	select 'drop table inv.' + name + ';'
	from sys.tables
	where schema_id = @v_SchemaId
);

open cursorTabNames;

fetch next from cursorTabNames into @v_ObjectName;

while @@FETCH_STATUS = 0
begin
	exec sp_executesql @v_ObjectName;

	fetch next from cursorTabNames into @v_ObjectName;
end

close cursorTabNames;
deallocate cursorTabNames;


declare cursorProcNames cursor local fast_forward for
(
	select 'drop procedure inv.' + name + ';'
	from sys.procedures
	where schema_id = @v_SchemaId
);

open cursorProcNames;

fetch next from cursorProcNames into @v_ObjectName;

while @@FETCH_STATUS = 0
begin
	exec sp_executesql @v_ObjectName;

	fetch next from cursorProcNames into @v_ObjectName;
end

close cursorProcNames;
deallocate cursorProcNames;