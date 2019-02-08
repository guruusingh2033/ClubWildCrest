using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WildCrest.Models.WildCrestModels;

namespace WildCrest.Controllers.Admin
{
    public class AdminProfileController : Controller
    {
        ClubWildCrestEntities context = new ClubWildCrestEntities();
        // GET: AdminProfile
        [Authorize(Roles = "2")]
        public ActionResult Index()
        {
            int id = Convert.ToInt32(Request.Cookies["UserID"].Value);
            var data = context.tbl_Profile.SingleOrDefault(s => s.ID == id);
            Profile profile = new Profile();
            profile.ID = data.ID;
            profile.F_Name = data.F_Name;
            profile.L_Name = data.L_Name;
            profile.Address = data.Address;
            profile.Email = data.Email;
            profile.PhoneNo = data.PhoneNo;
            profile.DOB = data.DOB;
            profile.City = data.City;
            profile.State = data.State;
            profile.Status = data.Status;
            profile.EmailNotifications = data.EmailNotifications;
            profile.Password = data.Password;
            return View(profile);
        }

        [Authorize(Roles = "2")]
        public ActionResult EditProfilebyID(int id)
        {
            var data = context.tbl_Profile.SingleOrDefault(s => s.ID == id);
            Profile profile = new Profile();
            if (data != null)
            {
                profile.ID = data.ID;
                profile.F_Name = data.F_Name;
                profile.L_Name = data.L_Name;
                profile.Address = data.Address;
                profile.Email = data.Email;
                profile.PhoneNo = data.PhoneNo;
                profile.DOB = data.DOB;
                profile.City = data.City;
                profile.State = data.State;
                profile.Status = data.Status;
                profile.EmailNotifications = data.EmailNotifications;
                profile.Password = data.Password;
            }
            return View(profile);
        }

        [HttpPost]
        public JsonResult UpdateProfileByID(Profile prf)
        {
            var data = context.tbl_Profile.Find(prf.ID);
            if (data != null)
            {
                data.F_Name = prf.F_Name;
                data.L_Name = prf.L_Name;
                data.Address = prf.Address;
                data.Email = prf.Email;
                data.PhoneNo = prf.PhoneNo;
                data.City = prf.City;
                data.Password = prf.Password;
                data.State = prf.State;
                data.EmailNotifications = prf.EmailNotifications;
                data.DOB = prf.DOB;
                context.Entry(data).State = EntityState.Modified;
                context.SaveChanges();

                //----------------------------------------------------------------------------------

                Response.Cookies["UserName"].Value = prf.Email.Trim();
                Response.Cookies["RememberUserName"].Value = prf.Email.Trim();
                Response.Cookies["RememberPassword"].Value = prf.Password.Trim();

                //----------------------------------------------------------------------------------
            }
            return Json("Updated.");
        }
    }
}