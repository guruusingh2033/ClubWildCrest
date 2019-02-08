using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WildCrest.Models.WildCrestModels;

namespace WildCrest.Controllers.Admin
{
    public class UserStaysToAdminController : Controller
    {
        ClubWildCrestEntities context = new ClubWildCrestEntities();
        // GET: UserStaysToAdmin
        [Authorize(Roles = "2")]
        public ActionResult Index()
        {
            List<UserStays> usr = new List<UserStays>();           
            var data = context.tbl_UsersStay.ToList();
            foreach(var i in data)
            {
                int? TotalStays = context.tbl_MembershipPlans.SingleOrDefault(a => a.ID == i.MemberID).NoOfStays;
                int? pStay = context.tbl_UsersStay.Where(s => s.UserID == i.UserID && s.MemberID == i.MemberID).Count();
                int? lStay = 0;
                if ((TotalStays - pStay) > 0)
                {
                    lStay = TotalStays - pStay;
                }
                
                usr.Add(new UserStays()
                {
                    ID = i.ID,
                    UserID = i.UserID,
                    Username = context.tbl_Profile.SingleOrDefault(s => s.ID == i.UserID).F_Name + " " + context.tbl_Profile.SingleOrDefault(s => s.ID == i.UserID).L_Name,
                    CheckInDate = i.CheckInDate,
                    CheckoutDate = i.CheckoutDate,
                    PreviousStays = pStay,
                    LapseStays = lStay,
                    MembershipName = context.tbl_MembershipPlans.SingleOrDefault(a => a.ID == i.MemberID).PlanName,
                    MemberID=i.MemberID
                });
            }
            
            return View(usr.GroupBy(o => new { o.UserID, o.MemberID }).Select(o => o.FirstOrDefault()));
        }

        [HttpPost]
        public JsonResult SearchMemberByPhone(string phoneNo)
        {
            var data = context.tbl_Profile.SingleOrDefault(s => s.UserType == 3 && s.PhoneNo == phoneNo);
            if (data != null)
            {
                Profile prf = new Profile();
                prf.ID = data.ID; 
                prf.Address = data.Address;
                prf.City = data.City;
                prf.DOB = data.DOB;
                prf.Email = data.Email;
                prf.F_Name = data.F_Name;
                prf.L_Name = data.L_Name;
                prf.State = data.State;
                prf.PhoneNo = data.PhoneNo;
                var membership = context.tbl_UserMembership.Where(s => s.UserID == data.ID).Count();
                prf.UserMembershipCount = membership;
                return Json(prf);
            }
            else
            {
                return Json("notFound");
            }
        }

        [Authorize(Roles = "2")]
        public ActionResult AddCheckInOutDate(int usrid)
        {
            var data = context.tbl_Profile.SingleOrDefault(s => s.UserType == 3 && s.ID == usrid);
            Profile prf = new Profile();
            if (data != null)
            {                
                prf.ID = data.ID;
                prf.Address = data.Address;
                prf.City = data.City;
                prf.DOB = data.DOB;
                prf.Email = data.Email;
                prf.F_Name = data.F_Name;
                prf.L_Name = data.L_Name;
                prf.State = data.State;
                prf.PhoneNo = data.PhoneNo;

                List<UserStays> usr = new List<UserStays>();
                var stays = context.tbl_UsersStay.ToList();
                
                foreach (var i in stays)
                {
                    int? pStay = context.tbl_UsersStay.Where(s => s.UserID == usrid && s.MemberID== i.MemberID).Count();
                    
                    int? lStay = 0;
                    if (pStay != 0)
                    {
                        int? TotalStays = context.tbl_MembershipPlans.SingleOrDefault(a => a.ID == i.MemberID).NoOfStays;
                        if ((TotalStays - pStay) > 0)
                        {
                            lStay = TotalStays - pStay;
                        }
                        usr.Add(new UserStays()
                        {
                            ID = i.ID,
                            UserID = usrid,
                            Username = context.tbl_Profile.SingleOrDefault(s => s.ID == usrid).F_Name + " " + context.tbl_Profile.SingleOrDefault(s => s.ID == usrid).L_Name,
                            CheckInDate = i.CheckInDate,
                            CheckoutDate = i.CheckoutDate,
                            PreviousStays = pStay,
                            LapseStays = lStay,
                            MembershipName = context.tbl_MembershipPlans.SingleOrDefault(a => a.ID == i.MemberID).PlanName,
                            MemberID = i.MemberID
                        });
                    }
                    
                }
                if (usr.Count > 0)
                {
                    ViewBag.UserStays = usr.GroupBy(o => new { o.UserID, o.MemberID }).Select(o => o.FirstOrDefault());
                }
                else
                {
                    ViewBag.UserStays = null;
                }
                
                ViewBag.UserMembership = context.tbl_UserMembership.Where(m => m.UserID == usrid).ToList();
            }
                return View(prf);
        }

        [HttpPost]
        public JsonResult addStay(int UserID,int memberID,string checkIn,string checkOut)
        {
            tbl_UsersStay usrStay = new tbl_UsersStay();
            usrStay.UserID = UserID;
            usrStay.MemberID = memberID;
            usrStay.CheckInDate = checkIn;
            usrStay.CheckoutDate = checkOut;
            context.tbl_UsersStay.Add(usrStay);
            context.SaveChanges();
            return Json("Stay Added.");
        }
    }
}