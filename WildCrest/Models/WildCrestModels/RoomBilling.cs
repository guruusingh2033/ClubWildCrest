using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WildCrest.Models.WildCrestModels
{
    public class RoomBilling
    {
        public int ID { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNo { get; set; }
        public string RoomID { get; set; }
        public string CheckinDate { get; set; }
        public Nullable<double> Amount { get; set; }
        //public Nullable<double> ExtraBedCharges { get; set; }
        public Nullable<double> SGST { get; set; }
        public Nullable<double> CGST { get; set; }
        public Nullable<double> NetAmount { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> Discount { get; set; }

        public Nullable<double> AdvancedPayment { get; set; }
        public Nullable<double> AmtToBePaid { get; set; }

    }
}