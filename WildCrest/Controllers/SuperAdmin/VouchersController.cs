using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WildCrest.Models.WildCrestModels;

namespace WildCrest.Controllers.SuperAdmin
{
    public class VouchersController : Controller
    {
        ClubWildCrestEntities context = new ClubWildCrestEntities();
        // GET: Vouchers
        [Authorize(Roles = "1,2")]
        public ActionResult Index()
        {
            var data = context.tbl_Voucher.Where(s => s.VoucherForApprvFrmSuperAdm != false && s.IsDeleted != true).ToList();
            List<Voucher> voucher = new List<Voucher>();
            foreach (var i in data)
            {
                voucher.Add(new Voucher()
                {
                    ID = i.ID,
                    VoucherName = i.VoucherName,
                    VoucherDetails = i.VoucherDetails,
                    AddedBy = i.AddedBy
                });
            }
            return View(voucher);
        }

        [Authorize(Roles = "1")]
        public ActionResult VouchersForApproval()
        {
            var data = context.tbl_Voucher.Where(s => s.VoucherForApprvFrmSuperAdm == false).ToList();
            List<Voucher> voucher = new List<Voucher>();
            foreach (var i in data)
            {
                voucher.Add(new Voucher()
                {
                    ID = i.ID,
                    VoucherName = i.VoucherName,
                    VoucherDetails = i.VoucherDetails,
                    AddedBy = i.AddedBy
                });
            }
            return View(voucher);
        }

        [Authorize(Roles = "1,2")]
        public ActionResult AddNewVoucher()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CreateNewVoucher(Voucher voucher)
        {
            int currentUserTypeLoggedin = Convert.ToInt32(Request.Cookies["UserType"].Value);
            tbl_Voucher tblVoucher = new tbl_Voucher();
            tblVoucher.VoucherName = voucher.VoucherName;
            tblVoucher.VoucherDetails = voucher.VoucherDetails;
            tblVoucher.AddedBy = Request.Cookies["AddedBy"].Value;
            if (currentUserTypeLoggedin != 1)
            {
                tblVoucher.VoucherForApprvFrmSuperAdm = false;
            }
            else
            {
                tblVoucher.VoucherForApprvFrmSuperAdm = true;
            }
            tblVoucher.IsDeleted = false;
            context.tbl_Voucher.Add(tblVoucher);
            context.SaveChanges();
            return Json(currentUserTypeLoggedin);
        }

        [HttpPost]
        public JsonResult ApproveVoucherById(int id)
        {
            var data = context.tbl_Voucher.Find(id);
            if (data != null)
            {
                data.VoucherForApprvFrmSuperAdm = true;
                context.Entry(data).State = EntityState.Modified;
                context.SaveChanges();
            }
            return Json("Approved.");
        }

        [Authorize(Roles = "1,2")]
        public ActionResult VoucherDetailsByID(int id, string editDetail)
        {
            var data = context.tbl_Voucher.SingleOrDefault(s => s.ID == id);
            Voucher voucher = new Voucher();
            voucher.ID = data.ID;
            voucher.VoucherName = data.VoucherName;
            voucher.VoucherDetails = data.VoucherDetails;
            voucher.AddedBy = data.AddedBy;
            if (editDetail == "Details")
            {
                return View("~/Views/Vouchers/ViewVoucher.cshtml", voucher);
            }
            else
            {
                return View("~/Views/Vouchers/EditVoucherDetails.cshtml", voucher);
            }
        }

        [Authorize(Roles = "1,2")]
        public ActionResult NewDelVoucherDetailsByID(int id, string editDetail)
        {
            var data = context.tbl_Voucher.SingleOrDefault(s => s.ID == id);
            Voucher voucher = new Voucher();
            voucher.ID = data.ID;
            voucher.VoucherName = data.VoucherName;
            voucher.VoucherDetails = data.VoucherDetails;
            voucher.AddedBy = data.AddedBy;
            if (editDetail == "New")
            {
                ViewBag.NewDelVoucher = true;
            }
            else
            {
                ViewBag.NewDelVoucher = false;
            }
            return View(voucher);
        }

