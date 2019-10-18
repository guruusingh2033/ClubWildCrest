using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WildCrest.Models.WildCrestModels;

namespace WildCrest.Models.WildCrestModels
{
    public class MembershipHistoryModel
    {

       public List<RoomBooking_Details> RoomsDetails { get; set; }
    public int? TotalStay { get; set; }
        public int? ComplementaryStay { get; set; }
        public string MembershipPlan { get; set; }
        public string MembershipPlanDate { get; set; }
    }
    public class MembershipHistoryEntity
    {
        public int HistoryID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> MembershipID { get; set; }
        public DateTime MembershipJoiningDate { get; set; }
        public DateTime MembershipExpiryDate { get; set; }
    }


    }