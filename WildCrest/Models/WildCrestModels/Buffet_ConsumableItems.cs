using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WildCrest.Models.WildCrestModels
{
    public class Buffet_ConsumableItems
    {
        
            public int ID { get; set; }
            public Nullable<int> BuffetItem_ID { get; set; }
            public Nullable<int> Inventory_ID { get; set; }
            public Nullable<double> Quantity { get; set; }
            public string Measurement { get; set; }

            public string Inventory_ItemName { get; set; }
            public string Price { get; set; }
        
    }
}