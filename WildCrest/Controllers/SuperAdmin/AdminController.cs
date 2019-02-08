using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WildCrest.Models.WildCrestModels;

namespace WildCrest.Controllers.SuperAdmin
{
    public class AdminController : Controller
    {
        ClubWildCrestEntities context = new ClubWildCrestEntities();
        // GET: Admin
        [Authorize(Roles = "1")]
        public ActionResult Index()
        {
            List<Profile> lst = new List<Profile>();
            var adminList = context.tbl_Profile.OrderByDescending(a => a.ID).Where(s => s.UserType == 2).ToList();
            foreach (var i in adminList)
            {
                var staffSetting = context.tbl_PermissionsToStaff.Where(ss => ss.UserID == i.ID).ToList();
               
                lst.Add(new Profile()
                {
                    ID = i.ID,
                    F_Name = i.F_Name,
                    L_Name = i.L_Name,
                    Address = i.Address,
                    Email = i.Email,
                    PhoneNo = i.PhoneNo,
                    DOB = i.DOB,
                    City = i.City,
                    State = i.State,
                    EmailNotifications = i.EmailNotifications,
                    UserType = i.UserType,
                    Status = i.Status,
                    Password = i.Password,
                    AddedBy = i.AddedBy,
                    pageListTotal = staffSetting.Count()
                });

            }
            return View(lst);
        }

        [HttpGet]
        [Authorize(Roles = "1")]
        public ActionResult AddNewAdmin()
        {
            return View();
        }

        [HttpPost]
        public JsonResult DeleteAdminByID(int id)
        {
            var findUser = context.tbl_Profile.Find(id);
            var usrInMemberTbl = context.tbl_UserMembership.Where(s => s.UserID == id);
            if (findUser != null)
            {
                if (usrInMemberTbl != null)
                {
                    foreach (var i in usrInMemberTbl)
                    {
                        context.Entry(i).State = EntityState.Deleted;
                    }
                }
                var staffPermission = context.tbl_PermissionsToStaff.Where(p => p.UserID == id).ToList();
                if (staffPermission != null)
                {
                    foreach (var sp in staffPermission)
                    {
                        context.Entry(sp).State = EntityState.Deleted;
                    }
                }
                context.Entry(findUser).State = EntityState.Deleted;
                context.SaveChanges();
            }
            return Json("Deleted.");
        }

        //[HttpPost]       
        [Authorize(Roles = "1")]
        public ActionResult AdminDetailsByID(int id, string editDetail)
        {
            var data = context.tbl_Profile.SingleOrDefault(s => s.ID == id);
            Profile prf = new Profile();
            if (data != null)
            {
                prf.ID = data.ID;
                prf.AddedBy = data.AddedBy;
                prf.Address = data.Address;
                prf.City = data.City;
                prf.DOB = data.DOB;
                prf.Email = data.Email;
                prf.EmailNotifications = data.EmailNotifications;
                prf.F_Name = data.F_Name;
                prf.L_Name = data.L_Name;
                prf.Password = data.Password;
                prf.PhoneNo = data.PhoneNo;
                prf.State = data.State;
                prf.Status = data.Status;
            }
            if (editDetail == "Details")
            {
                return View("~/Views/Admin/AdminDetailsByID.cshtml", prf);
            }
            else
            {
                return View("~/Views/Admin/EditAdminByID.cshtml", prf);
            }
        }

        [HttpPost]
        public JsonResult UpdateDetailsByID(Profile prf)
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
                data.AddedBy = Request.Cookies["AddedBy"].Value;
                data.UserType = prf.UserType;
                data.DOB = prf.DOB;
                context.Entry(data).State = EntityState.Modified;
                context.SaveChanges();
            }
            return Json("Updated.");
        }

        [HttpPost]
        public JsonResult AddNewAdmin(Profile newAdmin)
        {
            tbl_Profile prf = new tbl_Profile();
            prf.F_Name = newAdmin.F_Name;
            prf.L_Name = newAdmin.L_Name;
            prf.Address = newAdmin.Address;
            prf.Email = newAdmin.Email;
            prf.PhoneNo = newAdmin.PhoneNo;
            prf.DOB = newAdmin.DOB;
            prf.City = newAdmin.City;
            prf.State = newAdmin.State;
            prf.Status = true;
            prf.EmailNotifications = newAdmin.EmailNotifications;
            prf.UserType = newAdmin.UserType;
            prf.Password = newAdmin.Password;
            prf.AddedBy = Request.Cookies["AddedBy"].Value;
            context.tbl_Profile.Add(prf);
            context.SaveChanges();
            return Json("Added.");
        }

        [Authorize(Roles = "1")]
        public ActionResult StaffSetting(int id)
        {
            ViewBag.UserIDForStaffSetting = id;
            return View();
        }

        [HttpPost]
        public JsonResult StaffSettingById(PermissionsToStaff pstaff)
        {
            tbl_PermissionsToStaff permission = new tbl_PermissionsToStaff();
            permission.UserID = pstaff.UserID;
            permission.MemberPermission = pstaff.MemberPermission;
            permission.MembershipPlanPermission = pstaff.MembershipPlanPermission;
            permission.EventsPermission = pstaff.EventsPermission;
            permission.VendorsPermission = pstaff.VendorsPermission;
            permission.MenuItemsPermission = pstaff.MenuItemsPermission;
            permission.FoodBillingEditPermission = pstaff.FoodBillingEditPermission;
            context.tbl_PermissionsToStaff.Add(permission);
            context.SaveChanges();
            return Json("set");
        }

        [Authorize(Roles = "1")]
        public ActionResult EditStaffSetting(int id)
        {
            var staffSetting = context.tbl_PermissionsToStaff.SingleOrDefault(ss => ss.UserID == id);
            PermissionsToStaff staff = new PermissionsToStaff();
            if (staffSetting != null)
            {
                staff.ID = staffSetting.ID;
                staff.UserID = staffSetting.UserID;
                staff.MemberPermission = staffSetting.MemberPermission;
                staff.MembershipPlanPermission = staffSetting.MembershipPlanPermission;
                staff.EventsPermission = staffSetting.EventsPermission;
                staff.VendorsPermission = staffSetting.VendorsPermission;
                staff.MenuItemsPermission = staffSetting.MenuItemsPermission;
                staff.FoodBillingEditPermission = staffSetting.FoodBillingEditPermission;
            }
            return View(staff);
        }

        [HttpPost]
        public JsonResult UpdatePermissionsById(PermissionsToStaff pstaff)
        {
            var data = context.tbl_PermissionsToStaff.Find(pstaff.ID);
            if (data != null)
            {
                data.UserID = pstaff.UserID;
                data.MemberPermission = pstaff.MemberPermission;
                data.MembershipPlanPermission = pstaff.MembershipPlanPermission;
                data.EventsPermission = pstaff.EventsPermission;
                data.VendorsPermission = pstaff.VendorsPermission;
                data.MenuItemsPermission = pstaff.MenuItemsPermission;
                data.FoodBillingEditPermission = pstaff.FoodBillingEditPermission;
                context.Entry(data).State = EntityState.Modified;
                context.SaveChanges();
            }
            
            return Json("set");
        }

        public JsonResult CheckUserNameDuplicasy(string email)
        {
            int i = context.tbl_Profile.Count(s => s.Email == email);
            return Json(i);
        }
    }
}