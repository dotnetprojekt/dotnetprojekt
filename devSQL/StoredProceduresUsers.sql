use InvoiceSystem

go

create procedure inv.usp_UsersAdd
	@p_FirstName nvarchar(128),
	@p_LastName nvarchar(128),
	@p_Login nvarchar(32),
	@p_PasswordHash nvarchar(128),
	@p_Email nvarchar(128),
	@p_IsAdmin bit = 0,
	@p_User int
as
begin
	set nocount on;
	set xact_abort on;

	if( @p_FirstName is null or @p_LastName is null or @p_Login is null or @p_PasswordHash is null or @p_User is null)
		raiserror (15600,-1,-1, 'inv.usp_UserAdd');
	else
		insert into inv.Users with(rowlock)
		(
			Usr_FirstName,
			Usr_LastName,
			Usr_Login,
			Usr_PasswordHash,
			Usr_Email,
			Usr_IsAdmin
		)
		values
		(
			@p_FirstName,
			@p_LastName,
			@p_Login,
			@p_PasswordHash,
			@p_Email,
			@p_IsAdmin
		);
end

go

create procedure inv.usp_UsersList
	@p_pageNumber int,
	@p_rowsPerPage int
as
begin
	set nocount on;
	set xact_abort on;

	select
		Usr_Id,
		Usr_FirstName,
		Usr_LastName,
		Usr_Login,
		Usr_Email,
		Usr_IsAdmin,
		case
			when Usr_Status = 1 then 'NEW'
			when Usr_Status = 2 then 'ACTIVE'
			when Usr_Status = 3 then 'BLOCKED'
		end,
		Usr_IsLogged
	from inv.Users with(nolock)
	order by Usr_Id
	offset ((@p_pageNumber-1)*@p_rowsPerPage) rows
	fetch next (@p_rowsPerPage) rows only;
end

go

create procedure inv.usp_UsersBlock
	@p_Login nvarchar(32)
as
begin
	set nocount on;
	set xact_abort on;

	update inv.Users with(rowlock)
	set Usr_Status = 3
	where Usr_Login = @p_Login
		and Usr_Status in (1,2)
end

go

create procedure inv.usp_UsersUnblock
	@p_Login nvarchar(32)
as
begin
	set nocount on;
	set xact_abort on;

	update inv.Users with(rowlock)
	set Usr_Status = 2
	where Usr_Login = @p_Login
		and Usr_Status = 3;
end

go

create procedure inv.usp_UsersAccept
	@p_Login nvarchar(32)
as
begin
	set nocount on;
	set xact_abort on;

	update inv.Users with(rowlock)
	set Usr_Status = 2
	where Usr_Login = @p_Login
		and Usr_Status = 1
end

go

create procedure inv.usp_UsersLogin
	@p_Login nvarchar(32),
	@p_PasswordHash nvarchar(128)
as
begin
	set nocount on;
	set xact_abort on;
	declare @v_loginStatus bit = 0;

	begin transaction

		update inv.Users with(rowlock)
		set Usr_IsLogged = 1
		where Usr_Login = @p_Login
			and Usr_PasswordHash = @p_PasswordHash
			and Usr_IsLogged = 0
			and Usr_Status = 2

		if(@@ROWCOUNT > 0)
			set @v_loginStatus = 1;
		else
			set @v_loginStatus = 0;

	commit transaction

	return @v_loginStatus;
end

go

create procedure inv.usp_UsersLogout
	@p_Login nvarchar(32)
as
begin
	set nocount on;
	set xact_abort on;

	update inv.Users with(rowlock)
	set Usr_IsLogged = 0
	where Usr_Login = @p_Login;
end