        [HttpPost]
        public JsonResult UpdateVoucherById(Voucher updVoucher)
        {
            var data = context.tbl_Voucher.Find(updVoucher.ID);
            if (data != null)
            {
                data.VoucherName = updVoucher.VoucherName;
                data.VoucherDetails = updVoucher.VoucherDetails;
                context.Entry(data).State = EntityState.Modified;
                context.SaveChanges();
            }
            return Json("Updated.");
        }

        [HttpPost]
        public JsonResult DeleteVoucherById(int id)
        {
            int currentUserTypeLoggedin = Convert.ToInt32(Request.Cookies["UserType"].Value);
            if (currentUserTypeLoggedin == 1)
            {
                var data = context.tbl_Voucher.Find(id);
                if (data != null)
                {
                    var userVoucher = context.tbl_VoucherUsedByUser.Where(u => u.VoucherID == id).ToList();
                    if (userVoucher != null)
                    {
                        foreach (var uv in userVoucher)
                        {
                            context.Entry(uv).State = EntityState.Deleted;
                            context.SaveChanges();
                        }
                    }
                    context.Entry(data).State = EntityState.Deleted;
                    context.SaveChanges();
                }
            }
            else
            {
                var data = context.tbl_Voucher.Find(id);
                if (data != null)
                {
                    data.IsDeleted = true;
                    data.DeletedBy= Request.Cookies["AddedBy"].Value;
                    context.Entry(data).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            return Json(currentUserTypeLoggedin);
        }

        [HttpPost]
        public JsonResult sendVouchersToAll(int voucherID)
        {
            var voucher = context.tbl_Voucher.Find(voucherID);
            var users = context.tbl_Profile.Where(s => s.UserType == 3 && s.EmailNotifications == true).ToList();
            if (voucher != null)
            {
                string body = "<b>Voucher Details :</b><br>" + voucher.VoucherDetails;
                string from = "EmailID_Of_Sender@gmail.com";

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from);
                foreach (var multi in users)
                {
                    mail.To.Add(new MailAddress(multi.Email));
                }
                SmtpClient client = new SmtpClient();
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = "smtp.gmail.com";
                mail.Subject = voucher.VoucherName;
                mail.IsBodyHtml = true;
                client.EnableSsl = true;
                client.Credentials = new System.Net.NetworkCredential("EmailID_Of_Sender", "Password_Of_Sender");
                client.Port = 587;
                mail.Body = body;
                //client.Send(mail);
            }
            return Json("Sent.");
        }

        [HttpPost]
        public JsonResult sendVouchersToMultipleGroups(int voucherID, List<int> grpList)
        {
            var voucher = context.tbl_Voucher.Find(voucherID);
            MailMessage mail = new MailMessage();
            for (var i = 0; i < grpList.Count(); i++)
            {
                int? id = grpList[i];
                var getUserFromGrp = context.tbl_UserGroup.Where(g => g.GroupID == id).ToList();
                foreach (var email in getUserFromGrp)
                {
                    var eml = context.tbl_Profile.SingleOrDefault(e => e.ID == email.UserID).Email;
                    mail.To.Add(new MailAddress(eml));
                }
            }
            string body = "<b>Voucher Details :</b><br>" + voucher.VoucherDetails;
            string from = "EmailID_Of_Sender@gmail.com";


            mail.From = new MailAddress(from);

            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "smtp.gmail.com";
            mail.Subject = voucher.VoucherName;
            mail.IsBodyHtml = true;
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("EmailID_Of_Sender", "Password_Of_Sender");
            client.Port = 587;
            mail.Body = body;
            //client.Send(mail);

            return Json("Sent.");
        }

        [HttpPost]
        public JsonResult sendVouchersToMultipleUsers(int voucherID, List<int> usrList)
        {
            var voucher = context.tbl_Voucher.Find(voucherID);
            MailMessage mail = new MailMessage();
            for (var i = 0; i < usrList.Count(); i++)
            {
                int? id = usrList[i];
                var eml = context.tbl_Profile.SingleOrDefault(e => e.ID == id && e.EmailNotifications == true);
                if (eml.Email != null)
                    mail.To.Add(new MailAddress(eml.Email));
            }
            string body = "<b>Voucher Details :</b><br>" + voucher.VoucherDetails;
            string from = "EmailID_Of_Sender@gmail.com";

            mail.From = new MailAddress(from);
            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "smtp.gmail.com";
            mail.Subject = voucher.VoucherName;
            mail.IsBodyHtml = true;
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("EmailID_Of_Sender", "Password_Of_Sender");
            client.Port = 587;
            mail.Body = body;
            //client.Send(mail);

            return Json("Sent.");
        }

        [Authorize(Roles = "1,2")]
        public ActionResult UserWithVoucher()
        {
            var data = context.tbl_Voucher.Where(s => s.VoucherForApprvFrmSuperAdm != false && s.IsDeleted != true).ToList();
            List<Voucher> lst = new List<Voucher>();
            if (data != null)
            {
                foreach (var i in data)
                {
                    lst.Add(new Voucher()
                    {
                        ID = i.ID,
                        VoucherName = i.VoucherName
                    });
                }
            }
            var userData = context.tbl_Profile.Where(ud => ud.UserType == 3 && ud.NewUsrBySuperApprv != false && ud.DelUsrBySuperApprv != true).ToList();
            List<Voucher> usrlst = new List<Voucher>();
            if (userData != null)
            {
                foreach (var usr in userData)
                {
                    usrlst.Add(new Voucher()
                    {
                        ID = usr.ID,
                        VoucherName = usr.F_Name + " " + usr.L_Name
                    });
                }
            }
            ViewBag.UserProfile = usrlst;
            return View(lst);
        }

        [HttpPost]
        public JsonResult AddVoucherDetailsWithUser(VoucherUsedByUser userVoucher)
        {
            var check = context.tbl_VoucherUsedByUser.SingleOrDefault(s => s.UserID == userVoucher.UserID && s.VoucherID == userVoucher.VoucherID);
            if (check == null)
            {
                tbl_VoucherUsedByUser tbl = new tbl_VoucherUsedByUser();
                tbl.VoucherID = userVoucher.VoucherID;
                tbl.UserID = userVoucher.UserID;
                context.tbl_VoucherUsedByUser.Add(tbl);
                context.SaveChanges();
                return Json("Saved");
            }
            else
            {
                return Json("AlreadyExist.");
            }

        }

        [Authorize(Roles = "1")]
        public ActionResult VouchersForDeletion()
        {
            var data = context.tbl_Voucher.Where(s => s.IsDeleted == true).ToList();
            List<Voucher> voucher = new List<Voucher>();
            foreach (var i in data)
            {
                voucher.Add(new Voucher()
                {
                    ID = i.ID,
                    VoucherName = i.VoucherName,
                    VoucherDetails = i.VoucherDetails,
                    DeletedBy = i.DeletedBy
                });
            }
            return View(voucher);
        }

        [Authorize(Roles = "1,2")]
        public ActionResult SendVouchers(int voucherID)
        {
            var voucherName = context.tbl_Voucher.SingleOrDefault(v => v.ID == voucherID).VoucherName;
            var groupNames = context.tbl_Groups.OrderBy(t => t.GroupName).ToList();
            List<MemberGroup> grpList = new List<MemberGroup>();
            if (groupNames != null)
            {
                foreach (var g in groupNames)
                {
                    grpList.Add(new MemberGroup()
                    {
                        ID = g.ID,
                        GroupName = g.GroupName
                    });
                }

            }
            ViewBag.GroupList = grpList;
            ViewBag.voucherID = voucherID;
            ViewBag.voucherName = voucherName;

            var users = context.tbl_Profile.Where(s => s.UserType == 3 && s.EmailNotifications == true).OrderBy(d => d.F_Name).ToList();
            List<Profile> prf = new List<Profile>();
            foreach (var i in users)
            {
                prf.Add(new Profile()
                {
                    ID = i.ID,
                    F_Name = i.F_Name,
                    L_Name = i.L_Name
                });
            }
            ViewBag.UsersList = prf;
            return View();
        }
    }
}