exec tSQLt.NewTestClass 'StoredProceduresInvoicesTest';

go
create procedure StoredProceduresInvoicesTest.[testInvoiceAdd] 
as
begin
	exec tsqlt.FakeTable @TableName = 'inv.Invoices', @Identity = 1, @Defaults = 1;
	declare @invoiceNum nvarchar(16);
	declare @retCount int;

	exec inv.InvoiceAdd 1, 2, 'Title', '<goods>Product</goods>', 1000.0, 1230.0, 0.0, 1230.0, 1;
	
	select @invoiceNum=Inv_Number from inv.Invoices where Inv_Title = 'Title';
	select @retCount=count(*) from inv.Invoices;

	exec tSQLt.AssertNotEquals @invoiceNum, null;	
	exec tSQLt.AssertEquals 1, @retCount;
end;


go
create procedure StoredProceduresInvoicesTest.[testInvoiceErrorAdd] 
as
begin
	exec tsqlt.FakeTable @TableName = 'inv.Invoices', @Identity = 1, @Defaults = 1;

	exec tSQLt.ExpectException;
	exec inv.InvoiceAdd null, 2, 'Title', '<goods>Product</goods>', 1000.0, 1230.0, 0.0, 1230.0, 1;

end;

go
create procedure StoredProceduresInvoicesTest.[testNextInvoiceNumber] 
as
begin
	exec tsqlt.FakeTable 'inv.Invoices';
	declare @v_InvoiceNumber nvarchar(16);

	exec inv.NextInvoiceNumber @v_InvoiceNumber output;
	exec tSQLt.AssertNotEquals @v_InvoiceNumber, null;
end;

go
create procedure StoredProceduresInvoicesTest.[testInvoicesNonArchivedList] 
as
begin
	exec tsqlt.FakeTable @TableName = 'inv.Invoices', @Identity = 1, @Defaults = 1;
	
	declare @Invoices table (
		Inv_Id int,
		Inv_Number nvarchar(16),
		Inv_DateOfIssue datetime2,
		Inv_Partners nvarchar(128),
		Inv_Title nvarchar(2048),
		Inv_OverallCost decimal(9,2),
		Inv_Status int
	);

	declare @retCount int;

	exec inv.InvoiceAdd 1, 2, 'Title1', '<goods>Product1</goods>', 1000.0, 1230.0, 0.0, 1230.0, 1;
	exec inv.InvoiceAdd 2, 3, 'Title2', '<goods>Product2</goods>', 1000.0, 1230.0, 0.0, 1230.0, 1;
	exec inv.InvoiceAdd 3, 4, 'Title3', '<goods>Product3</goods>', 1000.0, 1230.0, 0.0, 1230.0, 1;
	exec inv.InvoiceAdd 5, 6, 'Title4', '<goods>Product4</goods>', 1000.0, 1230.0, 0.0, 1230.0, 1;
	exec inv.InvoiceAdd 4, 5, 'Title5', '<goods>Product5</goods>', 1000.0, 1230.0, 0.0, 1230.0, 1;

	insert into @Invoices(Inv_Id,Inv_Number,Inv_DateOfIssue,Inv_Partners,Inv_Title,Inv_OverallCost,Inv_Status) exec inv.usp_InvoicesList 1, 30, 0;
	select @retCount=count(*) from @Invoices;

	exec tSQLt.AssertEquals 5, @retCount;
end;

go
create procedure StoredProceduresInvoicesTest.[testInvoicesNonArchivedListWithArchivedInvoices] 
as
begin
	exec tsqlt.FakeTable @TableName = 'inv.Invoices', @Identity = 1, @Defaults = 1;
	
	declare @Invoices table (
		Inv_Id int,
		Inv_Number nvarchar(16),
		Inv_DateOfIssue datetime2,
		Inv_Partners nvarchar(128),
		Inv_Title nvarchar(2048),
		Inv_OverallCost decimal(9,2),
		Inv_Status int
	);

	declare @retCount int;

	exec inv.InvoiceAdd 1, 2, 'Title1', '<goods>Product1</goods>', 1000.0, 1230.0, 0.0, 1230.0, 1;
	exec inv.InvoiceAdd 2, 3, 'Title2', '<goods>Product2</goods>', 1000.0, 1230.0, 0.0, 1230.0, 1;
	exec inv.InvoiceAdd 3, 4, 'Title3', '<goods>Product3</goods>', 1000.0, 1230.0, 0.0, 1230.0, 1;
	update inv.Invoices set Inv_Status = 3;
	exec inv.InvoicesArchive;
	exec inv.InvoiceAdd 5, 6, 'Title4', '<goods>Product4</goods>', 1000.0, 1230.0, 0.0, 1230.0, 1;
	exec inv.InvoiceAdd 4, 5, 'Title5', '<goods>Product5</goods>', 1000.0, 1230.0, 0.0, 1230.0, 1;

	insert into @Invoices(Inv_Id,Inv_Number,Inv_DateOfIssue,Inv_Partners,Inv_Title,Inv_OverallCost,Inv_Status) exec inv.usp_InvoicesList 1, 30, 0;
	select @retCount=count(*) from @Invoices;
	exec tSQLt.AssertEquals 2, @retCount;

