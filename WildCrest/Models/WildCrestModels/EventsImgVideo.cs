using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WildCrest.Models.WildCrestModels
{
    public class EventsImgVideo
    {
        public int ID { get; set; }
        public Nullable<int> EventID { get; set; }
        public string Img_Video_Path { get; set; }
    }
}