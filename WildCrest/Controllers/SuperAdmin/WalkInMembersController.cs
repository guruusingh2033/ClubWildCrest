using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WildCrest.Models.WildCrestModels;

namespace WildCrest.Controllers.SuperAdmin
{
    public class WalkInMembersController : Controller
    {
        ClubWildCrestEntities context = new ClubWildCrestEntities();
        // GET: WalkInMembers
        [Authorize(Roles = "1,2")]
        public ActionResult Index()
        {
            List<Profile> lst = new List<Profile>();
            var memberList = context.tbl_Profile.OrderByDescending(a => a.ID).Where(s => s.UserType == 3 && s.NewUsrBySuperApprv != false && s.DelUsrBySuperApprv != true && s.Walk_In_Member == true).ToList();
            foreach (var i in memberList)
            {
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
                    //Password = i.Password,
                    AddedBy = i.AddedBy,
                    NewUsrBySuperApprv = i.NewUsrBySuperApprv,
                    DelUsrBySuperApprv = i.DelUsrBySuperApprv
                });
            }
            return View(lst);
        }

        [Authorize(Roles = "1")]
        public ActionResult ApproveWalkInMembers()
        {
            List<Profile> lst = new List<Profile>();
            var memberList = context.tbl_Profile.OrderByDescending(a => a.ID).Where(s => s.UserType == 3 && s.NewUsrBySuperApprv == false && s.Walk_In_Member == true).ToList();
            foreach (var i in memberList)
            {
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
                    //Password = i.Password,
                    AddedBy = i.AddedBy,
                    NewUsrBySuperApprv = i.NewUsrBySuperApprv,
                    DelUsrBySuperApprv = i.DelUsrBySuperApprv
                });
            }
            return View(lst);
        }

        [Authorize(Roles = "1")]
        public ActionResult DeleteWalkInMembers()
        {
            List<Profile> lst = new List<Profile>();
            var memberList = context.tbl_Profile.OrderByDescending(a => a.ID).Where(s => s.UserType == 3 && s.DelUsrBySuperApprv == true && s.Walk_In_Member == true).ToList();
            foreach (var i in memberList)
            {
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
                    //Password = i.Password,
                    AddedBy = i.AddedBy,
                    NewUsrBySuperApprv = i.NewUsrBySuperApprv,
                    DelUsrBySuperApprv = i.DelUsrBySuperApprv,
                    DeletedBy=i.DeletedBy
                });
            }
            return View(lst);
        }

        [HttpPost]
        public JsonResult CancelDeleteWalkInMemberById(int id)
        {
            var findUser = context.tbl_Profile.SingleOrDefault(u => u.ID == id);
            if (findUser != null)
            {
                findUser.DelUsrBySuperApprv = false;
                context.Entry(findUser).State = EntityState.Modified;
                context.SaveChanges();
            }
            return Json("NotDeleted.");
        }

        [Authorize(Roles = "1,2")]
        public ActionResult Walk_In_Member()
        {
            var data = context.tbl_Profile.Where(s => s.UserType == 3 && s.Walk_In_Member != true).OrderBy(d=>d.F_Name).ToList();
            List<Profile> prf = new List<Profile>();
            foreach (var i in data)
            {
                prf.Add(new Profile()
                {
                    ID = i.ID,
                    F_Name = i.F_Name,
                    L_Name = i.L_Name

                });
            }
            ViewBag.ReferalUser = prf;
            return View();
        }

        [HttpPost]
        public JsonResult ApproveUser(int id)
        {
            var data = context.tbl_Profile.SingleOrDefault(s => s.ID == id);
            if (data != null)
            {
                data.Walk_In_Member = false;
                context.Entry(data).State = EntityState.Modified;
                context.SaveChanges();
            }
            return Json("Approved");
        }

        [HttpPost]
        public JsonResult DeleteWalkInUserById(int id)
        {
            int currentUserTypeLoggedin = Convert.ToInt32(Request.Cookies["UserType"].Value);
            var findUser = context.tbl_Profile.SingleOrDefault(u => u.ID == id);
            if (currentUserTypeLoggedin == 1)
            {
                if (findUser != null)
                {
                    var staydata = context.tbl_UsersStay.Where(s => s.UserID == id).ToList();
                    foreach (var i in staydata)
                    {
                        context.Entry(i).State = EntityState.Deleted;
                    }
                    context.Entry(findUser).State = EntityState.Deleted;
                    context.SaveChanges();
                }
            }
            else
            {
                if (findUser != null)
                {
                    findUser.DelUsrBySuperApprv = true;
                    findUser.DeletedBy= Request.Cookies["AddedBy"].Value;
                    context.Entry(findUser).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            return Json(currentUserTypeLoggedin);
        }

        [Authorize(Roles = "1,2")]
        public ActionResult EditWalkInUserByID(int id)
        {
            var data = context.tbl_Profile.SingleOrDefault(s => s.ID == id);
            var UserMembership = context.tbl_UserMembership.SingleOrDefault(a => a.UserID == id);
            Profile prf = new Profile();
            List<Profile> referenceNamesList = new List<Profile>();
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
                //prf.Password = data.Password;
                prf.PhoneNo = data.PhoneNo;
                prf.State = data.State;
                prf.Status = data.Status;
                prf.Reference_Of_Walk_In = data.Reference_Of_Walk_In;

                var referenceNames = context.tbl_Profile.Where(s => s.UserType == 3 && s.Walk_In_Member != true).OrderBy(d => d.F_Name).ToList();
                foreach (var i in referenceNames)
                {
                    referenceNamesList.Add(new Profile()
                    {
                        ID = i.ID,
                        F_Name = i.F_Name,
                        L_Name = i.L_Name

                    });
                }
            }
            ViewBag.ReferalUser = referenceNamesList;
            return View(prf);
        }

        [Authorize(Roles = "1,2")]
        public ActionResult DetailsWalkInUserByID(int id)
        {
            var data = context.tbl_Profile.SingleOrDefault(s => s.ID == id);
            var UserMembership = context.tbl_UserMembership.SingleOrDefault(a => a.UserID == id);
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
                //prf.Password = data.Password;
                prf.PhoneNo = data.PhoneNo;
                prf.State = data.State;
                prf.Status = data.Status;
                prf.Reference_Of_Walk_In = data.Reference_Of_Walk_In;

                List<RoomBooking_Details> usLst = new List<RoomBooking_Details>();
                var stayData = context.tbl_RoomBooking.Where(a => a.UserID == id).ToList();

                if (stayData.Count > 0)
                {

                    var date = DateTime.Today;
                    string DateFormat = date.ToString(@"MM\/dd\/yyyy");
                    foreach (var i in stayData)
                    {
                        string rooms = "";
                        string[] arrDate = i.Check_Out.Split('/');
                        //var tempDate = arrDate[1] + "/" + arrDate[0] + "/" + arrDate[2];
                        DateTime checkoutDate = new DateTime(Convert.ToInt32(arrDate[2]), Convert.ToInt32(arrDate[0]), Convert.ToInt32(arrDate[1]));

                        if (checkoutDate >= DateTime.Today)
                        {
                            var stayDataDetails = context.tbl_RoomBooking_Details.Where(a => a.Booking_ID == i.Booking_ID).ToList();
                            foreach (var str in stayDataDetails)
                            {
                                rooms += context.tbl_Rooms.SingleOrDefault(r => r.ID == str.RoomID).RoomNo + ",";
                            }

                            if (rooms.Length > 1)
                                rooms = rooms.Substring(0, rooms.Length - 1);

                            usLst.Add(new RoomBooking_Details()
                            {
                                RoomNo = rooms,
                                Check_In = i.Check_In,
                                Check_Out = i.Check_Out

                            });
                        }
                    }
                }
                ViewBag.WalkInStayDetails = usLst;

                List<MenusBillingSection> reslist = new List<MenusBillingSection>();

                var restaurant = context.tbl_MenusBillingSection.Where(u => u.UserID == id).ToList();
                if (restaurant.Count > 0)
                {

                    foreach (var r in restaurant)
                    {
                        List<MenusBillingDetailsWithBillNo> resDetailsList = new List<MenusBillingDetailsWithBillNo>();
                        var restaurantDetails = context.tbl_MenusBillingDetailsWithBillNo.Where(u => u.BillNo == r.Bill_Number).ToList();
                        foreach (var rd in restaurantDetails)
                        {
                            resDetailsList.Add(new MenusBillingDetailsWithBillNo()
                            {
                                ID = rd.ID,
                                BillNo = rd.BillNo,
                                FoodName = rd.FoodName,
                                Quantity = rd.Quantity
                            });
                        }
                        reslist.Add(new MenusBillingSection()
                        {
                            PaymentDate = r.PaymentDate,
                            //FoodItemName = r.FoodItemName,
                            //Quantity = r.Quantity,
                            MenusBillingDetailsWithBillNo = resDetailsList
                        });
                    }
                }

                ViewBag.FoodBillingDetails = reslist;
            }
            return View(prf);
        }

        [Authorize(Roles = "1,2")]
        public ActionResult NewDelMemberDetailsByID(int id,string editDetail)
        {
            var data = context.tbl_Profile.SingleOrDefault(s => s.ID == id);
            var UserMembership = context.tbl_UserMembership.SingleOrDefault(a => a.UserID == id);
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
                //prf.Password = data.Password;
                prf.PhoneNo = data.PhoneNo;
                prf.State = data.State;
                prf.Status = data.Status;
                prf.Reference_Of_Walk_In = data.Reference_Of_Walk_In;
                
                List<RoomBooking_Details> usLst = new List<RoomBooking_Details>();
                var stayData = context.tbl_RoomBooking.Where(a => a.UserID == id).ToList();

                if (stayData.Count > 0)
                {

                    var date = DateTime.Today;
                    string DateFormat = date.ToString(@"MM\/dd\/yyyy");
                    foreach (var i in stayData)
                    {
                        string rooms = "";
                        string[] arrDate = i.Check_Out.Split('/');
                        //var tempDate = arrDate[1] + "/" + arrDate[0] + "/" + arrDate[2];
                        DateTime checkoutDate = new DateTime(Convert.ToInt32(arrDate[2]), Convert.ToInt32(arrDate[0]), Convert.ToInt32(arrDate[1]));

                        if (checkoutDate >= DateTime.Today)
                        {
                            var stayDataDetails = context.tbl_RoomBooking_Details.Where(a => a.Booking_ID == i.Booking_ID).ToList();
                            foreach (var str in stayDataDetails)
                            {
                                rooms += context.tbl_Rooms.SingleOrDefault(r => r.ID == str.RoomID).RoomNo + ",";
                            }

                            if (rooms.Length > 1)
                                rooms = rooms.Substring(0, rooms.Length - 1);

                            usLst.Add(new RoomBooking_Details()
                            {
                                RoomNo = rooms,
                                Check_In = i.Check_In,
                                Check_Out = i.Check_Out

                            });
                        }
                    }
                }
                ViewBag.WalkInStayDetails = usLst;

                List<MenusBillingSection> reslist = new List<MenusBillingSection>();

                var restaurant = context.tbl_MenusBillingSection.Where(u => u.UserID == id).ToList();
                if (restaurant.Count > 0)
                {
                    foreach (var r in restaurant)
                    {
                        List<MenusBillingDetailsWithBillNo> resDetailsList = new List<MenusBillingDetailsWithBillNo>();
                        var restaurantDetails = context.tbl_MenusBillingDetailsWithBillNo.Where(u => u.BillNo == r.Bill_Number).ToList();
                        foreach (var rd in restaurantDetails)
                        {
                            resDetailsList.Add(new MenusBillingDetailsWithBillNo()
                            {
                                ID = rd.ID,
                                BillNo = rd.BillNo,
                                FoodName = rd.FoodName,
                                Quantity = rd.Quantity
                            });
                        }
                        reslist.Add(new MenusBillingSection()
                        {
                            PaymentDate = r.PaymentDate,

                            MenusBillingDetailsWithBillNo = resDetailsList
                        });
                    }
                }

                ViewBag.FoodBillingDetails = reslist;
            }
            if (editDetail == "New")
            {
                ViewBag.NewDelWalkIn = true;
            }
            else
            {
                ViewBag.NewDelWalkIn = false;
            }
            return View(prf);
        }

        [HttpPost]
        public JsonResult UpdateWalkinMemberByID(Profile prf)
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
                //data.Password = prf.Password;
                data.State = prf.State;
                data.AddedBy = Request.Cookies["AddedBy"].Value;
                data.UserType = prf.UserType;
                data.EmailNotifications = prf.EmailNotifications;
                data.Reference_Of_Walk_In = prf.Reference_Of_Walk_In;
                data.DOB = prf.DOB;
                context.Entry(data).State = EntityState.Modified;
                context.SaveChanges();
            }
            return Json("Updated.");
        }

        [HttpPost]
        public JsonResult AddNewMember(Profile newMember)
        {
            int currentUserTypeLoggedin = Convert.ToInt32(Request.Cookies["UserType"].Value);
            tbl_Profile prf = new tbl_Profile();
            prf.F_Name = newMember.F_Name;
            prf.L_Name = newMember.L_Name;
            prf.Address = newMember.Address;
            prf.Email = newMember.Email;
            prf.PhoneNo = newMember.PhoneNo;
            prf.DOB = newMember.DOB;
            prf.City = newMember.City;
            prf.State = newMember.State;
            prf.Status = true;
            prf.EmailNotifications = newMember.EmailNotifications;
            prf.UserType = newMember.UserType;
            //prf.Password = newMember.Password;
            prf.AddedBy = Request.Cookies["AddedBy"].Value;
            prf.Walk_In_Member = true;
            //if (currentUserTypeLoggedin == 2)
            //{
            //    prf.NewUsrBySuperApprv = false;
            //}
            //else
            //{
                prf.NewUsrBySuperApprv = true;
            //}
            prf.DelUsrBySuperApprv = false;
            prf.Reference_Of_Walk_In = newMember.Reference_Of_Walk_In;
            context.tbl_Profile.Add(prf);
            context.SaveChanges();
            int userId = prf.ID;
            var jsonResult = new
            {
                userId = userId,
                currentUserTypeLoggedin = currentUserTypeLoggedin
            };

            return Json(jsonResult);
        }

        [HttpPost]
        public JsonResult ApproveWalkin(int id)
        {
            var data = context.tbl_Profile.SingleOrDefault(s => s.ID == id);
            if (data != null)
            {
                data.NewUsrBySuperApprv = true;
                context.Entry(data).State = EntityState.Modified;
                context.SaveChanges();
            }
            return Json("Approved");
        }

        public ActionResult RoomBooking(int id)
        {
            ViewBag.WalkinID = id;
            return View();
        }

        public ActionResult FoodBilling(int id)
        {
            var data = context.tbl_MenusBillingSection.ToList().LastOrDefault();
            if (data != null)
            {
                ViewBag.BillNumber = data.Bill_Number + 1;
            }
            else
            {
                ViewBag.BillNumber = 1;
            }

            var data1 = context.tbl_Profile.Find(id);
            Profile prf = new Profile();
            if (data1 != null)
            {
                prf.F_Name = data1.F_Name;
                prf.ID = data1.ID;
                prf.PhoneNo = data1.PhoneNo;
                prf.L_Name = data1.L_Name;
            }
            return View(prf);
        }
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    