end;

go
create procedure StoredProceduresInvoicesTest.[testInvoicesArchivedList] 
as
begin
	exec tsqlt.FakeTable @TableName = 'inv.Invoices', @Identity = 1, @Defaults = 1;
	
	declare @Invoices table (
		Inv_Id int,
		Inv_Number nvarchar(16),
		Inv_DateOfIssue datetime2,
		Inv_Partners nvarchar(128),
		Inv_Title nvarchar(2048),
		Inv_OverallCost decimal(9,2),
		Inv_Status int
	);

	declare @retCount int;

	exec inv.InvoiceAdd 1, 2, 'Title1', '<goods>Product1</goods>', 1000.0, 1230.0, 0.0, 1230.0, 1;
	exec inv.InvoiceAdd 2, 3, 'Title2', '<goods>Product2</goods>', 1000.0, 1230.0, 0.0, 1230.0, 1;
	exec inv.InvoiceAdd 3, 4, 'Title3', '<goods>Product3</goods>', 1000.0, 1230.0, 0.0, 1230.0, 1;
	update inv.Invoices set Inv_Status = 3;
	exec inv.InvoicesArchive;
	exec inv.InvoiceAdd 5, 6, 'Title4', '<goods>Product4</goods>', 1000.0, 1230.0, 0.0, 1230.0, 1;
	exec inv.InvoiceAdd 4, 5, 'Title5', '<goods>Product5</goods>', 1000.0, 1230.0, 0.0, 1230.0, 1;

	insert into @Invoices(Inv_Id,Inv_Number,Inv_DateOfIssue,Inv_Partners,Inv_Title,Inv_OverallCost,Inv_Status) exec inv.usp_InvoicesList 1, 30, 1;
	select @retCount=count(*) from @Invoices;
	exec tSQLt.AssertEquals 3, @retCount;

end;

go
create procedure StoredProceduresInvoicesTest.[testInvoicesArchive] 
as
begin
	exec tsqlt.FakeTable @TableName = 'inv.Invoices', @Identity = 1, @Defaults = 1;

	declare @retCount int;
	declare @Invoices table (
		Inv_Id int,
		Inv_Number nvarchar(16),
		Inv_DateOfIssue datetime2,
		Inv_Partners nvarchar(128),
		Inv_Title nvarchar(2048),
		Inv_OverallCost decimal(9,2),
		Inv_Status int
	);

	exec inv.InvoiceAdd 1, 2, 'Title1', '<goods>Product1</goods>', 1000.0, 1230.0, 0.0, 1230.0, 1;
	exec inv.InvoiceAdd 2, 3, 'Title2', '<goods>Product2</goods>', 1000.0, 1230.0, 0.0, 1230.0, 1;
	exec inv.InvoiceAdd 3, 4, 'Title3', '<goods>Product3</goods>', 1000.0, 1230.0, 0.0, 1230.0, 1;
	update inv.Invoices set Inv_DateOfIssue = dateadd(year,-2,getutcdate());
	exec inv.InvoicesArchive;

	insert into @Invoices(Inv_Id,Inv_Number,Inv_DateOfIssue,Inv_Partners,Inv_Title,Inv_OverallCost,Inv_Status) exec inv.usp_InvoicesList 1, 30, 1;
	select @retCount=count(*) from @Invoices;
	exec tSQLt.AssertEquals 3, @retCount;

end;

