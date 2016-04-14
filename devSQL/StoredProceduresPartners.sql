use InvoiceSystem

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
		and not exists (select top 1 1 from inv.Partners where Part_Vatin = @p_Vatin)
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
		Part_id,
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