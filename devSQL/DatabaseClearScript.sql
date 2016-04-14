use InvoiceSystem

drop table dbo.Logs;

drop view inv.LogTab;

declare @v_ObjectName nvarchar(4000);
declare @v_TmpName nvarchar(4000);
declare @v_Cmd nvarchar(4000);

declare @v_SchemaId int =	(
							select 
								schema_id
							from sys.schemas
							where name = 'inv'
						);

----------------------------------- Foreign Keys -----------------------------------

declare cursorFK cursor local fast_forward for
(
	select
		(select distinct name from sys.objects where object_id = fk.constraint_object_id),
		(select distinct name from sys.tables where object_id = fk.parent_object_id)
	from sys.foreign_key_columns fk
);

open cursorFK;

fetch next from cursorFK into @v_ObjectName, @v_TmpName;

while @@FETCH_STATUS = 0
begin
	set @v_Cmd = 'alter table inv.' + @v_TmpName + ' drop constraint ' + @v_ObjectName;

	exec sp_executesql @v_Cmd;

	fetch next from cursorFK into @v_ObjectName, @v_TmpName;
end

close cursorFK;
deallocate cursorFK;

----------------------------------- Tables -----------------------------------

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

----------------------------------- Procedures -----------------------------------

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