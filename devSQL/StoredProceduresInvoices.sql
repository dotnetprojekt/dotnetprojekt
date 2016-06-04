use InvoiceSystem;

go

	if(type_id('PartnerTable') is not null)
		drop type PartnerTable;

	create type PartnerTable as table
	(
		v_Part_Id int,
		v_Part_FirstName nvarchar(128),
		v_Part_LastName nvarchar(128),
		v_Part_CompanyName nvarchar(256),
		v_Part_Vatin decimal(24,0),
		v_Part_Address nvarchar(2048)
	);

go

	if(object_id('inv.usp_InvoiceNumberNext') is not null)
		drop procedure inv.usp_InvoiceNumberNext;

	if(object_id('inv.usp_InvoiceAdd') is not null)
		drop procedure inv.usp_InvoiceAdd;

	if(object_id('inv.usp_InvoicesSearch') is not null)
		drop procedure inv.usp_InvoicesSearch;

	if(object_id('inv.usp_InvoicesDetailsGet') is not null)
		drop procedure inv.usp_InvoicesDetailsGet;

	if(object_id('inv.usp_InvoicesSetPaid') is not null)
		drop procedure inv.usp_InvoicesSetPaid;

	if(object_id('inv.usp_InvoicesArchive') is not null)
		drop procedure inv.usp_InvoicesArchive;

	if(object_id('inv.usp_InvoicesDelete') is not null)
		drop procedure inv.usp_InvoicesDelete;

go

	create procedure inv.usp_InvoiceNumberNext --InvoiceDAL.GetInvoiceNumber()
		@p_InvoiceNumber nvarchar(16) output
	as
	begin
		set nocount on;
		set xact_abort on;

		declare @v_Number int;
		declare @v_MaxYear int;


		begin transaction

		select @v_Number = number, @v_MaxYear = yearLast from dbo.CurrentNumber;

			if( year(getutcdate()) > @v_MaxYear )
			begin
				update dbo.CurrentNumber
				set number = 2,
					yearLast = year(getutcdate());

				set @v_Number = 1;
			end
			else
				update dbo.CurrentNumber
				set number = number + 1;
			
			set @p_InvoiceNumber = convert(nvarchar(16),(select 'INV-' + convert(nvarchar(4),year(getutcdate())) + '/' + right('000000'+convert(nvarchar(6),@v_Number),7)));

		commit transaction

	end

go

	create procedure inv.usp_InvoiceAdd -- InvoiceDAL.InvoiceAdd()
		@p_InvoiceNumber nvarchar(16),
		@p_VendorVatin decimal(24,0),
		@p_BuyerVatin decimal(24,0),
		@p_Title nvarchar(2048),
		@p_Goods nvarchar(max),
		@p_OverallNetValue decimal(9,2),
		@p_OverallGrossValue decimal(9,2),
		@p_Discount decimal(3,2),
		@p_OverallCost decimal(9,2)
	as
	begin
		set nocount on;
		set xact_abort on;

		if( @p_VendorVatin is null or @p_BuyerVatin is null or @p_Title is null or @p_OverallNetValue is null or @p_OverallGrossValue is null or @p_OverallCost is null )
			raiserror (15600,-1,-1, 'inv.usp_UserAdd');
		else
		begin
			begin transaction
				
				declare @v_VendorId int = (select Part_Id from inv.Partners where Part_Vatin = @p_VendorVatin);
				declare @v_BuyerId int = (select Part_Id from inv.Partners where Part_Vatin = @p_BuyerVatin);

				insert into inv.Invoices
				(
					Inv_Number,
					Inv_VendorId,
					Inv_BuyerId,
					Inv_Title,
					Inv_Goods,
					Inv_OverallNetValue,
					Inv_OverallGrossValue,
					Inv_Discount,
					Inv_OverallCost
				)
				values
				(
					@p_InvoiceNumber,
					@v_VendorId,
					@v_BuyerId,
					@p_Title,
					@p_Goods,
					@p_OverallNetValue,
					@p_OverallGrossValue,
					@p_Discount,
					@p_OverallCost
				);
			commit transaction
		end
	end

