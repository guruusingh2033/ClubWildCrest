using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WildCrest.Models.WildCrestModels;

namespace WildCrest.Controllers.SuperAdmin
{
    public class CustomBillingController : Controller
    {
        ClubWildCrestEntities context = new ClubWildCrestEntities();
        //
        // GET: /CustomBilling/

        public ActionResult Index(string filter = "", string filterFromReport = "")
        {
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

        public ActionResult Add()
        {
            return View();
        }
        string GetBillNO()
        {

            var CustomBill = context.tbl_custombilling.OrderByDescending(obj => obj.ServiceID).FirstOrDefault();
            string BillNo =CustomBill==null?null:CustomBill.ServiceBillNO;
            if (!string.IsNullOrEmpty(BillNo))
            {
                int lastno = int.Parse(BillNo.Substring(BillNo.LastIndexOf("/") + 1)) + 1;
                DateTime AprilDay = new DateTime(DateTime.Today.Year, 4, 01);
                int compareValue = AprilDay.CompareTo(DateTime.Today);
                string dateformat = "";
                string nextyear = "";
                switch (compareValue)
                {
                    case 0:
                        string paymentDate = context.tbl_custombilling.OrderByDescending(obj => obj.ServiceID).FirstOrDefault().BillingDate.ToString();
                        if (paymentDate == AprilDay.ToString("MM/dd/yyyy"))
                        {
                            lastno = int.Parse(BillNo.Substring(BillNo.LastIndexOf("/") + 1)) + 1;
                        }
                        else
                        {
                            lastno = 1;
                        }
                        nextyear = DateTime.Now.AddYears(1).Year.ToString().Substring(2);
                        dateformat = "Custom/" + DateTime.Now.Year.ToString() + "-" + nextyear + "/00" + lastno;

                        break;
                    case -1:
                        string LastpaymentDate = context.tbl_custombilling.OrderByDescending(obj => obj.ServiceID).FirstOrDefault().BillingDate.ToString();
                        int compare = AprilDay.CompareTo(Convert.ToDateTime(LastpaymentDate));
                        if (compare == 1)
                        {
                            lastno = 1;
                        }
                        else
                        {
                            lastno = int.Parse(BillNo.Substring(BillNo.LastIndexOf("/") + 1)) + 1;
                        }


                        nextyear = DateTime.Now.AddYears(1).Year.ToString().Substring(2);
                        dateformat = "Custom/" + DateTime.Now.Year.ToString() + "-" + nextyear + "/00" + lastno;
                        break;

                    case 1:
                        lastno = int.Parse(BillNo.Substring(BillNo.LastIndexOf("/") + 1)) + 1;

                        nextyear = DateTime.Now.Year.ToString().Substring(2);
                        dateformat = "Custom/" + DateTime.Now.AddYears(-compareValue).Year.ToString() + "-" + nextyear + "/00" + lastno;
                        break;
                }

                return dateformat;
            }
            else
            {
                int lastno = 1;
                DateTime AprilDay = new DateTime(DateTime.Today.Year, 4, 01);
                int compareValue = AprilDay.CompareTo(DateTime.Today);
                string dateformat = "";
                string nextyear = "";
                switch (compareValue)
                {
                    case 0:
                      
                            lastno = 1;
                      
                        nextyear = DateTime.Now.AddYears(1).Year.ToString().Substring(2);
                        dateformat = "Custom/" + DateTime.Now.Year.ToString() + "-" + nextyear + "/00" + lastno;

                        break;
                    case -1:
                      
                            lastno = 1;
                        nextyear = DateTime.Now.AddYears(1).Year.ToString().Substring(2);
                        dateformat = "Custom/" + DateTime.Now.Year.ToString() + "-" + nextyear + "/00" + lastno;
                        break;

                    case 1:
                        lastno = 1;

                        nextyear = DateTime.Now.Year.ToString().Substring(2);
                        dateformat = "Custom/" + DateTime.Now.AddYears(-compareValue).Year.ToString() + "-" + nextyear + "/00" + lastno;
                        break;
                }

                return dateformat;
            }
            
        }
        public JsonResult SaveBill(tbl_custombilling model)
        {
            model.BillingDate = DateTime.Now.Date;
            model.Gst_Amount =(Math.Round((double)model.Amount_Without_Gst * ((double)model.Gst_Amount / (double)100), 2))*model.Qty;
            model.TotalAmount = model.Amount_Without_Gst + model.Gst_Amount;
            model.ServiceBillNO = GetBillNO();
            model.Billed_By = Convert.ToInt32(Request.Cookies["UserID"].Value);
            context.tbl_custombilling.Add(model);
            context.SaveChanges();
            return Json("Saved");
        }
        public ActionResult BillsDataAccToDay(int day = 1, string sDate = "", string eDate = "")
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

                    queryForBills = "select m.ServiceID as ServiceID,IsNull(m.Amount_Without_Gst,0) as Amount_Without_Gst ,m.TotalAmount as TotalAmount,m.Gst_Amount as Gst_Amount,m.Customer as Customer,m.PhoneNo as PhoneNo,m.BillingDate as BillingDate,m.Mode_Of_Payment as Mode_Of_Payment,m.TotalAmount as TotalAmount,m.ServiceName as ServiceName from tbl_custombilling m "+
                        "where m.BillingDate>= cast('" + startDateFormat + "' as date) and m.BillingDate <= cast('" + LastDateFormat + "' as date)";

                }
                else
                {
                    queryForBills = "select m.ServiceID as ServiceID,IsNull(m.Amount_Without_Gst,0) as Amount_Without_Gst ,m.TotalAmount as TotalAmount,m.Gst_Amount as Gst_Amount,m.Customer as Customer,m.PhoneNo as PhoneNo,m.BillingDate as BillingDate,m.Mode_Of_Payment as Mode_Of_Payment,m.TotalAmount as TotalAmount,m.ServiceName as ServiceName from tbl_custombilling m " +
                          "where m.BillingDate >= cast('" + startDateFormat + "' as date) and m.BillingDate <= cast('" + LastDateFormat + "' as date) and m.Billed_By=" + Billed_By;

                }
                var data = context.Database.SqlQuery<CustomBillingModel>(queryForBills);
                double? finaltotalVal = 0;
                double? finalcsgst = 0;
                List<CustomBillingModel> menusBill = new List<CustomBillingModel>();
                foreach (var i in data)
                {
                    //double? totalVal = 0;
                    //double? csgst = 0;
                    menusBill.Add(new CustomBillingModel()
                    {
                        ServiceID = i.ServiceID,
                        Customer = i.Customer,
                        ServiceName = i.ServiceName,
                        Amount_Without_Gst = i.Amount_Without_Gst,
                        Gst_Amount = i.Gst_Amount,
                        TotalAmount = i.TotalAmount,
                        Mode_Of_Payment = i.Mode_Of_Payment,
                        billingDateStr = String.Format("{0:d/M/yyyy}", i.BillingDate),
                        PhoneNo=i.PhoneNo,

                    });
                    finaltotalVal += i.TotalAmount;
                    finalcsgst += i.Gst_Amount;

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
                ViewBag.TotalAmount = finaltotalVal - finalcsgst;
                ViewBag.CSGST = Math.Round((Double)finalcsgst, 2);
                return PartialView("~/Views/CustomBilling/_CustomBillsDataAccToDay.cshtml", menusBill);
            }
            return null;
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

        public ActionResult BillDetailByBillNo(int id=0)
        {
            CustomBillingModel Item = new CustomBillingModel();
            if (id > 0)
            {
                var data = context.tbl_custombilling.SingleOrDefault(x => x.ServiceID == id);
                Item.ServiceID = data.ServiceID;
                Item.Customer = data.Customer;
                Item.ServiceName = data.ServiceName;
                Item.ServiceBillNO = data.ServiceBillNO;
                Item.PhoneNo = data.PhoneNo;
                Item.Qty = data.Qty;
                Item.Mode_Of_Payment = data.Mode_Of_Payment;
                Item.TotalAmount = data.TotalAmount;
                Item.Gst_Amount = data.Gst_Amount;
                Item.Details = data.Details;
                Item.BillingDate = data.BillingDate;
                Item.Amount_Without_Gst = data.Amount_Without_Gst;
                Item.Customer_GstNO = data.Customer_GstNO;
            }
            return View(Item);
        }
    }
}
