using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WildCrest.Models.WildCrestModels
{
    public class MembersDocs
    {
        public int UserID { get; set; }
        public string ImagePath { get; set; }
    }

    public class MemberPhoto
    {
        public int UserID { get; set; }
        public string DocPath { get; set; }

    }
}