go
create procedure StoredProceduresInvoicesTest.[testInvoicesDelete] 
as
begin
	exec tsqlt.FakeTable @TableName = 'inv.Invoices', @Identity = 1, @Defaults = 1;

	declare @retCount int;
	declare @Invoices table (
		Inv_Id int,
		Inv_Number nvarchar(16),
		Inv_DateOfIssue datetime2,
		Inv_Partners nvarchar(128),
		Inv_Title nvarchar(2048),
		Inv_OverallCost decimal(9,2),
		Inv_Status int
	);

	exec inv.InvoiceAdd 1, 2, 'Title1', '<goods>Product1</goods>', 1000.0, 1230.0, 0.0, 1230.0, 1;
	exec inv.InvoiceAdd 2, 3, 'Title2', '<goods>Product2</goods>', 1000.0, 1230.0, 0.0, 1230.0, 1;
	exec inv.InvoiceAdd 3, 4, 'Title3', '<goods>Product3</goods>', 1000.0, 1230.0, 0.0, 1230.0, 1;
	update inv.Invoices set Inv_DateOfIssue = dateadd(year,-2,getutcdate());
	update inv.Invoices set Inv_Status = 3;
	exec inv.InvoicesDelete;
	exec inv.InvoiceAdd 4, 5, 'Title4', '<goods>Product4</goods>', 1000.0, 1230.0, 0.0, 1230.0, 1;

	insert into @Invoices(Inv_Id,Inv_Number,Inv_DateOfIssue,Inv_Partners,Inv_Title,Inv_OverallCost,Inv_Status) exec inv.usp_InvoicesList 1, 30, 0;
	select @retCount=count(*) from @Invoices;
	exec tSQLt.AssertEquals 1, @retCount;

end;

go
create procedure StoredProceduresInvoicesTest.[testInvoicesDetails] 
as
begin
	exec tsqlt.FakeTable @TableName = 'inv.Invoices', @Identity = 1, @Defaults = 1;
	exec tsqlt.FakeTable @TableName = 'inv.Partners', @Identity = 1, @Defaults = 1;
	exec tsqlt.FakeTable @TableName = 'inv.Users', @Identity = 1, @Defaults = 1;
	
	declare @InvoicesActual table (
		Inv_Id int,
		Inv_Number nvarchar(16),
		Inv_DateOfIssue datetime2,
		Inv_VendorId int,
		Inv_VendorFirstName nvarchar(128),
		Inv_VendorLastName nvarchar(128),
		Inv_VendorCompanyName nvarchar(128),
		Inv_VendorVatin decimal(24,0),
		Inv_BuyerId int,
		Inv_BuyerFirstName nvarchar(128),
		Inv_BuyerLastName nvarchar(128),
		Inv_BuyerCompanyName nvarchar(128),
		Inv_BuyerVatin decimal(24,0),
		Inv_Title nvarchar(2048),
		Inv_Goods xml,
		Inv_OverallNetValue decimal(9,2),
		Inv_OverallGrossValue decimal(9,2),
		Inv_Discount decimal(3,2),
		Inv_OverallCost decimal(9,2),
		Inv_Status int,
		Inv_Creator int,
		Inv_CreatorFirstName nvarchar(128),
		Inv_CreatorLastName nvarchar(128)
	);

	declare @retCount int;
	declare @vendor nvarchar(128);
	declare @buyer nvarchar(128);
	declare @creator nvarchar(128);

	exec inv.usp_UsersAdd 'CreatorName','CreatorNameLastName', 'Login', 'Password','test.mail@example.com',0,2
	exec inv.usp_PartnersAdd 'VendorName', 'VendorLastName', 'VendorCompanyName', 100000000000000000000000, 'VendorAddres', 1;
	exec inv.usp_PartnersAdd 'BuyerName', 'BuyerLastName', 'BuyerCompanyName', 200000000000000000000000, 'BuyerAddres', 2;

	exec inv.InvoiceAdd 1, 2, 'Title1', '<goods>Product1</goods>', 1000.0, 1230.0, 0.0, 1230.0, 1;

	insert into @InvoicesActual exec inv.InvoicesDetails 1;

	select @retCount=count(*) from @InvoicesActual;
	
	select @vendor=Inv_VendorFirstName from @InvoicesActual;
	select @buyer=Inv_BuyerFirstName from @InvoicesActual;
	select @creator=Inv_CreatorFirstName from @InvoicesActual;

	exec tSQLt.AssertEquals 1, @retCount;
	exec tSQLt.AssertEquals 'VendorName', @vendor;
	exec tSQLt.AssertEquals 'BuyerName', @buyer;
	exec tSQLt.AssertEquals 'CreatorName', @creator;

end;

go
exec tSQLt.RunAll