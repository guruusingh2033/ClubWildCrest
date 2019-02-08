using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WildCrest.Models.WildCrestModels
{
    public class Voucher
    {
        public int ID { get; set; }
        public string VoucherName { get; set; }
        public string VoucherDetails { get; set; }
        public string AddedBy { get; set; }
        public Nullable<bool> VoucherForApprvFrmSuperAdm { get; set; }
        public Nullable<bool> IsDeleted { get; set; }

        public string DeletedBy { get; set; }
    }
}