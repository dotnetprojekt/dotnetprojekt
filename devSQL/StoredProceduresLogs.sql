use InvoiceSystem;

go

	if(object_id('inv.usp_WriteLog') is not null)
		drop procedure inv.usp_WriteLog;

go

	create procedure inv.usp_WriteLog
		@p_Context nvarchar(1024),
		@p_Status int,
		@p_Login nvarchar(32),
		@p_Description nvarchar(4000)

	as
	begin
		set nocount on;
		set xact_abort on;

		if(@p_Context is null or @p_Status is null or @p_Login is null)
			raiserror (15600,-1,-1, 'inv.usp_PartnerAdd');
		else
			insert into dbo.Logs with(rowlock)
			(
				Log_Context,
				Log_Status,
				Log_User,
				Log_Description
			)
			values
			(
				@p_Context,
				(
					select	case
								when @p_Status = 1 then 'INFO'
								when @p_Status = 2 then 'WARNING'
								when @p_Status = 3 then 'ERROR'
							end
				),
				(
					select Usr_Id
					from inv.Users
					where Usr_Login = @p_Login
				),
				@p_Description
			);
	end