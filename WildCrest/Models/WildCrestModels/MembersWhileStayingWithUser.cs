using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WildCrest.Models.WildCrestModels
{
    public class MembersWhileStayingWithUser
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> Age { get; set; }
        public string Gender { get; set; }
        public Nullable<int> UserStay_ID { get; set; }
        public Nullable<int> UserID { get; set; }
    }
}