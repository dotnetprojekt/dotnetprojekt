use InvoiceSystem;

go

create procedure inv.usp_WriteLog
	@p_Context nvarchar(1024),
	@p_Status int,
	@p_Description nvarchar(4000)
as
begin
	set nocount on;
	set xact_abort on;

	if(@p_Context is null or @p_Status is null)
		raiserror (15600,-1,-1, 'inv.usp_PartnerAdd');
	else
		insert into inv.Logs
		(
			Log_Context,
			Log_Status,
			Log_Description
		)
		values
		(
			@p_Context,
			(select	case
						when @p_Status = 1 then 'INFO'
						when @p_Status = 2 then 'WARNING'
						when @p_Status = 3 then 'ERROR'
					end),
			@p_Description
		);
end