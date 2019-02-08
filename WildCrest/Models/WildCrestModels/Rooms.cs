using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WildCrest.Models.WildCrestModels
{
    public class Rooms
    {
        public int? ID { get; set; }
        public string Room_Type { get; set; }
        public Nullable<int> RoomCost { get; set; }
        public string RoomNo { get; set; }
    }
}