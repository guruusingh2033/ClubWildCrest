using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WildCrest.Models.WildCrestModels
{
    public class EditMember
    {
        public int ID { get; set; }
        public string F_Name { get; set; }
        public string L_Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string DOB { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public Nullable<bool> EmailNotifications { get; set; }
       
        public string Password { get; set; }
    }
}