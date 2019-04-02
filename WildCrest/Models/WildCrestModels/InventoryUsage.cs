using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WildCrest.Models.WildCrestModels
{
    public class InventoryUsage
    {
        public int ID { get; set; }
        public Nullable<int> InventoryID { get; set; }
        public Nullable<double> Used_Qty { get; set; }
        public string Description { get; set; }
        public double? TotalQuantity { get; set; }
        public bool IsBuffetFood { get; set; }
        public string Used_Date { get; set; }

        public int Left_Stock { get; set; }

        public Nullable<int> MenusBillingDetailsID { get; set; }
        public Nullable<int> BillNo { get; set; }
        public string GST_NonGST_Bill { get; set; }
    }
}