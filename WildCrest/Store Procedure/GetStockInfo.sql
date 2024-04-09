CREATE procedure GetStockInfo       
(      
@InventoryID int 
)      
AS      
Set nocount on              
	SELECT  
	iu.Description , iu.Used_Qty, iu.Used_Date,
	inv.Quantity as TotalQuantity
	 FROM 
        tbl_InventoryUsage AS iu
		 JOIN 
        tbl_Inventory AS inv ON iu.InventoryID = inv.ID
    WHERE 
        iu.InventoryID = @InventoryID 
        AND CAST(iu.Used_Date AS DATE) >= DATEADD(year, -2, GETDATE())
		ORDER BY 
    CAST(iu.Used_Date AS DATE) DESC