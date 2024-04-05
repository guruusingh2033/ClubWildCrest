Create procedure GetInventoryUsageData       
(      
@StartDate DateTime,           
@EndDate DateTime 
)      
AS      
Set nocount on              
SELECT inv.ID,
       inv.Item_Name, 
       sum(iu.Used_Qty) as UsedQuantity, 
	   inv.Type, 
	   inv.Price,
	   inv.Measurement,
	   CONCAT(v.Vendor_First_Name, ' ', v.Vendor_Last_Name) AS VendorName,
	   SUBSTRING(MAX(iu.Used_Date), 1, 2) + '/' + SUBSTRING(MAX(iu.Used_Date), 4, 2) + '/' + SUBSTRING(MAX(iu.Used_Date), 7, 4) AS UsageDate
	   --iu.Used_Date as UsageDate
		FROM  tbl_Inventory AS inv
		Left JOIN tbl_InventoryUsage AS iu ON inv.ID = iu.InventoryID
		JOIN dbo.tbl_Vendors AS v ON inv.VendorID = v.ID
		Where CAST(iu.Used_Date AS date) >= CONVERT(date, @StartDate) and CAST(iu.Used_Date AS date) <= CONVERT(date, @EndDate)
		group by inv.ID, inv.Item_Name, inv.Type, inv.Price, v.Vendor_First_Name, v.Vendor_Last_Name, inv.Measurement
		
		          
              
         
