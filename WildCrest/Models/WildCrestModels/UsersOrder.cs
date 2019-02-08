using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WildCrest.Models.WildCrestModels
{
    public class UsersOrder
    {
        public int ID { get; set; }
        public string FoodItemName { get; set; }
        public string Description { get; set; }
        public Nullable<int> UserStay_ID { get; set; }
        public Nullable<double> Cost { get; set; }
        public Nullable<int> UserID { get; set; }
    }
}