go

	create procedure inv.usp_InvoicesDetailsGet -- InvoiceDAL.InvoiceGetDetails()
		@p_Inv_Id int
	as
	begin
		select
			Inv_Id,
			Inv_Number,
			Inv_DateOfIssue,
			Inv_VendorId,
			Inv_BuyerId,
			Inv_Title,
			Inv_Goods,
			Inv_OverallNetValue,
			Inv_OverallGrossValue,
			Inv_Discount,
			Inv_OverallCost,
			Inv_Status
		from inv.Invoices with(nolock)
		where Inv_Id = @p_Inv_Id;
	end

go

	create procedure inv.usp_InvoicesSetPaid -- InvoiceDAL.InvoiceSetPaid()
		@p_Inv_Id int
	as
	begin
		update inv.Invoices with(rowlock)
		set Inv_Status = 2
		where Inv_Id = @p_Inv_Id
			and Inv_Status = 1;
	end

go

	create procedure inv.usp_InvoicesArchive -- Job
	as
	begin
		set nocount on;

		declare @p_Date datetime2 = dateadd(month,-6,getutcdate());

		while(1=1)
		begin
			update top (1000) inv.Invoices with(rowlock)
			set Inv_Status = 3
			where Inv_Status != 3
				and Inv_DateOfIssue < @p_Date

			if(@@rowcount=0)
				break;

			waitfor delay '00:00:02';
		end
	end

go

	create procedure inv.usp_InvoicesDelete -- Job
	as
	begin
		set nocount on;

		declare @p_Date datetime2 = dateadd(year,-6,getutcdate());

		while(1=1)
		begin
			delete top (1000)
			from inv.Invoices
			where Inv_Status = 3
				and Inv_DateOfIssue < @p_Date

			if(@@rowcount=0)
				break;

			waitfor delay '00:00:02';
		end
	end

go

	create procedure inv.usp_InvoicesSearch -- InvoiceDAL.InvoiceSearch()
		@p_Number nvarchar(16),
		@p_DateStart nvarchar(16),
		@p_DateEnd nvarchar(16),
		@p_VendorFirstName nvarchar(128),
		@p_VendorLastName nvarchar(128),
		@p_VendorCompany nvarchar(256),
		@p_VendorVatin decimal(24,0),
		@p_BuyerFirstName nvarchar(128),
		@p_BuyerLastName nvarchar(128),
		@p_BuyerCompany nvarchar(256),
		@p_BuyerVatin decimal(24,0),
		@p_Title nvarchar(2048),
		@p_CostMin decimal(9,2),
		@p_CostMax decimal(9,2),
		@p_StatusFilter nvarchar(3),
		@p_pageNumber int,
		@p_rowsOffset int,
		@p_rowsPerPage int
	as
	begin
		set nocount on;
		set xact_abort on;

		declare @v_dynamicList table
		(
			v_number int
		);

		declare @v_InvoicesTmp table
		(
			v_Inv_Id int,
			v_Inv_Number nvarchar(16),
			v_Inv_DateOfIssue datetime2(7),
			v_Inv_VendorId int,
			v_Inv_BuyerId int,
			v_Inv_Title nvarchar(2048),
			v_Inv_OverallCost decimal(9,2),
			v_Inv_Status int
		);

		declare @v_VendorSearch bit = case when	(
													@p_VendorFirstName is not null
													or @p_VendorLastName is not null
													or @p_VendorCompany is not null
													or @p_VendorVatin is not null
												) then 1
											else 0 end;

		declare @v_BuyerSearch bit = case when	(
													@p_BuyerFirstName is not null
													or @p_BuyerLastName is not null
													or @p_BuyerCompany is not null
													or @p_BuyerVatin is not null
												) then 1
											else 0 end;

		if( @v_VendorSearch = 1)
		begin
			declare @v_VendorTab PartnerTable;

			insert into @v_VendorTab
			exec inv.usp_PartnerSearch
				@p_VendorFirstName,
				@p_VendorLastName,
				@p_VendorCompany,
				@p_VendorVatin;
		end

		if( @v_BuyerSearch = 1 )
		begin
			declare @v_BuyerTab PartnerTable;

			insert into @v_BuyerTab
			exec inv.usp_PartnerSearch
				@p_BuyerFirstName,
				@p_BuyerLastName,
				@p_BuyerCompany,
				@p_BuyerVatin;
		end

			declare @v_QueryBody nvarchar(max) = 
