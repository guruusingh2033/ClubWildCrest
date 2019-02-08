using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WildCrest.Models.WildCrestModels
{
    public class UserMembership
    {
        public int ID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> MembershipID { get; set; }
        public string PlanName { get; set; }
        public string PlanDetails { get; set; }
        public Nullable<int> NoOfStays { get; set; }
    }

    public class MembershipPlans
    {
        public int ID { get; set; }
        public string PlanName { get; set; }
        public string PlanDetails { get; set; }
        public Nullable<int> NoOfStays { get; set; }
        public Nullable<bool> MemShipApprvFrmSuperAdm { get; set; }
        public Nullable<double> PlanAmount { get; set; }
        public int MembershipPlanForYear { get; set; }

        public string AddedBy { get; set; }
        public string DeletedBy { get; set; }
    }
}