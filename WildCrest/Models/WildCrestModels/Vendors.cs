using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WildCrest.Models.WildCrestModels
{
    public class Vendors
    {
        public int ID { get; set; }
        public string Vendor_First_Name { get; set; }
        public string Vendor_Last_Name { get; set; }
        public string Contact_No { get; set; }
        public string EmailID { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public Nullable<bool> IsNewApproved { get; set; }
        public Nullable<bool> isDeleted { get; set; }

        public string AddedBy { get; set; }
        public string DeletedBy { get; set; }
    }

    public class VendorOrders
    {
        public int ID { get; set; }
        public Nullable<int> VendorID { get; set; }
        public string Product_Name { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> Price { get; set; }
    }

    public class ViewOrders
    {
        public int VendorOrderID { get; set; }
        public string Vendor_Full_Name { get; set; }
        public string Contact_No { get; set; }
        public string Product_Name { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> Price { get; set; }
    }
}