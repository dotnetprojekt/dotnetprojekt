use InvoiceSystem;

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
		declare @v_MaxYear int = (
									select
										max(year(Inv_DateOfIssue))
									from inv.Invoices with(nolock)
								);
		begin transaction

			if( year(getutcdate()) > @v_MaxYear )
			begin
				update dbo.CurrentNumber
				set number = 1;
			end

			select @v_Number = number from dbo.CurrentNumber;

			update dbo.CurrentNumber
			set number = number + 1;
			
			set @p_InvoiceNumber = convert(nvarchar(16),(select 'INV-' + convert(nvarchar(4),year(getutcdate())) + '/' + right('000000'+convert(nvarchar(5),@v_Number),7)));

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

	create procedure inv.usp_InvoicesSearch
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
		@p_pageNumber int,
		@p_rowsPerPage int
	as
	begin
		set nocount on;
		set xact_abort on;

		declare @v_dynamicList table
		(
			v_number int
		);

		declare @v_VendorTab table
		(
			v_Part_Id int,
			v_Part_FirstName nvarchar(128),
			v_Part_LastName nvarchar(128),
			v_Part_CompanyName nvarchar(256),
			v_Part_Vatin decimal(24,0),
			v_Part_Address nvarchar(2048)
		);

		declare @v_BuyerTab table
		(
			v_Part_Id int,
			v_Part_FirstName nvarchar(128),
			v_Part_LastName nvarchar(128),
			v_Part_CompanyName nvarchar(256),
			v_Part_Vatin decimal(24,0),
			v_Part_Address nvarchar(2048)
		);

		insert into @v_VendorTab
		exec inv.usp_PartnerSearch
			@p_VendorFirstName,
			@p_VendorLastName,
			@p_VendorCompany,
			@p_VendorVatin;

		insert into @v_BuyerTab
		exec inv.usp_PartnerSearch
			@p_BuyerFirstName,
			@p_BuyerLastName,
			@p_BuyerCompany,
			@p_BuyerVatin;

			declare @v_QueryBody nvarchar(max) = 
'select
	Inv_Id,
	Inv_Number,
	Inv_DateOfIssue,
	(
		select
			isnull(Part_FirstName,'''') + '' '' + isnull(Part_LastName,'') + 
				case
					when Part_CompanyName is not null then '' ('' + Part_CompanyName + '')''
					else ''''
				end
			from Partners with(nolock)
			where Part_Id = Inv_VendorId
	),
	(
		select
			isnull(Part_FirstName,'''') + isnull(Part_LastName,'''') + 
				case
					when Part_CompanyName is not null then '' ('' + Part_CompanyName + '')''
					else ''''
				end
			from Partners with(nolock)
			where Part_Id = Inv_BuyerId
	)
	Inv_Title,
	Inv_OverallCost,
	Inv_Status
from inv.Invoices with(nolock)
where 1=1';

			declare @v_QueryConditions nvarchar(max) = '';

			declare @v_QueryEnd nvarchar(max) = 
' order by Inv_Id
offset ('+ convert(nvarchar(32),(@p_pageNumber-1)*@p_rowsPerPage) +') rows
fetch next ('+ convert(nvarchar(32),@p_rowsPerPage)+') rows only';

			if(@p_DateStart is not null)
				set @v_QueryConditions = @v_QueryConditions + CHAR(13)+CHAR(10)+ '	and Inv_DateOfIssue >= ''' + @p_DateStart + '''';

			if(@p_DateEnd is not null)
				set @v_QueryConditions = @v_QueryConditions + CHAR(13)+CHAR(10)+ '	and Inv_DateOfIssue <= ''' + @p_DateEnd + '''';

			if(@p_Title is not null)
				set @v_QueryConditions = @v_QueryConditions + CHAR(13)+CHAR(10)+ '	and Contains(Inv_Title,''' + @p_Title + ''')';

			if(@p_CostMin is not null)
				set @v_QueryConditions = @v_QueryConditions + CHAR(13)+CHAR(10)+ '	and Inv_OverallCost >= ''' + convert(nvarchar(16),@p_CostMin) + '''';

			if(@p_CostMax is not null)
				set @v_QueryConditions = @v_QueryConditions + CHAR(13)+CHAR(10)+ '	and Inv_OverallCost <= ''' + convert(nvarchar(16),@p_CostMax) + '''';

			select @v_QueryBody + @v_QueryConditions + @v_QueryEnd;

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

		declare @p_Date datetime2 = dateadd(year,-1,getutcdate());

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

		declare @p_Date datetime2 = dateadd(year,-1,getutcdate());

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

DECLARE @RC int
DECLARE @p_Number nvarchar(16) = null;
DECLARE @p_DateStart nvarchar(16) = '2016-10-10';
DECLARE @p_DateEnd nvarchar(16) = '2016-12-12';
DECLARE @p_VendorFirstName nvarchar(128) = null;
DECLARE @p_VendorLastName nvarchar(128) = null;
DECLARE @p_VendorCompany nvarchar(256) = null;
DECLARE @p_VendorVatin decimal(24,0) = null;
DECLARE @p_BuyerFirstName nvarchar(128) = null;
DECLARE @p_BuyerLastName nvarchar(128) = null;
DECLARE @p_BuyerCompany nvarchar(256) = null;
DECLARE @p_BuyerVatin decimal(24,0) = null;
DECLARE @p_Title nvarchar(2048) = 'TSW'
DECLARE @p_CostMin decimal(9,2) = 500;
DECLARE @p_CostMax decimal(9,2) = 1500;
DECLARE @p_pageNumber int = 1;
DECLARE @p_rowsPerPage int = 300;

-- TODO: Set parameter values here.

EXECUTE @RC = [inv].[usp_InvoicesSearch] 
   @p_Number
  ,@p_DateStart
  ,@p_DateEnd
  ,@p_VendorFirstName
  ,@p_VendorLastName
  ,@p_VendorCompany
  ,@p_VendorVatin
  ,@p_BuyerFirstName
  ,@p_BuyerLastName
  ,@p_BuyerCompany
  ,@p_BuyerVatin
  ,@p_Title
  ,@p_CostMin
  ,@p_CostMax
  ,@p_pageNumber
  ,@p_rowsPerPage
GO

