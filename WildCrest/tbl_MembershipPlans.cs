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
    
    public partial class tbl_MembershipPlans
    {
        public tbl_MembershipPlans()
        {
            this.tbl_UserMembership = new HashSet<tbl_UserMembership>();
            this.tbl_UsersStay = new HashSet<tbl_UsersStay>();
        }
    
        public int ID { get; set; }
        public string PlanName { get; set; }
        public string PlanDetails { get; set; }
        public Nullable<int> NoOfStays { get; set; }
        public Nullable<bool> MemShipApprvFrmSuperAdm { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string AddedBy { get; set; }
        public string DeletedBy { get; set; }
        public Nullable<double> PlanAmount { get; set; }
        public int MembershipPlanForYear { get; set; }
    
        public virtual ICollection<tbl_UserMembership> tbl_UserMembership { get; set; }
        public virtual ICollection<tbl_UsersStay> tbl_UsersStay { get; set; }
    }
}
