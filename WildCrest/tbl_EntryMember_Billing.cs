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
    
    public partial class tbl_EntryMember_Billing
    {
        public int Bill_ID { get; set; }
        public Nullable<double> Price_Without_Gst { get; set; }
        public Nullable<double> Gst_Amount { get; set; }
        public Nullable<double> Price_With_Gst { get; set; }
        public string Members { get; set; }
        public Nullable<double> Total_Amount { get; set; }
        public Nullable<double> Amount_Paid { get; set; }
        public string Mode_Of_Payment { get; set; }
        public string Date_Of_Billing { get; set; }
        public Nullable<int> Billed_By { get; set; }
        public Nullable<int> Member_ID { get; set; }
    }
}
