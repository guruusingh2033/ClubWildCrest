//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WildCrest
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_NonGST_MenusBillDetWithBillNo
    {
        public int ID { get; set; }
        public Nullable<int> NonGst_BillNo { get; set; }
        public string FoodName { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<double> Quantity { get; set; }
        public Nullable<double> OldQuantity { get; set; }
        public string QtyUpdatedDate { get; set; }
        public string QtyUpdatedBy { get; set; }
    }
}
