use InvoiceSystem

go

	drop schema "inv";

go

	declare @v_SchemaId int =	(
								select 
									schema_id
								from sys.schemas
								where name = 'inv'
							);
	

	select 'drop table "inv.' + name + '";'
	from sys.tables
	where schema_id = @v_SchemaId


	select 'drop procedure "inv.' + name + '";'
	from sys.procedures
	where schema_id = @v_SchemaId

	declare cursor 