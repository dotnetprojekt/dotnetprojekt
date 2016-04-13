use InvoiceSystem;

go

create schema inv;

go

create table inv.Logs
(
	Log_Id int identity(1,1) not null,
	Log_Date datetime2 constraint DF_Date default getutcdate(),
	Log_Context nvarchar(1024) not null,
	Log_Status nvarchar(10) not null,
	Log_Description nvarchar(4000),

	constraint PK_logs primary key (Log_Id)
);

go

create table inv.Invoices
(
	Inv_Id int identity(1,1) not null,
	Inv_Number uniqueidentifier not null constraint DF_Number default newid(),
	Inv_DateOfIssue datetime2 not null constraint DF_DateOfIssue default getutcdate(),
	Inv_VendorId int not null,
	Inv_BuyerId int not null,
	Inv_Title nvarchar(2048) not null,
	Inv_Goods xml,
	Inv_OverallNetValue decimal(9,2) not null,
	Inv_OverallGrossValue decimal(9,2) not null,
	Inv_Discount decimal(3,2) constraint DF_Discount default null,
	Inv_OverallCost decimal(9,2) not null,
	Inv_Status int not null constraint DF_Status default 0,
	Inv_Creator int not null,
	
	constraint PK_Invoices primary key (Inv_Id),
	constraint CK_Status check ((Inv_Status=0) or (Inv_Status=1) or (Inv_Status=2)),
);

go

create table inv.Partners
(
	Part_Id int identity(1,1) not null,
	Part_FirstName nvarchar(128) null,
	Part_LastName nvarchar(128) null,
	Part_CompanyName nvarchar(256) null,
	Part_Vatin decimal(24,0) not null,
	Part_Address nvarchar(2048) not null,

	constraint PK_Partners primary key (Part_Id),
	constraint CK_Names check	(
									(Part_FirstName is not null and Part_LastName is not null and Part_CompanyName is not null)
									or (Part_FirstName is not null and Part_LastName is not null and Part_CompanyName is null)
									or (Part_FirstName is null and Part_LastName is null and Part_CompanyName is not null)
								),
	constraint UQ_Vatin unique (Part_Vatin)
);

go

create table inv.Users
(
	Usr_Id int identity(1,1) not null,
	Usr_FirstName nvarchar(128) not null,
	Usr_LastName nvarchar(128) not null,
	Usr_Login nvarchar(32) not null,
	Usr_PasswordHash nvarchar(128) not null,
	Usr_Email nvarchar(128),
	Usr_IsAdmin bit not null constraint DF_IsAdmin default 0,
	Usr_IsActive bit not null constraint DF_IsActive default 1,

	constraint PK_Users primary key (Usr_Id),
	constraint UQ_Login unique (Usr_Login)
);

go

alter table inv.Invoices
add constraint FK_Vendor
foreign key (Inv_VendorId) references inv.Partners(Part_Id);

alter table inv.Invoices
add constraint FK_Buyer
foreign key (Inv_BuyerId) references inv.Partners(Part_Id);

alter table inv.Invoices
add constraint FK_Creator
foreign key (Inv_Creator) references inv.Users(Usr_Id);