using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WildCrest.Models.WildCrestModels;

namespace WildCrest.Controllers.SuperAdmin
{
    public class MembershipPlansController : Controller
    {
        ClubWildCrestEntities context = new ClubWildCrestEntities();
        // GET: MembershipPlans
        [Authorize(Roles = "1,2")]
        public ActionResult Index()
        {
            List<MembershipPlans> memberPlans = new List<MembershipPlans>();
            if (Request.Cookies["PageSetting"] != null)
            {
                if (Request.Cookies["PageSetting"]["MembershipPlanPermission"] != "None")
                {
                    var plans = context.tbl_MembershipPlans.Where(s => s.MemShipApprvFrmSuperAdm != false && s.IsDeleted != true).ToList();

                    foreach (var i in plans)
                    {
                        memberPlans.Add(new MembershipPlans()
                        {
                            ID = i.ID,
                            PlanName = i.PlanName,
                            PlanDetails = i.PlanDetails,
                            NoOfStays = i.NoOfStays,
                            AddedBy = i.AddedBy,
                            PlanAmount = i.PlanAmount
                        });
                    }
                    return View(memberPlans);
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                var plans = context.tbl_MembershipPlans.Where(s => s.MemShipApprvFrmSuperAdm != false && s.IsDeleted != true).ToList();

                foreach (var i in plans)
                {
                    memberPlans.Add(new MembershipPlans()
                    {
                        ID = i.ID,
                        PlanName = i.PlanName,
                        PlanDetails = i.PlanDetails,
                        NoOfStays = i.NoOfStays,
                        AddedBy = i.AddedBy,
                        PlanAmount = i.PlanAmount
                    });
                }
                return View(memberPlans);
            }
            
        }

        [Authorize(Roles = "1")]
        public ActionResult MemberShipForApproval()
        {
            var plans = context.tbl_MembershipPlans.Where(s => s.MemShipApprvFrmSuperAdm == false).ToList();
            List<MembershipPlans> memberPlans = new List<MembershipPlans>();
            foreach (var i in plans)
            {
                memberPlans.Add(new MembershipPlans()
                {
                    ID = i.ID,
                    PlanName = i.PlanName,
                    PlanDetails = i.PlanDetails,
                    NoOfStays = i.NoOfStays,
                    AddedBy=i.AddedBy,
                    PlanAmount = i.PlanAmount
                });
            }
            return View(memberPlans);
        }

        [Authorize(Roles = "1")]
        public ActionResult MemberShipForDeletion()
        {
            var plans = context.tbl_MembershipPlans.Where(s => s.IsDeleted == true).ToList();
            List<MembershipPlans> memberPlans = new List<MembershipPlans>();
            foreach (var i in plans)
            {
                memberPlans.Add(new MembershipPlans()
                {
                    ID = i.ID,
                    PlanName = i.PlanName,
                    PlanDetails = i.PlanDetails,
                    NoOfStays = i.NoOfStays,
                    DeletedBy=i.DeletedBy,
                    PlanAmount = i.PlanAmount
                });
            }
            return View(memberPlans);
        }

        [HttpPost]
        public JsonResult CancelDeleteMembershipById(int id)
        {
            var findUser = context.tbl_MembershipPlans.SingleOrDefault(u => u.ID == id);
            if (findUser != null)
            {
                findUser.IsDeleted = false;
                context.Entry(findUser).State = EntityState.Modified;
                context.SaveChanges();
            }
            return Json("NotDeleted.");
        }

        [HttpPost]
        public JsonResult ApproveMemberShipPlans(int id)
        {
            var data = context.tbl_MembershipPlans.Find(id);
            if (data != null)
            {
                data.MemShipApprvFrmSuperAdm = true;
                context.Entry(data).State = EntityState.Modified;
                context.SaveChanges();
            }
            return Json("Approved.");
        }

        [Authorize(Roles = "1,2")]
        public ActionResult AddNewPlan()
        {
            if (Request.Cookies["PageSetting"] != null)
            {
                if (Request.Cookies["PageSetting"]["MembershipPlanPermission"] == "All")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                return View();
            }
                   
        }
        [HttpPost]
        public JsonResult AddNewMembershipPlan(MembershipPlans newPlan)
        {
            int currentUserTypeLoggedin = Convert.ToInt32(Request.Cookies["UserType"].Value);
            tbl_MembershipPlans tblPlans = new tbl_MembershipPlans();
            tblPlans.PlanName = newPlan.PlanName;
            tblPlans.PlanDetails = newPlan.PlanDetails;
            tblPlans.NoOfStays = newPlan.NoOfStays;
            tblPlans.AddedBy= Request.Cookies["AddedBy"].Value;
            tblPlans.PlanAmount = newPlan.PlanAmount;
            tblPlans.MembershipPlanForYear = newPlan.MembershipPlanForYear;
            if (currentUserTypeLoggedin != 1)
            {
                tblPlans.MemShipApprvFrmSuperAdm = false;
            }
            else
            {
                tblPlans.MemShipApprvFrmSuperAdm = true;
            }
            tblPlans.IsDeleted = false;
            context.tbl_MembershipPlans.Add(tblPlans);
            context.SaveChanges();
            return Json(currentUserTypeLoggedin);
        }

        [Authorize(Roles = "1,2")]
        public ActionResult ViewEditPlanByID(int id, string editDetail)
        {
            
            if (Request.Cookies["PageSetting"] != null)
            {
                var data = context.tbl_MembershipPlans.SingleOrDefault(s => s.ID == id);
                MembershipPlans plan = new MembershipPlans();
                plan.ID = data.ID;
                plan.PlanName = data.PlanName;
                plan.PlanDetails = data.PlanDetails;
                plan.NoOfStays = data.NoOfStays;
                plan.PlanAmount = data.PlanAmount;
                plan.MembershipPlanForYear = data.MembershipPlanForYear;
                if (Request.Cookies["PageSetting"]["MembershipPlanPermission"] == "All")
                {
                    if (editDetail == "Details")
                    {
                        return View("~/Views/MembershipPlans/ViewEditPlanByID.cshtml", plan);
                    }
                    else
                    {
                        return View("~/Views/MembershipPlans/EditPlanByID.cshtml", plan);
                    }
                }
                else if (Request.Cookies["PageSetting"]["MembershipPlanPermission"] == "View")
                {
                    if (editDetail == "Details")
                    {
                        return View("~/Views/MembershipPlans/ViewEditPlanByID.cshtml", plan);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Login");
                    }
                }

                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                var data = context.tbl_MembershipPlans.SingleOrDefault(s => s.ID == id);
                MembershipPlans plan = new MembershipPlans();
                plan.ID = data.ID;
                plan.PlanName = data.PlanName;
                plan.PlanDetails = data.PlanDetails;
                plan.NoOfStays = data.NoOfStays;
                plan.PlanAmount = data.PlanAmount;
                plan.MembershipPlanForYear = data.MembershipPlanForYear;
                if (editDetail == "Details")
                {
                    return View("~/Views/MembershipPlans/ViewEditPlanByID.cshtml", plan);
                }
                else
                {
                    return View("~/Views/MembershipPlans/EditPlanByID.cshtml", plan);
                }
            }
                      
        }

        [Authorize(Roles = "1,2")]
        public ActionResult NewDelMembershipDetailByID(int id, string editDetail)
        {
            var data = context.tbl_MembershipPlans.SingleOrDefault(s => s.ID == id);
            MembershipPlans plan = new MembershipPlans();
            plan.ID = data.ID;
            plan.PlanName = data.PlanName;
            plan.PlanDetails = data.PlanDetails;
            plan.NoOfStays = data.NoOfStays;
            if (editDetail == "New")
            {
                ViewBag.NewDelPlan = true;
            }
            else
            {
                ViewBag.NewDelPlan = false;
            }
            return View(plan);
        }

        [HttpPost]
        public JsonResult EditMembershipPlan(MembershipPlans newPlan)
        {
            var data = context.tbl_MembershipPlans.SingleOrDefault(s => s.ID == newPlan.ID);
            if (data != null)
            {
                data.ID = newPlan.ID;
                data.PlanName = newPlan.PlanName;
                data.PlanDetails = newPlan.PlanDetails;
                data.NoOfStays = newPlan.NoOfStays;
                data.PlanAmount = newPlan.PlanAmount;
                data.MembershipPlanForYear = newPlan.MembershipPlanForYear;
                context.Entry(data).State = EntityState.Modified;
                context.SaveChanges();
            }
            return Json("Updated.");
        }

        [HttpPost]
        public JsonResult DeletePlanById(int id)
        {
            int currentUserTypeLoggedin = Convert.ToInt32(Request.Cookies["UserType"].Value);
            if (currentUserTypeLoggedin == 1)
            {
                var data = context.tbl_MembershipPlans.Find(id);
                var userMembership = context.tbl_UserMembership.Where(a => a.MembershipID == id).ToList();
                var userStay = context.tbl_UsersStay.Where(b => b.MemberID == id).ToList();
                if (data != null)
                {
                    foreach (var i in userMembership)
                    {
                        context.Entry(i).State = EntityState.Deleted;
                        context.SaveChanges();
                    }
                    foreach (var u in userStay)
                    {
                        var members = context.tbl_MembersWhileStayingWithUser.Where(m => m.UserStay_ID == u.ID).ToList();
                        foreach(var m in members)
                        {
                            context.Entry(m).State = EntityState.Deleted;
                            context.SaveChanges();
                        }
                        var foodOrders = context.tbl_UsersOrder.Where(m => m.UserStay_ID == u.ID).ToList();
                        foreach (var f in foodOrders)
                        {
                            context.Entry(f).State = EntityState.Deleted;
                            context.SaveChanges();
                        }
                        var billDet = context.tbl_BillingDetails.Where(m => m.UserStay_ID == u.ID).ToList();
                        foreach (var d in billDet)
                        {
                            context.Entry(d).State = EntityState.Deleted;
                            context.SaveChanges();
                        }
                        context.Entry(u).State = EntityState.Deleted;
                        context.SaveChanges();
                    }
                    context.Entry(data).State = EntityState.Deleted;
                    context.SaveChanges();
                }
            }
            else
            {
                var data = context.tbl_MembershipPlans.Find(id);
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
    }

}