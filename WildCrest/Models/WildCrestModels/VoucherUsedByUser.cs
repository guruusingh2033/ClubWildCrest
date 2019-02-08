using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WildCrest.Models.WildCrestModels
{
    public class VoucherUsedByUser
    {
        public int ID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> VoucherID { get; set; }
    }
}