'select
	Inv_Id,
	Inv_Number,
	Inv_DateOfIssue,
	Inv_VendorId,
	Inv_BuyerId,
	Inv_Title,
	Inv_OverallCost,
	Inv_Status
from inv.Invoices with(nolock)
where 1=1';

			declare @v_QueryConditions nvarchar(max) = '';

			if(@p_Number is not null)
				set @v_QueryConditions = @v_QueryConditions +char(13)+char(10)+ '	and Inv_Number like ''%'+ @p_Number + '%'''; --fulltextsearch?

			if(@p_DateStart is not null)
				set @v_QueryConditions = @v_QueryConditions +char(13)+char(10)+ '	and Inv_DateOfIssue >= ''' + @p_DateStart + '''';

			if(@p_DateEnd is not null)
				set @v_QueryConditions = @v_QueryConditions +char(13)+char(10)+ '	and Inv_DateOfIssue <= ''' + @p_DateEnd + '''';

			if(@p_Title is not null)
				set @v_QueryConditions = @v_QueryConditions +char(13)+char(10)+ '	and Inv_Title like ''%'+ @p_Title + '%'''; --fulltextsearch?

			if(@p_CostMin is not null)
				set @v_QueryConditions = @v_QueryConditions +char(13)+char(10)+ '	and Inv_OverallCost >= ' + convert(nvarchar(16),@p_CostMin);

			if(@p_CostMax is not null)
				set @v_QueryConditions = @v_QueryConditions +char(13)+char(10)+ '	and Inv_OverallCost <= ' + convert(nvarchar(16),@p_CostMax);

			if( @p_StatusFilter is not null and @p_StatusFilter != '111' )
			begin
				declare @tmp nvarchar(128) = '(0';

				if( substring(@p_StatusFilter,1,1) = '1' )
					set @tmp = @tmp + ',1';
					--set @v_QueryConditions = @v_QueryConditions + char(13)+char(10)+ '	and Inv_Status = 1';

				if( substring(@p_StatusFilter,2,1) = '1')
					set @tmp = @tmp + ',2';
					--set @v_QueryConditions = @v_QueryConditions + char(13)+char(10)+ '	or Inv_Status = 2';

				if( SUBSTRING(@p_StatusFilter,3,1) = '1')
					set @tmp = @tmp + ',3';
					--set @v_QueryConditions = @v_QueryConditions + char(13)+char(10)+ '	or Inv_Status = 3';

				set @tmp = @tmp + ')';
				set @v_QueryConditions = @v_QueryConditions + char(13)+char(10)+ '	and Inv_Status in ' + @tmp;
			end

			set @v_QueryBody = @v_QueryBody + @v_QueryConditions;

			insert into @v_InvoicesTmp
			exec sp_executesql @v_QueryBody;
		
			select
				v_Inv_Id,
				v_Inv_Number,
				v_Inv_DateOfIssue,
				isnull(v.Part_FirstName,'') + ' ' + isnull(v.Part_LastName,'') + ' (' + isnull(v.Part_CompanyName,'') + ')' as v_Inv_Vendor,
				isnull(b.Part_FirstName,'') + ' ' + isnull(b.Part_LastName,'') + ' (' + isnull(b.Part_CompanyName,'') + ')' as v_Inv_Buyer,
				v_Inv_Title,
				v_Inv_OverallCost,
				v_Inv_Status
			from @v_InvoicesTmp
				inner join inv.Partners v
					on v_Inv_VendorId = v.Part_Id
				inner join inv.Partners b
					on v_Inv_BuyerId = b.Part_Id
			where (exists (select top 1 1 from @v_VendorTab vv where v_Part_Id = v_Inv_VendorId) or @v_VendorSearch = 0)
				and (exists (select top 1 1 from @v_BuyerTab where v_Part_Id = v_Inv_BuyerId) or @v_BuyerSearch = 0)
			order by v_Inv_Id
			offset ((@p_pageNumber-1)*@p_rowsOffset) rows
			fetch next (@p_rowsPerPage) rows only;

	end

go