create procedure inv.NextInvoiceNumber
	@p_InvoiceNumber nvarchar(16) output
as
begin
	set nocount on;
	set xact_abort on;

	declare @v_Number int;
	declare @v_MaxYear int =(
								select
									isnull(max(year(Inv_DateOfIssue)),0)
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

		set @p_InvoiceNumber = convert(nvarchar(16),(select 'INV-' + convert(nvarchar(4),year(getutcdate())) + '/' + left('000000'+convert(nvarchar(5),@v_Number),7)));

	commit transaction

end

go

create procedure inv.InvoiceAdd
	@p_VendorId int,
	@p_BuyerId int,
	@p_Title nvarchar(2048),
	@p_Goods xml,
	@p_OverallNetValue decimal(9,2),
	@p_OverallGrossValue decimal(9,2),
	@p_Discount decimal(3,2),
	@p_OverallCost decimal(3,2),
	@p_Creator int
as
begin
	set nocount on;
	set xact_abort on;

	declare @v_InvoiceNumber nvarchar(16);

	if( @p_VendorId is null or @p_BuyerId is null or @p_Title is null or @p_OverallNetValue is null or @p_OverallGrossValue is null or @p_OverallCost is null or @p_Creator is null )
		raiserror (15600,-1,-1, 'inv.usp_UserAdd');
	else
	begin
		begin transaction
				
			exec inv.NextInvoiceNumber @v_InvoiceNumber output;

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
				Inv_OverallCost,
				Inv_Creator
			)
			values
			(
				@v_InvoiceNumber,
				@p_VendorId,
				@p_BuyerId,
				@p_Title,
				@p_Goods,
				@p_OverallNetValue,
				@p_OverallGrossValue,
				@p_Discount,
				@p_OverallCost,
				@p_Creator
			);
		commit transaction
	end
end

go

create procedure inv.usp_InvoicesList
	@p_pageNumber int,
	@p_rowsPerPage int,
	@p_showArchived bit = 0
as
begin
	set nocount on;
	set xact_abort on;

	if(@p_showArchived = 0)
		select
			Inv_Id,
			Inv_Number,
			Inv_DateOfIssue,
			(
				select
					isnull(Part_FirstName,'') + isnull(Part_LastName,'') + 
						case
							when Part_CompanyName is not null then ' (' + Part_CompanyName + ')'
							else ''
						end
				 from Partners with(nolock)
				 where Part_Id = Inv_VendorId
			),
			(
				select
					isnull(Part_FirstName,'') + isnull(Part_LastName,'') + 
						case
							when Part_CompanyName is not null then ' (' + Part_CompanyName + ')'
							else ''
						end
				 from Partners with(nolock)
				 where Part_Id = Inv_BuyerId
			)
			Inv_Title,
			Inv_OverallCost,
			Inv_Status
		from inv.Invoices with(nolock)
		where Inv_Status in (1,2)
		order by Inv_Id
		offset ((@p_pageNumber-1)*@p_rowsPerPage) rows
		fetch next (@p_rowsPerPage) rows only
	else
		select
			Inv_Id,
			Inv_Number,
			Inv_DateOfIssue,
			(
				select
					isnull(Part_FirstName,'') + isnull(Part_LastName,'') + 
						case
							when Part_CompanyName is not null then ' (' + Part_CompanyName + ')'
							else ''
						end
				 from Partners with(nolock)
				 where Part_Id = Inv_VendorId
			),
			(
				select
					isnull(Part_FirstName,'') + isnull(Part_LastName,'') + 
						case
							when Part_CompanyName is not null then ' (' + Part_CompanyName + ')'
							else ''
						end
				 from Partners with(nolock)
				 where Part_Id = Inv_BuyerId
			)
			Inv_Title,
			Inv_OverallCost,
			Inv_Status
		from inv.Invoices with(nolock)
		where Inv_Status = 3
		order by Inv_Id
		offset ((@p_pageNumber-1)*@p_rowsPerPage) rows
		fetch next (@p_rowsPerPage) rows only
end

go

create procedure inv.InvoicesDetails
	@p_Inv_Id int
as
begin
	select
		Inv_Id,
		Inv_Number,
		Inv_DateOfIssue,
		Inv_VendorId,
		(select Part_FirstName from Partners with(nolock) where Part_Id = Inv_VendorId) as Inv_VendorFirstName,
		(select Part_LastName from Partners with(nolock) where Part_Id = Inv_VendorId) as Inv_VendorLastName,
		(select Part_CompanyName from Partners with(nolock) where Part_Id = Inv_VendorId) as Inv_VendorCompanyName,
		(select Part_Vatin from Partners with(nolock) where Part_Id = Inv_VendorId) as Inv_VendorVatin,
		Inv_BuyerId,
		(select Part_FirstName from Partners with(nolock) where Part_Id = Inv_BuyerId) as Inv_BuyerFirstName,
		(select Part_LastName from Partners with(nolock) where Part_Id = Inv_BuyerId) as Inv_BuyerLastName,
		(select Part_CompanyName from Partners with(nolock) where Part_Id = Inv_BuyerId) as Inv_BuyerCompanyName,
		(select Part_Vatin from Partners with(nolock) where Part_Id = Inv_BuyerId) as Inv_BuyerVatin,
		Inv_Title,
		Inv_Goods,
		Inv_OverallNetValue,
		Inv_OverallGrossValue,
		Inv_Discount,
		Inv_OverallCost,
		Inv_Status,
		Inv_Creator,
		(select Usr_FirstName from inv.Users with(nolock) where Usr_id = Inv_Creator) as Inv_CreatorFirstName,
		(select Usr_LastName from inv.Users with(nolock) where Usr_Id = Inv_Creator) as Inv_CreatorLastName
	from inv.Invoices with(nolock)
	where Inv_Id = @p_Inv_Id;
end

go

create procedure inv.InvoicesSetPaid
	@p_Inv_Id int
as
begin
	update inv.Invoices with(rowlock)
	set Inv_Status = 2
	where Inv_Id = @p_Inv_Id
		and Inv_Status = 1;
end

go

create procedure inv.InvoicesArchive
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

create procedure inv.InvoicesDelete
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