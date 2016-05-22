exec tSQLt.NewTestClass 'StoredProceduresBussinessPartnersTest';

go
create procedure StoredProceduresBussinessPartnersTest.[testPartnerAdd] 
as
begin
	exec tsqlt.FakeTable 'inv.Partners'
	declare @retCount int

	exec inv.usp_PartnersAdd 'FirstName1', 'LastName1', 'CompanyName1', 100000000000000000000000, 'Address1', 1;
	exec inv.usp_PartnersAdd 'FirstName2', 'LastName2', 'CompanyName2', 200000000000000000000000, 'Address2', 2;

	select @retCount = count(*) from inv.Partners;
	exec tSQLt.AssertEquals 2, @retCount;
end;

go
create procedure StoredProceduresBussinessPartnersTest.[testPartnerAddNamesNull] 
as
begin
	exec tsqlt.FakeTable 'inv.Partners'
	declare @retCount int

	exec inv.usp_PartnersAdd null, null, 'CompanyName1', 100000000000000000000000, 'Address1', 1;

	select @retCount = count(*) from inv.Partners;
	exec tSQLt.AssertEquals 1, @retCount;
end;

go
create procedure StoredProceduresBussinessPartnersTest.[testPartnerAddCompanyNull] 
as
begin
	exec tsqlt.FakeTable 'inv.Partners'
	declare @retCount int

	exec inv.usp_PartnersAdd 'FirstName1', 'LastName1', null, 100000000000000000000000, 'Address1', 1;

	select @retCount = count(*) from inv.Partners;
	exec tSQLt.AssertEquals 1, @retCount;
end;

go
create procedure StoredProceduresBussinessPartnersTest.[testPartnerAddErrorVatinExist] 
as
begin
	exec tsqlt.FakeTable 'inv.Partners'

	exec tSQLt.ExpectException;
	exec inv.usp_PartnersAdd 'FirstName1', 'LastName1', 'CompanyName1', 100000000000000000000000, 'Address1', 1;
	exec inv.usp_PartnersAdd 'FirstName2', 'LastName2', 'CompanyName2', 100000000000000000000000, 'Address2', 2;

end;

go
create procedure StoredProceduresBussinessPartnersTest.[testPartnerAddErrorAllExpectedNull] 
as
begin
	exec tsqlt.FakeTable 'inv.Partners'

	exec tSQLt.ExpectException;
	exec inv.usp_PartnersAdd null, null, null, 100000000000000000000000, 'Address1', 1;

end;

go
create procedure StoredProceduresBussinessPartnersTest.[testPartnerAddErrorUserNull] 
as
begin
	exec tsqlt.FakeTable 'inv.Partners'

	exec tSQLt.ExpectException;
	exec inv.usp_PartnersAdd 'FirstName1', 'LastName1', 'CompanyName1', 100000000000000000000000, 'Address1', null;	
end;

go
create procedure StoredProceduresBussinessPartnersTest.[testPartnerList] 
as
begin
	exec tsqlt.FakeTable 'inv.Partners';
	declare @retCount int;

	declare @Partners table (
		Part_id int,
		Part_FirstName nvarchar(128),
		Part_LastName nvarchar(128),
		Part_CompanyName nvarchar(128),
		Part_Vatin decimal(24,0),
		Part_Address nvarchar(2048)
	);

	exec inv.usp_PartnersAdd 'FirstName1', 'LastName1', 'CompanyName1', 100000000000000000000000, 'Address1', 1;
	exec inv.usp_PartnersAdd 'FirstName2', 'LastName2', 'CompanyName2', 200000000000000000000000, 'Address2', 1;
	exec inv.usp_PartnersAdd 'FirstName3', 'LastName3', 'CompanyName3', 300000000000000000000000, 'Address3', 1;
	exec inv.usp_PartnersAdd 'FirstName4', 'LastName4', 'CompanyName4', 400000000000000000000000, 'Address4', 1;	

	insert into @Partners exec inv.usp_PartnersList 1,30;
	select @retCount=count(*) from @Partners;

	exec tSQLt.AssertEquals 4, @retCount;
end;

go
create procedure StoredProceduresBussinessPartnersTest.[testPartnerListPaging] 
as
begin
	exec tsqlt.FakeTable 'inv.Partners';
	declare @retCount int;

	declare @Partners table (
		Part_id int,
		Part_FirstName nvarchar(128),
		Part_LastName nvarchar(128),
		Part_CompanyName nvarchar(128),
		Part_Vatin decimal(24,0),
		Part_Address nvarchar(2048)
	);

	exec inv.usp_PartnersAdd 'FirstName1', 'LastName1', 'CompanyName1', 100000000000000000000000, 'Address1', 1;
	exec inv.usp_PartnersAdd 'FirstName2', 'LastName2', 'CompanyName2', 200000000000000000000000, 'Address2', 1;
	exec inv.usp_PartnersAdd 'FirstName3', 'LastName3', 'CompanyName3', 300000000000000000000000, 'Address3', 1;
	exec inv.usp_PartnersAdd 'FirstName4', 'LastName4', 'CompanyName4', 400000000000000000000000, 'Address4', 1;	

	insert into @Partners exec inv.usp_PartnersList 2,2;
	select @retCount=count(*) from @Partners;

	exec tSQLt.AssertEquals 2, @retCount;
end;

go
exec tSQLt.RunAll