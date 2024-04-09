using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WildCrest.Models.WildCrestModels;

namespace WildCrest.Models.ViewModels
{
    public class StockInfoVM
    {
        public double? TotalUsedQuantity { get; set; }
        public List<InventoryUsage> Records {get;set ;}
    }
}