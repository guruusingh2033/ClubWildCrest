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
    
    public partial class tbl_UsersStay
    {
        public tbl_UsersStay()
        {
            this.tbl_BillingDetails = new HashSet<tbl_BillingDetails>();
            this.tbl_MembersWhileStayingWithUser = new HashSet<tbl_MembersWhileStayingWithUser>();
            this.tbl_UsersOrder = new HashSet<tbl_UsersOrder>();
        }
    
        public int ID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string CheckInDate { get; set; }
        public string CheckoutDate { get; set; }
        public Nullable<int> MemberID { get; set; }
        public Nullable<int> NoOfMembers { get; set; }
        public Nullable<int> RoomID { get; set; }
        public string NonMemberName { get; set; }
        public string NonMemberPhoneNo { get; set; }
        public Nullable<int> NoOfNightStays { get; set; }
        public Nullable<int> ComplementaryStays { get; set; }
        public Nullable<double> TotalAmountWithoutTax { get; set; }
        public Nullable<double> AdvancedPayment { get; set; }
    
        public virtual ICollection<tbl_BillingDetails> tbl_BillingDetails { get; set; }
        public virtual tbl_MembershipPlans tbl_MembershipPlans { get; set; }
        public virtual ICollection<tbl_MembersWhileStayingWithUser> tbl_MembersWhileStayingWithUser { get; set; }
        public virtual tbl_Profile tbl_Profile { get; set; }
        public virtual ICollection<tbl_UsersOrder> tbl_UsersOrder { get; set; }
    }
}
