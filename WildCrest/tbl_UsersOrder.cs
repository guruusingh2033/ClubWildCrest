//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WildCrest
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_UsersOrder
    {
        public int ID { get; set; }
        public string FoodItemName { get; set; }
        public string Description { get; set; }
        public Nullable<int> UserStay_ID { get; set; }
        public Nullable<double> Cost { get; set; }
        public Nullable<int> UserID { get; set; }
    
        public virtual tbl_UsersStay tbl_UsersStay { get; set; }
    }
}
