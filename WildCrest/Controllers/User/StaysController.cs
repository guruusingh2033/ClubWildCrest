using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WildCrest.Models.WildCrestModels;

namespace WildCrest.Controllers.User
{
    public class StaysController : Controller
    {
        ClubWildCrestEntities context = new ClubWildCrestEntities();
        // GET: Stays
        [Authorize(Roles = "3")]
        public ActionResult Index()
        {
            int usrID = Convert.ToInt32(Request.Cookies["UserID"].Value);
            var a = context.tbl_UserMembership.SingleOrDefault(u => u.UserID == usrID);
            UserStays us = new UserStays();
            if (a != null)
            {
                us.MembershipName = context.tbl_MembershipPlans.SingleOrDefault(m => m.ID == a.MembershipID).PlanName;
                var data = context.tbl_UsersStay.Where(d => d.UserID == usrID && d.MemberID == a.MembershipID).ToList();
                int? TotalStays = context.tbl_MembershipPlans.SingleOrDefault(t => t.ID == a.MembershipID).NoOfStays;
                int? pStay = data.Count();
                int? lStay = 0;
                if ((TotalStays - pStay) > 0)
                {
                    lStay = TotalStays - pStay;
                }
                us.TotalStays = TotalStays;
                us.PreviousStays = pStay;
                us.LapseStays = lStay;

                List<UserStays> usr = new List<UserStays>();
                foreach (var i in data)
                {
                    usr.Add(new UserStays()
                    {
                        ID = i.ID,
                        CheckInDate = i.CheckInDate,
                        CheckoutDate = i.CheckoutDate,
                        NoOfMembers = i.NoOfMembers
                        //NoOfMembers = context.tbl_MembersWhileStayingWithUser.Where(m => m.UserStay_ID == i.ID).Count()
                    });
                }
                ViewBag.UserStays = usr;
            }
            else
            {
                us = null;
            }
            return View(us);
        }
    }
}