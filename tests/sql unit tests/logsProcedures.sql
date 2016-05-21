exec tSQLt.NewTestClass 'StoredProceduresLogTest';

go
create procedure StoredProceduresLogTest.[testWriteLog] 
as
begin
	exec tsqlt.FakeTable 'dbo.Logs'
	declare @retCount int;
	declare @status1 nvarchar(16);
	declare @status2 nvarchar(16);
	declare @status3 nvarchar(16);

	exec inv.usp_WriteLog 'Test context 1', 1, 'Login1', 'Test description 1';
	exec inv.usp_WriteLog 'Test context 2', 2, 'Login2', 'Test description 2';
	exec inv.usp_WriteLog 'Test context 3', 3, 'Login3', 'Test description 3';

	select @retCount = count(*) from dbo.Logs;

	select @status1 = Log_Status from dbo.Logs where Log_Context = 'Test context 1';
	select @status2 = Log_Status from dbo.Logs where Log_Context = 'Test context 2';
	select @status3 = Log_Status from dbo.Logs where Log_Context = 'Test context 3';

	exec tSQLt.AssertEquals 3, @retCount;
	exec tSQLt.AssertEquals 'INFO', @status1;
	exec tSQLt.AssertEquals 'WARNING', @status2;
	exec tSQLt.AssertEquals 'ERROR', @status3;
end;

go
create procedure StoredProceduresLogTest.[testWriteErrorLog] 
as
begin
	exec tsqlt.FakeTable 'dbo.Logs';

	exec tSQLt.ExpectException;
	exec inv.usp_WriteLog null, 1, 'Login', 'Test description';
	
end;


go
exec tSQLt.RunAll