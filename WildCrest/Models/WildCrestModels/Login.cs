using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WildCrest.Models.WildCrestModels
{
    public class Login
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Nullable<int> UserType { get; set; }

        //public virtual tbl_Roles tbl_Roles { get; set; }
    }
}