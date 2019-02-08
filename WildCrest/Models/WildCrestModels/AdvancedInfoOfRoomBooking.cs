using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WildCrest.Models.WildCrestModels
{
    public class AdvancedInfoOfRoomBooking
    {
        //public string Checkin { get; set; }
        //public string Checkout { get; set; }
        public int? RoomCost { get; set; }
        public string RoomNo { get; set; }
        public string RoomType { get; set; }
        public string MemberName { get; set; }
        public string NonMember { get; set; }
        public string MemberPhoneNo { get; set; }
        public string NonMemberPhone { get; set; }
        
        //public int UserStayID { get; set; }
        public int ID { get; set; }
        public int? RoomID { get; set; }
        public int UserID { get; set; }
        public int Booking_ID { get; set; }
        public string Check_In { get; set; }
        public string Check_Out { get; set; }
        public int? Bill_Number { get; set; }
    }

    public class AdvancedInfoOfTableBooking
    {
        public int? TableID { get; set; }        
        public string BookingDate { get; set; }
        public string BookingStartTime { get; set; }
        public string BookingEndTime { get; set; }
        public string TableNo { get; set; }
        public string MemberName { get; set; }
        public string MemberPhoneNo { get; set; }
    }
}