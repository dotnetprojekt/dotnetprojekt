exec tSQLt.NewTestClass 'StoredProceduresUsersTest';

go
create procedure StoredProceduresUsersTest.[testLoginSuccesssful] 
as
begin
	exec tsqlt.FakeTable 'inv.Users'
	declare @correctLogin nvarchar
    declare @correctPassword nvarchar
	declare @retIsLogged int

    set @correctLogin='correctLogin'
	set @correctPassword='correctPassword'

	exec inv.usp_UsersAdd 'FirstName','LastName', @correctLogin, @correctPassword,'test.mail@example.com',0,2
	exec inv.usp_UsersAccept @correctLogin
	
	exec @retIsLogged = inv.usp_UsersLogin @correctLogin, @correctPassword;
	exec tSQLt.AssertEquals 1, @retIsLogged -- 1 == is logged
end;

go
create procedure StoredProceduresUsersTest.[testLoginIncorrectPassword] 
as
begin
	exec tsqlt.FakeTable 'inv.Users'
	declare @correctLogin nvarchar
    declare @correctPassword nvarchar
    declare @incorrectPassword nvarchar
	declare @retIsLogged int

    set @correctLogin='testLogin'
	set @correctPassword='correctPassword'
	set @incorrectPassword ='incorrectPassword'

	exec inv.usp_UsersAdd 'FirstName','LastName', @correctLogin, @correctPassword,'test.mail@example.com',0,2
	exec inv.usp_UsersAccept @correctLogin

	exec @retIsLogged = inv.usp_UsersLogin @correctLogin, @incorrectPassword;
	exec tSQLt.AssertEquals 0, @retIsLogged -- 0 == not logged
end;


go
create procedure StoredProceduresUsersTest.[testLoginIncorrectLogin] 
as
begin
	exec tsqlt.FakeTable 'inv.Users'
	declare @correctLogin nvarchar
	declare @incorrectLogin nvarchar
    declare @correctPassword nvarchar
	declare @retIsLogged int

    set @correctLogin='correctLogin'	
	set @incorrectLogin ='incorrectLogin'
	set @correctPassword='correctPassword'

	exec inv.usp_UsersAdd 'FirstName','LastName', @correctLogin, @correctPassword,'test.mail@example.com',0,2
	exec inv.usp_UsersAccept @correctLogin

	exec @retIsLogged = inv.usp_UsersLogin @incorrectLogin, @correctPassword;
	exec tSQLt.AssertEquals 0, @retIsLogged -- 0 == not logged
end;

go
create procedure StoredProceduresUsersTest.[testLogout] 
as
begin
	exec tsqlt.FakeTable 'inv.Users'
	declare @correctLogin nvarchar
	declare @retIsLogged int

    set @correctLogin='correctLogin'

	exec inv.usp_UsersAdd 'FirstName','LastName', @correctLogin, 'testPassword','test.mail@example.com',0,2
	exec inv.usp_UsersAccept @correctLogin

	exec inv.usp_UsersLogout @correctLogin;
	select @retIsLogged=Usr_isLogged from inv.Users where Usr_Login = @correctLogin;
	exec tSQLt.AssertEquals 0, @retIsLogged -- 0 == not logged
end;

go
create procedure StoredProceduresUsersTest.[testUserAccept] 
as
begin
	exec tsqlt.FakeTable 'inv.Users'
	declare @correctLogin nvarchar
	declare @retVal int

    set @correctLogin='correctLogin'

	exec inv.usp_UsersAdd 'FirstName','LastName', @correctLogin, 'testPassword','test.mail@example.com',0,2
	exec inv.usp_UsersAccept @correctLogin

	select @retVal=Usr_Status from inv.Users where Usr_Login = @correctLogin;
	
	exec tSQLt.AssertEquals 2, @retVal -- 2 == active user
end;


go
create procedure StoredProceduresUsersTest.[testUserBlock] 
as
begin
	exec tsqlt.FakeTable 'inv.Users'
	declare @correctLogin nvarchar
	declare @retVal int

    set @correctLogin='correctLogin'

	exec inv.usp_UsersAdd 'FirstName','LastName', @correctLogin, 'testPassword','test.mail@example.com',0,2
	exec inv.usp_UsersBlock @correctLogin

	select @retVal=Usr_Status from inv.Users where Usr_Login = @correctLogin;
	
	exec tSQLt.AssertEquals 3, @retVal -- 3 == blocked user
end;

go
create procedure StoredProceduresUsersTest.[testUserUnblock] 
as
begin
	exec tsqlt.FakeTable 'inv.Users'
	declare @correctLogin nvarchar
	declare @retVal int

    set @correctLogin='correctLogin'

	exec inv.usp_UsersAdd 'FirstName','LastName', @correctLogin, 'testPassword','test.mail@example.com',0,2
	exec inv.usp_UsersBlock @correctLogin
	select @retVal=Usr_Status from inv.Users where Usr_Login = @correctLogin;
	exec tSQLt.AssertEquals 3, @retVal -- 3 == blocked user

	exec inv.usp_UsersUnblock @correctLogin

	select @retVal=Usr_Status from inv.Users where Usr_Login = @correctLogin;
	
	exec tSQLt.AssertEquals 2, @retVal -- 2 == blocked user
end;


go
create procedure StoredProceduresUsersTest.[testUserList] 
as
begin
	exec tsqlt.FakeTable 'inv.Users'
	declare @retCount int;

	declare  @Users TABLE (
		Usr_Id int,
		Usr_FirstName nvarchar(128),
		Usr_LastName nvarchar(128),
		Usr_Login nvarchar(32),
		Usr_Email nvarchar(128),
		Usr_IsAdmin bit,
		Usr_Status nvarchar(128),
		Usr_IsLogged bit
	);
	exec inv.usp_UsersAdd 'FirstName1','LastName1', 'Login1', 'testPassword1','test.mail1@example.com', 0, 2;
	exec inv.usp_UsersAdd 'FirstName2','LastName2', 'Login2', 'testPassword2','test.mail2@example.com', 0, 2;

    insert @Users(Usr_Id,Usr_FirstName,Usr_LastName,Usr_Login,Usr_Email,Usr_IsAdmin,Usr_Status,Usr_IsLogged) exec inv.usp_UsersList 1,30; 
	
	select @retCount=count(*) from @Users;
	exec tSQLt.AssertEquals 2, @retCount

	delete from @Users;
	insert @Users(Usr_Id,Usr_FirstName,Usr_LastName,Usr_Login,Usr_Email,Usr_IsAdmin,Usr_Status,Usr_IsLogged) exec inv.usp_UsersList 2,1; 
	select @retCount=count(*) from @Users;
	exec tSQLt.AssertEquals 1, @retCount

end;

go
create procedure StoredProceduresUsersTest.[testUserAdd] 
as
begin
	exec tsqlt.FakeTable 'inv.Users';
	declare @retVal nvarchar(128);
	exec inv.usp_UsersAdd 'FirstName','LastName', 'testLogin', 'testPassword','test.mail@example.com', 0, 2;
	select top(1) @retVal=Usr_Login from inv.Users;	
	exec tSQLt.AssertEquals 'testLogin', @retVal
end;

go
create procedure StoredProceduresUsersTest.[testUserErrorAdd] 
as
begin
	exec tsqlt.FakeTable 'inv.Users';

	exec tSQLt.ExpectException;
	exec inv.usp_UsersAdd null,'LastName', 'testLogin', 'testPassword','test.mail@example.com', 0, 2;
end;

go
exec tSQLt.RunAll