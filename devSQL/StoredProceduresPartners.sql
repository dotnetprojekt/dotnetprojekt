use InvoiceSystem

go

	if(object_id('inv.usp_PartnersAdd') is not null)
		drop procedure inv.usp_PartnersAdd;

	if(object_id('inv.usp_PartnersList') is not null)
		drop procedure inv.usp_PartnersList;

	if(object_id('inv.usp_PartnerSearch') is not null)
		drop procedure inv.usp_PartnerSearch;

	if(object_id('inv.usp_PartnerGetById') is not null)
		drop procedure inv.usp_PartnerGetById;

go

	create procedure inv.usp_PartnersAdd
		@p_FirstName nvarchar(128),
		@p_LastName nvarchar(128),
		@p_CompanyName nvarchar(256),
		@p_Vatin decimal(24,0),
		@p_Address nvarchar(2048),
		@p_User int
	as
	begin
		set nocount on;
		set xact_abort on;

		if	(
				(@p_FirstName is not null and @p_LastName is not null and @p_CompanyName is not null)
				or (@p_FirstName is null and @p_LastName is null and @p_CompanyName is not null)
				or (@p_FirstName is not null and @p_LastName is not null and @p_CompanyName is null)
			)
			and @p_User is not null
		begin
			insert into inv.Partners with(rowlock)
			(
				Part_FirstName,
				Part_LastName,
				Part_CompanyName,
				Part_Vatin,
				Part_Address
			)
			values
			(
				@p_FirstName,
				@p_LastName,
				@p_CompanyName,
				@p_Vatin,
				@p_Address
			);
		end
		else
			raiserror (15600,-1,-1, 'inv.usp_PartnerAdd');
	end

go

	create procedure inv.usp_PartnersList
		@p_pageNumber int,
		@p_rowsPerPage int
	as
	begin
		select
			Part_Id,
			Part_FirstName,
			Part_LastName,
			Part_CompanyName,
			Part_Vatin,
			Part_Address
		from inv.Partners with(nolock)
		order by Part_Id
		offset ((@p_pageNumber-1)*@p_rowsPerPage) rows
		fetch next (@p_rowsPerPage) rows only;
	end

go

	create procedure inv.usp_PartnerSearch
		@p_FirstName nvarchar(128),
		@p_LastName nvarchar(128),
		@p_CompanyName nvarchar(256),
		@p_Vatin decimal(24,0),
		@p_pageNumber int = 1,
		@p_rowsPerPage int = 2147483647
	as
	begin
		declare @v_QueryBody nvarchar(max) =
'select
	Part_Id,
	Part_FirstName,
	Part_LastName,
	Part_CompanyName,
	Part_Vatin,
	Part_Address
	from inv.Partners
where 1 = 1';

	declare @v_QueryConditions nvarchar(max) = '';

	declare @v_QueryEnd nvarchar(max) = 
char(13)+char(10)+'order by Part_Id
offset ('+ convert(nvarchar(32),(@p_pageNumber-1)*@p_rowsPerPage) +') rows
fetch next ('+ convert(nvarchar(32),@p_rowsPerPage)+') rows only';

		if(@p_vatin is not null)
			set @v_QueryConditions = @v_QueryConditions +char(13)+char(10)+ '	and Part_Vatin = ' + convert(nvarchar(24),@p_vatin);
		else
		begin
			if(@p_firstName is not null)
				set @v_QueryConditions = @v_QueryConditions +char(13)+char(10)+ '	and Part_FirstName = ''' + @p_firstName + '''';

			if(@p_lastName is not null)
				set @v_QueryConditions = @v_QueryConditions +char(13)+char(10)+ '	and Part_LastName = ''' + @p_lastName + '''';

			if(@p_companyName is not null)
				set @v_QueryConditions = @v_QueryConditions +char(13)+char(10)+ '	and Part_CompanyName = ''' + @p_companyName + '''';
		end
		
		set @v_QueryBody = @v_QueryBody+@v_QueryConditions+@v_QueryEnd;
		exec sp_executesql @v_QueryBody;
	end

go

	create procedure inv.usp_PartnerGetById
		@p_PartnerId int
	as
	begin
		select
			Part_Id,
			Part_FirstName,
			Part_LastName,
			Part_CompanyName,
			Part_Vatin,
			Part_Address
		from inv.Partners with(nolock)
		where Part_Id = @p_PartnerId;
	end

go