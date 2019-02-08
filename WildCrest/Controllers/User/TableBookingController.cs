using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WildCrest.Models.WildCrestModels;

namespace WildCrest.Controllers.User
{
    public class TableBookingController : Controller
    {
        ClubWildCrestEntities context = new ClubWildCrestEntities();
        // GET: TableBooking
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult checkTablesAvailablity(string TimeForBooking)
        {
            var list = getAvailableTablesList(TimeForBooking);
            return Json(list);
        }

        public List<TablesForBooking> getAvailableTablesList(string TimeForBooking)
        {
            //var date = DateTime.Today.ToShortDateString();
            var DateFormat = DateTime.Today;
            //string date = DateFormat.ToString("MM/dd/yyyy");
            string date = DateFormat.ToString(@"MM\/dd\/yyyy");
            var query = "SELECT r.* FROM tbl_TablesForBooking r WHERE NOT EXISTS ( SELECT 1 FROM tbl_BookingTable b WHERE b.TableID = r.ID and b.BookingDate = '" + date + "' AND (b.BookingEndTime > '" + TimeForBooking + "'))";
            List<TablesForBooking> lst = new List<TablesForBooking>();
            var data = context.Database.SqlQuery<tbl_TablesForBooking>(query);
            if (data != null)
            {
                foreach (var i in data)
                {
                    lst.Add(new TablesForBooking()
                    {
                        ID = i.ID,
                        TableNo=i.TableNo
                    });
                }
            }
            return lst;
        }

        public ActionResult AdvancedBookingofTable(string TimeForBooking)
        {
            var data = getAvailableTablesList(TimeForBooking);
            ViewBag.TimeForBooking = TimeForBooking;
            return View(data);
        }

        public JsonResult BookTableNow(int tableId,string TimeForBooking)
        {
            //var date = DateTime.Today.ToShortDateString();
            var DateFormat = DateTime.Today;
            //string date = DateFormat.ToString("MM/dd/yyyy");
            string date = DateFormat.ToString(@"MM\/dd\/yyyy");
            int currentUserId = Convert.ToInt32(Request.Cookies["UserID"].Value);
            tbl_BookingTable booking = new tbl_BookingTable();
            booking.UserID = currentUserId;
            booking.TableID = tableId;
            booking.BookingDate = date;
            booking.BookingStartTime = TimeForBooking;
            booking.BookingEndTime = (Convert.ToDecimal(TimeForBooking) + Convert.ToDecimal("02.00")).ToString();

            context.tbl_BookingTable.Add(booking);
            context.SaveChanges();
            return Json("booked");
        }
    }
}