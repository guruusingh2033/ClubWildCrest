using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WildCrest.Models.WildCrestModels
{
    public class MemberGroup
    {
        public int ID { get; set; }
        public string GroupName { get; set; }        
        public Nullable<int> UserID { get; set; }
        public string GroupDetail { get; set; }
        public int NoOfMembers { get; set; }
        public List<KeyValuePair<int, string>> MembersNameWithId { get; set; }
    }
}