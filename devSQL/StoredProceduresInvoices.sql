go

create procedure inv.InvoiceAdd
	@p_VendorId int,
	@p_BuyerId int,
	@p_Title nvarchar(2048),
	@p_Goods xml,
	@p_OverallNetValue decimal(9,2),
	@p_OverallGrossValue decimal(9,2),
	@p_Discount decimal(3,2),
	@p_OverallCost decimal(3,2),
	@p_Creator int
as
begin
	set nocount on;
	set xact_abort on;

	if( @p_VendorId is null or @p_BuyerId is null or @p_Title is null or @p_OverallNetValue is null or @p_OverallGrossValue is null or @p_OverallCost is null or @p_Creator is null )
		raiserror (15600,-1,-1, 'inv.usp_UserAdd');
	else
	begin
		insert into inv.Invoices
		(
			Inv_VendorId,
			Inv_BuyerId,
			Inv_Title,
			Inv_Goods,
			Inv_OverallNetValue,
			Inv_OverallGrossValue,
			Inv_Discount,
			Inv_OverallCost,
			Inv_Creator
		)
		values
		(
			@p_VendorId,
			@p_BuyerId,
			@p_Title,
			@p_Goods,
			@p_OverallNetValue,
			@p_OverallGrossValue,
			@p_Discount,
			@p_OverallCost,
			@p_Creator
		);
	end
end