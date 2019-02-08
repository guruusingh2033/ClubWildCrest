using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WildCrest.Models.WildCrestModels
{
    public class NonGST_MenusBillingSection
    {
        public int NonGstBillNo { get; set; }
        public string Customer_Name { get; set; }
        public string Phone { get; set; }
        public Nullable<double> PriceWithoutTax { get; set; }
        public Nullable<int> UserID { get; set; }
        public string PaymentDate { get; set; }
        public string Mode_Of_Payment { get; set; }
        public Nullable<int> Billed_By { get; set; }

        public List<NonGST_MenusBillDetWithBillNo> NonGST_MenusBillDetWithBillNo { get; set; }

        public int Temp_Day_Data { get; set; }
        public string Temp_Tax_Data { get; set; }
        public int Temp_AdminID_Data { get; set; }
        public string Temp_SDate_Data { get; set; }
        public string Temp_EndDate_Data { get; set; }
    }

    public class NonGST_MenusBillDetWithBillNo
    {
        public int ID { get; set; }
        public Nullable<int> NonGst_BillNo { get; set; }
        public string FoodName { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<double> Quantity { get; set; }
        public Nullable<double> OldQuantity { get; set; }
        public string QtyUpdatedDate { get; set; }
        public string QtyUpdatedBy { get; set; }

        public int ItemNameID { get; set; }
    }
}