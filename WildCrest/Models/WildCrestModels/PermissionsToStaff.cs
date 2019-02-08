using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WildCrest.Models.WildCrestModels
{
    public class PermissionsToStaff
    {
        public int ID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string MemberPermission { get; set; }
        public string MembershipPlanPermission { get; set; }
        public string EventsPermission { get; set; }
        public string VendorsPermission { get; set; }
        public string MenuItemsPermission { get; set; }
        public string FoodBillingEditPermission { get; set; }
    }
}