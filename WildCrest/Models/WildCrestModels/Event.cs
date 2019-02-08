using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WildCrest.Models.WildCrestModels
{
    public class Event
    {
        public int ID { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public string OrganisedBy { get; set; }
        public string EventDate { get; set; }
        public string EventTime { get; set; }
        public string Place { get; set; }
        public int ImageID { get; set; }
        public string ImagePath { get; set; }
        public string AddedBy { get; set; }

        public string DeletedBy { get; set; }

        public Nullable<bool> NewEventsApprvFrmSuper { get; set; }
        public Nullable<bool> DelEventsApprvFrmSuper { get; set; }
    }
}