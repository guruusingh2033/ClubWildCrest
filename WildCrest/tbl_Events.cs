//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WildCrest
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_Events
    {
        public tbl_Events()
        {
            this.tbl_EventsImgVideo = new HashSet<tbl_EventsImgVideo>();
        }
    
        public int ID { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public string OrganisedBy { get; set; }
        public string EventDate { get; set; }
        public string EventTime { get; set; }
        public string Place { get; set; }
        public string ImageUrl { get; set; }
        public string AddedBy { get; set; }
        public Nullable<bool> NewEventsApprvFrmSuper { get; set; }
        public Nullable<bool> DelEventsApprvFrmSuper { get; set; }
        public string DeletedBy { get; set; }
    
        public virtual ICollection<tbl_EventsImgVideo> tbl_EventsImgVideo { get; set; }
    }
}
