using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WildCrest.Models.WildCrestModels;

namespace WildCrest.Controllers.User
{
    public class MembershipDetailsController : Controller
    {
        ClubWildCrestEntities context = new ClubWildCrestEntities();
        // GET: MembershipDetails
        [Authorize(Roles = "3")]
        public ActionResult Index()
        {
            var plans = context.tbl_MembershipPlans.ToList();
            List<MembershipPlans> memberPlans = new List<MembershipPlans>();
            foreach (var i in plans)
            {
                memberPlans.Add(new MembershipPlans()
                {
                    ID = i.ID,
                    PlanName = i.PlanName,
                    PlanDetails = i.PlanDetails,
                    NoOfStays = i.NoOfStays
                });
            }
            return View(memberPlans);
        }

        [HttpPost]
        public JsonResult BuyMembership(int id)
        {
            tbl_UserMembership usrMemberShip = new tbl_UserMembership();
            usrMemberShip.MembershipID = id;
            usrMemberShip.UserID = Convert.ToInt32(Request.Cookies["UserID"].Value);
            context.tbl_UserMembership.Add(usrMemberShip);
            context.SaveChanges();
            return Json("MembershipSelected.");
        }

        [Authorize(Roles = "3")]
        public ActionResult ViewSelectedMembership()
        {
            int id = Convert.ToInt32(Request.Cookies["UserID"].Value);
            var data = context.tbl_UserMembership.Where(s => s.UserID == id).ToList();
            List<UserMembership> usrMemList = new List<UserMembership>();
            foreach (var i in data)
            {
                usrMemList.Add(new UserMembership()
                {
                    ID = i.ID,
                    UserID = i.UserID,
                    MembershipID = i.MembershipID,
                    PlanName = context.tbl_MembershipPlans.FirstOrDefault(s => s.ID == i.MembershipID).PlanName,
                    PlanDetails = context.tbl_MembershipPlans.FirstOrDefault(s => s.ID == i.MembershipID).PlanDetails,
                    NoOfStays = context.tbl_MembershipPlans.FirstOrDefault(s => s.ID == i.MembershipID).NoOfStays
                });
            }            
            return View(usrMemList);
        }
    }
}