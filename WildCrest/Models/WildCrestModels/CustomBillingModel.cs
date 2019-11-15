using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WildCrest.Models.WildCrestModels
{
    public class CustomBillingModel
    {
        public int ServiceID { get; set; }
        public string ServiceBillNO { get; set; }
        public string Customer { get; set; }
        public Nullable<double> Amount_Without_Gst { get; set; }
        public Nullable<double> Gst_Amount { get; set; }
        public Nullable<int> Qty { get; set; }
        public string Details { get; set; }
        public string PhoneNo { get; set; }
        public string Mode_Of_Payment { get; set; }
        public Nullable<System.DateTime> BillingDate { get; set; }
        public string billingDateStr { get; set; }
        public Nullable<double> TotalAmount { get; set; }
        public string ServiceName { get; set; }
        public Nullable<int> Billed_By { get; set; }
    }
}