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
    
    public partial class tbl_RoomBooking
    {
        public int Booking_ID { get; set; }
        public Nullable<double> AdvancedPayment { get; set; }
        public string Check_In { get; set; }
        public string Check_Out { get; set; }
        public Nullable<double> Amount { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> MemberID { get; set; }
        public Nullable<int> NoOfNightStays { get; set; }
        public Nullable<double> Discount { get; set; }
        public Nullable<double> GST { get; set; }
        public Nullable<double> AmtToBePaid { get; set; }
        public Nullable<int> Bill_Number { get; set; }
        public Nullable<System.DateTime> Billing_DateTime { get; set; }
        public string Mode_Of_Payment { get; set; }
        public string Cheque_No { get; set; }
        public string BankName { get; set; }
    }
}
