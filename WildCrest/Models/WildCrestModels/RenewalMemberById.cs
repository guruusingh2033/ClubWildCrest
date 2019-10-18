using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WildCrest.Models.WildCrestModels
{
    public class RenewalMemberById
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string CustomMemberID { get; set; }
        public string MembershipJoiningDate { get; set; }
        public int MembershipPlanID { get; set; }
        public string Mode_Of_Payment { get; set; }
        public string Cheque_No { get; set; }
        public string BankName { get; set; }
        public double Amount_Paid { get; set; }
    }
}