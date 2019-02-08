using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WildCrest.Models.WildCrestModels;

namespace WildCrest.Controllers.User
{
    public class RoomBookingController : Controller
    {
        ClubWildCrestEntities context = new ClubWildCrestEntities();
        // GET: RoomBooking
        public ActionResult Index()
        {
            int currentUserId = Convert.ToInt32(Request.Cookies["UserID"].Value);
            int? TotalStays = 0;
            int? pStay = 0;
            int? lStay = 0;
            var usermember = context.tbl_UserMembership.SingleOrDefault(s => s.UserID == currentUserId);
            if (usermember != null)
            {
                var userstay = context.tbl_UsersStay.Where(a => a.UserID == currentUserId && a.MemberID == usermember.MembershipID).ToList();

                TotalStays = context.tbl_MembershipPlans.SingleOrDefault(a => a.ID == usermember.MembershipID).NoOfStays;
                pStay = userstay.Count();
                if ((TotalStays - pStay) > 0)
                {
                    lStay = TotalStays - pStay;
                }
                ViewBag.MembershipID = usermember.MembershipID;
            }
            else
            {
                ViewBag.MembershipID = 0;
            }
            ViewBag.PreviousStays = pStay;
            ViewBag.LapseStays = lStay;
            
            return View();
        }

        [HttpPost]
        public JsonResult checkRoomsAvailablity(string checkIn, string checkOut)
        {            
            var data = getAvailableRoomsList(checkIn, checkOut);
            //TempData["AvailableRoomsList"] = lst;
            return Json(data);
        }

        public List<Rooms> getAvailableRoomsList(string checkIn, string checkOut)
        {
            //var query = "SELECT r.* FROM tbl_Rooms r WHERE NOT EXISTS(SELECT 1 FROM tbl_RoomBooking b WHERE b.RoomID = r.ID AND (('" + checkIn + "' >= b.Check_In AND '" + checkIn + "' <= b.Check_Out) OR ('" + checkIn + "' <= b.Check_In AND '" + checkOut + "' >= b.Check_Out)))";
            var query = "SELECT r.* FROM tbl_Rooms r WHERE NOT EXISTS(SELECT 1 FROM tbl_UsersStay b WHERE b.RoomID=r.ID and (('" + checkIn + "' >= b.CheckInDate AND '" + checkIn + "' <= b.CheckoutDate) OR ('" + checkIn + "' <= b.CheckInDate AND '" + checkOut + "' >= b.CheckoutDate)))";
            List<Rooms> lst = new List<Rooms>();
            var data = context.Database.SqlQuery<tbl_Rooms>(query);
            if (data != null)
            {
                foreach (var i in data)
                {
                    lst.Add(new Rooms()
                    {
                        ID = i.ID,
                        RoomCost = i.RoomCost,
                        Room_Type = i.Room_Type,
                        RoomNo = i.RoomNo
                    });
                }
            }
            return lst;
        }

        public ActionResult AdvancedBooking(string checkIn, string checkOut,int membershipID)
        {
            //var data = TempData["AvailableRoomsList"] as List<Rooms>;
            var data = getAvailableRoomsList(checkIn, checkOut);
            ViewBag.CheckINDate = checkIn;
            ViewBag.CheckOutDate = checkOut;
            ViewBag.MembershipID = membershipID;
            return View(data);
        }

        public JsonResult BookNow(int roomId,string checkin,string checkout,int membershipID)
        {
            int currentUserId= Convert.ToInt32(Request.Cookies["UserID"].Value);
            tbl_UsersStay booking = new tbl_UsersStay();
            booking.CheckInDate = checkin;
            booking.CheckoutDate = checkout;
            booking.UserID = currentUserId;
            booking.RoomID = roomId;
            if (membershipID != 0)
                booking.MemberID = membershipID;
            else
                booking.MemberID = null;
            context.tbl_UsersStay.Add(booking);
            context.SaveChanges();
            return Json("Booked");
        }
    }
}