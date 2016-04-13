use InvoiceSystem

go

create procedure inv.usp_UserAdd
	@p_FirstName nvarchar(128),
	@p_LastName nvarchar(128),
	@p_Login nvarchar(32),
	@p_PasswordHash nvarchar(128),
	@p_Email nvarchar(128),
	@p_IsAdmin bit = 0,
	@p_IsActive bit = 0,
	@p_User int
as
begin
	set nocount on;
	set xact_abort on;

	if( @p_FirstName is null or @p_LastName is null or @p_Login is null or @p_PasswordHash is null or @p_User is null)
		raiserror (15600,-1,-1, 'inv.usp_UserAdd');
	else
		insert into inv.Users
		(
			Usr_FirstName,
			Usr_LastName,
			Usr_Login,
			Usr_PasswordHash,
			Usr_Email,
			Usr_IsAdmin,
			Usr_IsActive
		)
		values
		(
			@p_FirstName,
			@p_LastName,
			@p_Login,
			@p_PasswordHash,
			@p_Email,
			@p_IsAdmin,
			@p_IsActive
		);
end