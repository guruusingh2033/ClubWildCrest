using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WildCrest.Models.WildCrestModels
{
    public class EntryMemberModel
    {
        public int ID { get; set; }
      
        public string Name { get; set; }
        public string Phone_No { get; set; }
       
        public string Total_Member { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<double> Total_Amount { get; set; }

        public Nullable<double> GstAmount { get; set; }
        public Nullable<double> Price_Without_Gst { get; set; }



        
        public int BillNo { get; set; }
        public string ModeOfPayment { get; set; }
        public string DateofBilling { get; set; }
        public Nullable<double> AmountPaid { get; set; }
        public string TokenNo { get; set; }
        public string Entrybillno { get; set; }
    }
}