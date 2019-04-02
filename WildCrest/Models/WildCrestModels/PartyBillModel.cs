using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WildCrest.Models.WildCrestModels
{
    public class PartyBillModel
    {
        public int Bill_ID { get; set; }
        public string Price_Without_Gst { get; set; }
        public string Gst_Amount { get; set; }
        public string Price_With_Gst { get; set; }
        public string Qty { get; set; }
        public string Total_Amount { get; set; }
        public string Amount_Paid { get; set; }
        public string Mode_Of_Payment { get; set; }
        public string Date_Of_Billing { get; set; }
        public Nullable<int> Billed_By { get; set; }
        public Nullable<int> Party_ID { get; set; }
        public Nullable<bool> IsAdvance_Payment { get; set; }
    }
}