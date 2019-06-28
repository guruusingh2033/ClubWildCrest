using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WildCrest.Models.WildCrestModels;

namespace WildCrest.Controllers.EntryMembers
{
    public class EntryController : Controller
    {
        //
        // GET: /Entry/
        ClubWildCrestEntities context = new ClubWildCrestEntities();
        [Authorize(Roles = "1,2,4,5")]
        public ActionResult AddNewEntry()
        {
            return View();
        }
        public JsonResult AddEntry(tbl_EntryMembers model)
        {
            if (model != null&&ModelState.IsValid)
            {
                context.tbl_EntryMembers.Add(model);
                context.SaveChanges();
                return Json(model.ID);
            }
            return Json("not saved");
        }
        [Authorize(Roles = "1,2,4,5")]
        public ActionResult EntryBillsIndex(string filter = "", string filterFromReport = "")
        {
            //List<EntryMemberModel> li = new List<EntryMemberModel>();
            //var data = context.tbl_EntryMember_Billing.ToList();
            //if (data != null)
            //{
            //    foreach(var d in data)
            //    {
            //        li.Add(new EntryMemberModel()
            //        {
            //            BillNo = d.Bill_ID,
            //            Name=context.tbl_EntryMembers.SingleOrDefault(x=>x.ID==d.Member_ID).Name,
            //            Phone_No= context.tbl_EntryMembers.SingleOrDefault(x => x.ID == d.Member_ID).MobileNo,
            //            Total_Member= d.Members,
            //            AmountPaid=d.Amount_Paid,
            //            ModeOfPayment=d.Mode_Of_Payment,
            //            DateofBilling=d.Date_Of_Billing
            //        });
            //    }
            //}
            if (filter != "")
            {
                var Data = JsonConvert.DeserializeObject<dynamic>(filter);
                
              
                ViewBag.Day = Data[1];
                ViewBag.SDate = Data[3];
                ViewBag.EDate = Data[5];

            }
            ViewBag.FilterFromReport = filterFromReport;
            return View();
        }
        public JsonResult SaveBill(string PayMode,int ID)
        {
            if (ID > 0)
            {
                
                if (context.tbl_EntryMember_Billing.Where(x => x.Member_ID == ID).Any())
                {
                    var data = context.tbl_EntryMember_Billing.Where(x => x.Member_ID == ID).FirstOrDefault();
                    EntryMemberModel li = new EntryMemberModel();
                    li.BillNo = data.Bill_ID;
                    li.Name = context.tbl_EntryMembers.SingleOrDefault(x => x.ID == ID).Name;
                    li.Phone_No = context.tbl_EntryMembers.SingleOrDefault(x => x.ID == ID).MobileNo;
                    li.Total_Member = data.Members;
                    li.AmountPaid = data.Amount_Paid;
                    li.ModeOfPayment = data.Mode_Of_Payment;
                    li.DateofBilling = data.Date_Of_Billing;
                    li.GstAmount = data.Gst_Amount;
                    li.Price_Without_Gst = data.Price_Without_Gst;
                    return Json(li);
                }
                else
                {
                    tbl_EntryMember_Billing model = new tbl_EntryMember_Billing();
                    model.Price_With_Gst =double.Parse(context.tbl_EntryMembers.SingleOrDefault(x => x.ID == ID).EntryFee);
                    model.Gst_Amount = Convert.ToDouble(model.Price_With_Gst) * (Convert.ToDouble(18) / (double)100);
                    model.Price_Without_Gst = Convert.ToDouble(model.Price_With_Gst) - Convert.ToDouble(model.Gst_Amount);

                    model.Members = context.tbl_EntryMembers.SingleOrDefault(x => x.ID == ID).qty;
                    model.Gst_Amount =Convert.ToDouble(model.Gst_Amount) * Convert.ToDouble(model.Members);
                    model.Total_Amount =double.Parse(context.tbl_EntryMembers.SingleOrDefault(x => x.ID == ID).TotalAmount);
                    model.Amount_Paid = model.Total_Amount;
                    model.Mode_Of_Payment = PayMode;
                    model.Billed_By = Convert.ToInt32(Request.Cookies["UserID"].Value);
                    model.Member_ID = ID;
                    model.Date_Of_Billing = DateTime.Now.ToString("MM/dd/yyyy");
                    context.tbl_EntryMember_Billing.Add(model);
                    context.SaveChanges();
                    EntryMemberModel li = new EntryMemberModel();
                    li.BillNo = model.Bill_ID;
                    li.Name = context.tbl_EntryMembers.SingleOrDefault(x => x.ID == ID).Name;
                        li.Phone_No = context.tbl_EntryMembers.SingleOrDefault(x => x.ID == ID).MobileNo;
                        li.Total_Member = model.Members;
                    li.AmountPaid = model.Amount_Paid;
                    li.ModeOfPayment = model.Mode_Of_Payment;
                    li.DateofBilling = model.Date_Of_Billing;
                    li.GstAmount = model.Gst_Amount;
                    li.Price_Without_Gst = model.Price_Without_Gst;
                 
                    return Json(li);
                }
            }
            return Json("not Found");
        }
        public ActionResult BillDetailsByBillNo(int id=0)
        {
            EntryMemberModel li = new EntryMemberModel();
            if (id > 0)
            {
                var data = context.tbl_EntryMember_Billing.SingleOrDefault(x => x.Bill_ID == id);
                li.BillNo = data.Bill_ID;
                li.Name = context.tbl_EntryMembers.SingleOrDefault(x => x.ID == data.Member_ID).Name;
                li.Phone_No = context.tbl_EntryMembers.SingleOrDefault(x => x.ID == data.Member_ID).MobileNo;
                li.Total_Member = data.Members;
                li.AmountPaid = data.Amount_Paid;
                li.ModeOfPayment = data.Mode_Of_Payment;
                li.DateofBilling = data.Date_Of_Billing;
            }
            return View(li);
        }
        public DateTime StartOfWeek(DateTime d)
        {
            if (d == DateTime.MinValue)
            {
                return d;
            }
            var result = d.DayOfWeek - DayOfWeek.Sunday;

            if (result < 0)
            {
                result += 7;
            }

            return d.AddDays(result * -1);
        }
        public ActionResult EntryBillsDataAccToDay(int day = 1, string sDate = "", string eDate = "")
        {
            int Billed_By = Convert.ToInt32(Request.Cookies["UserID"].Value);
            int UserType = Convert.ToInt32(Request.Cookies["UserType"].Value);
            DateTime startdate = DateTime.Now;
            DateTime LastDate = DateTime.Now;

            Session["Day"] = day;

            Session["StartDate"] = sDate;
            Session["EndDate"] = eDate;

            switch (day)
            {
                //Today
                case (2):
                    startdate = DateTime.Today;
                    LastDate = DateTime.Now;
                    break;
                //Yesterday
                case (3):
                    startdate = DateTime.Today.AddDays(-1);
                    LastDate = DateTime.Today.AddDays(-1);
                    break;
                //ThisWeek
                case (4):
                    startdate = StartOfWeek(DateTime.Now);
                    LastDate = DateTime.Now;
                    break;
                //ThisMonth
                case (5):
                    startdate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    LastDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
                    break;
                //This Year
                case (6):
                    var year = DateTime.Now.Year;
                    startdate = new DateTime(year, 1, 1);
                    LastDate = new DateTime(year, 12, DateTime.DaysInMonth(year, 12));
                    break;
                //Custom
                case (7):
                    string[] arrDate = sDate.Split('/');
                    string[] arrDate1 = eDate.Split('/');
                    startdate = new DateTime(Convert.ToInt32(arrDate[2]), Convert.ToInt32(arrDate[0]), Convert.ToInt32(arrDate[1]));
                    LastDate = new DateTime(Convert.ToInt32(arrDate1[2]), Convert.ToInt32(arrDate1[0]), Convert.ToInt32(arrDate1[1]));
                    break;
                default:
                    startdate = new DateTime(1900, 1, 1);
                    LastDate = DateTime.Now;
                    break;
            }
            string startDateFormat = startdate.ToString(@"MM\/dd\/yyyy");
            string LastDateFormat = LastDate.ToString(@"MM\/dd\/yyyy");
            //var data = context.tbl_MenusBillingSection.ToList();

            if (!string.IsNullOrEmpty(startDateFormat))
            {
                string queryForBills = "";
                if (UserType == 1 || (Request.Cookies["UserType"].Value == "2" && Request.Cookies["PageSetting"] != null && Request.Cookies["PageSetting"]["FoodBillingEditPermission"] == "All"))
                {

                    queryForBills = "select m.Bill_ID as BillNo,IsNull(m.Price_Without_Gst,0) as Price_Without_Gst ,m.Total_Amount as Total_Amount,m.Gst_Amount as GstAmount,Em.Name as Name,Em.MobileNo as Phone_No,m.Date_Of_Billing as DateofBilling,m.Mode_Of_Payment as ModeOfPayment,m.Amount_Paid as AmountPaid,m.Members as Total_Member from tbl_EntryMember_Billing m join" +
                   " tbl_EntryMembers Em on m.Member_ID = em.ID where cast(m.Date_Of_Billing as date) >= cast('" + startDateFormat + "' as date) and cast(m.Date_Of_Billing as date) <= cast('" + LastDateFormat + "' as date)";

                }
                else
                {
                    queryForBills = "select m.Bill_ID as BillNo,IsNull(m.Price_Without_Gst,0) as Price_Without_Gst ,m.Total_Amount as Total_Amount,m.Gst_Amount as GstAmount,Em.Name as Name,Em.MobileNo as Phone_No,m.Date_Of_Billing as DateofBilling,m.Mode_Of_Payment as ModeOfPayment,m.Amount_Paid as AmountPaid,m.Members as Total_Member from tbl_EntryMember_Billing m join" +
" tbl_EntryMembers Em on m.Member_ID = em.ID where cast(m.Date_Of_Billing as date) >= cast('" + startDateFormat + "' as date) and cast(m.Date_Of_Billing as date) <= cast('" + LastDateFormat + "' as date) and cast(m.Date_Of_Billing as date) <= cast('" + LastDateFormat + "' as date) and m.Billed_By=" + Billed_By;

                }
                var data = context.Database.SqlQuery<EntryMemberModel>(queryForBills);
                double? finaltotalVal = 0;
                double? finalcsgst = 0;
                List<EntryMemberModel> menusBill = new List<EntryMemberModel>();
                foreach (var i in data)
                {
                    //double? totalVal = 0;
                    //double? csgst = 0;
                    menusBill.Add(new EntryMemberModel()
                    {
                        BillNo = i.BillNo,
                        Name = i.Name,
                        Phone_No = i.Phone_No,
                        Total_Amount = i.Total_Amount,
                        Total_Member = i.Total_Member,
                        AmountPaid = i.AmountPaid,
                        ModeOfPayment = i.ModeOfPayment,
                        DateofBilling = i.DateofBilling
                    });
                    finaltotalVal +=i.Total_Amount;
                    finalcsgst += i.GstAmount;

                    //var d = context.tbl_MenusBillingDetailsWithBillNo.Where(a => a.BillNo == i.Bill_Number).ToList();
                    //foreach (var ii in d)
                    //{
                    //    totalVal = totalVal + (ii.Price * ii.Quantity);
                    //}
                    //csgst = totalVal * (2.5 / 100);
                    //csgst = (csgst * 2);
                    //finaltotalVal += totalVal;
                    //finalcsgst += csgst;
                }

                finaltotalVal = Math.Round((Double)finaltotalVal, 2);
                ViewBag.TotalAmount = finaltotalVal  - finalcsgst;
                ViewBag.CSGST = Math.Round((Double)finalcsgst, 2);
                return PartialView("~/Views/Entry/_EntryBillsDataAccToDay.cshtml", menusBill);
            }
            return null;
        
        }
    }
}
