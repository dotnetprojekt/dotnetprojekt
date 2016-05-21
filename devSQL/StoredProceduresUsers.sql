use InvoiceSystem

go

	if not exists (SELECT * FROM sys.schemas WHERE name = 'inv')
		EXEC('CREATE SCHEMA inv');

go

	if(object_id('inv.usp_UsersAdd') is not null)
		drop procedure inv.usp_UsersAdd;

	if(object_id('inv.usp_UsersSearch') is not null)
		drop procedure inv.usp_UsersSearch;

	if(object_id('inv.usp_UsersBlock') is not null)
		drop procedure inv.usp_UsersBlock;

	if(object_id('inv.usp_UsersUnblock') is not null)
		drop procedure inv.usp_UsersUnblock;

	if(object_id('inv.usp_UsersBlock') is not null)
		drop procedure inv.usp_PartnerGetById;

	if(object_id('inv.usp_UsersAccept') is not null)
		drop procedure inv.usp_UsersAccept;

	if(object_id('inv.usp_UsersLogin') is not null)
		drop procedure inv.usp_UsersLogin;

	if(object_id('inv.usp_UsersLogout') is not null)
		drop procedure inv.usp_UsersLogout;

go

	create procedure inv.usp_UsersAdd -- UserDAL.UserAdd()
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

	create procedure inv.usp_UsersSearch
		@p_FirstName nvarchar(128),
		@p_LastName nvarchar(128),
		@p_Login nvarchar(32),
		@p_Email nvarchar(128),
		@p_IsAdmin bit,
		@p_Status tinyint,
		@p_IsLogged bit,
		@p_pageNumber int,
		@p_rowsPerPage int
	as
	begin
		set nocount on;
		set xact_abort on;

		declare @v_QueryBody nvarchar(max) =
'select
	Usr_Id,
	Usr_FirstName,
	Usr_LastName,
	Usr_Login,
	Usr_Email,
	Usr_IsAdmin,
	Usr_Status,
	Usr_IsLogged
from inv.Users with(nolock)
where 1=1'

		declare @v_QueryConditions nvarchar(max) = '';

		declare @v_QueryEnd nvarchar(max) = 
char(13)+char(10)+'order by Usr_Id
offset ('+ convert(nvarchar(32),(@p_pageNumber-1)*@p_rowsPerPage) +') rows
fetch next ('+ convert(nvarchar(32),@p_rowsPerPage)+') rows only';

		if( @p_Login is not null )
			set @v_QueryConditions = @v_QueryConditions +char(13)+char(10)+'	and Usr_Login like ''%' + @p_Login + '%''';
		else
		begin
			if( @p_FirstName is not null)
				set @v_QueryConditions = @v_QueryConditions +char(13)+char(10)+'	and Usr_FirstName = ''' + @p_FirstName + '''';

			if( @p_LastName is not null)
				set @v_QueryConditions = @v_QueryConditions +char(13)+char(10)+'	and Usr_LastName = ''' + @p_LastName + '''';

			if( @p_Email is not null)
				set @v_QueryConditions = @v_QueryConditions +char(13)+char(10)+'	and Usr_Email = ''' + @p_Email + '''';

			if( @p_IsAdmin is not null)
				set @v_QueryConditions = @v_QueryConditions +char(13)+char(10)+'	and Usr_IsAdmin = ' + convert(nchar(1),@p_IsAdmin);

			if( @p_Status is not null)
				set @v_QueryConditions = @v_QueryConditions +char(13)+char(10)+'	and Usr_Status = ' + convert(nchar(1),@p_Status);

			if( @p_IsLogged is not null)
				set @v_QueryConditions = @v_QueryConditions +char(13)+char(10)+'	and Usr_IsLogged = ' + convert(nchar(1),@p_IsLogged);
		end

		set @v_QueryBody = @v_QueryBody+@v_QueryConditions+@v_QueryEnd;
		exec sp_executesql @v_QueryBody;
	end

go

	create procedure inv.usp_UsersBlock --UserDAL.UserBlock()
		@p_Login nvarchar(32)
	as
	begin
		set nocount on;
		set xact_abort on;

		update inv.Users with(rowlock)
		set Usr_Status = 3,
			Usr_IsLogged = 0
		where Usr_Login = @p_Login
			and Usr_Status != 3
			and Usr_IsAdmin = 0;

		if(@@ROWCOUNT > 0)
			return 1;
		else
			return 0;
	end

go

	create procedure inv.usp_UsersUnblock --UserDAL.UserUnblock()
		@p_Login nvarchar(32)
	as
	begin
		set nocount on;
		set xact_abort on;

		update inv.Users with(rowlock)
		set Usr_Status = 2
		where Usr_Login = @p_Login
			and Usr_Status = 3;

		if(@@ROWCOUNT > 0)
			return 1;
		else
			return 0;
	end

go

	create procedure inv.usp_UsersAccept --UserDAL.UserAccept()
		@p_Login nvarchar(32)
	as
	begin
		set nocount on;
		set xact_abort on;

		update inv.Users with(rowlock)
		set Usr_Status = 2
		where Usr_Login = @p_Login
			and Usr_Status = 1

		if(@@ROWCOUNT > 0)
			return 1;
		else
			return 0;
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
		where Usr_Login = @p_Login
			and Usr_IsLogged = 1;

		if(@@ROWCOUNT > 0)
			return 1;
		else
			return 0;
	end

go