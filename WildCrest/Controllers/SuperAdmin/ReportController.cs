using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WildCrest.Models.WildCrestModels;

namespace WildCrest.Controllers.SuperAdmin
{
    public class ReportController : Controller
    {
        ClubWildCrestEntities context = new ClubWildCrestEntities();

        public ActionResult Index(string sDate = "", string eDate = "")
        {
            if (Request.Cookies["UserType"].Value == "1" || (Request.Cookies["UserType"].Value == "2" && Request.Cookies["PageSetting"] != null && Request.Cookies["PageSetting"]["FoodBillingEditPermission"] == "All"))
            {
                if (sDate != "" && eDate != "")
                {
                    ViewBag.StartDate = sDate;
                    ViewBag.EndDate = eDate;
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public ActionResult ReportsdataAccToCustomDates(string sDate = "", string eDate = "")
        {
            //var menusReport = "select Cast(IsNull(sum(mbs.PriceWithoutTax),0) as float) Menus_Sale, Cast(IsNull(Sum(mbs.GST),0) as float) Menus_GST,Cast(0 as float) Menus_Discount,Cast((IsNull(sum(mbs.PriceWithoutTax),0)+IsNull(Sum(mbs.GST),0)) as float) Menus_Total from tbl_MenusBillingSection mbs where Cast(mbs.PaymentDate as date)>=Cast('" + sDate + "' as date) and Cast(mbs.PaymentDate as date)<=Cast('" + eDate + "' as date)";
            var menusReport = "select Cast(IsNull(sum(mbs.PriceWithoutTax),0) as decimal(18,2)) Menus_Sale, Cast(IsNull(Sum(mbs.GST),0) as decimal(18,2)) Menus_GST,Cast(IsNull(Sum(mbs.Discount),0) as decimal(18,2)) Menus_Discount,Cast((IsNull(sum(mbs.PriceWithoutTax),0)+IsNull(Sum(mbs.GST),0))-IsNull(Sum(mbs.Discount),0) as decimal(18,2)) Menus_Total from tbl_MenusBillingSection mbs where Cast(mbs.PaymentDate as date)>=Cast('" + sDate + "' as date) and Cast(mbs.PaymentDate as date)<=Cast('" + eDate + "' as date) and mbs.Table_Status is null";
            var menusData = context.Database.SqlQuery<MenusReports>(menusReport).FirstOrDefault();

            var nonGstMenusReport = "select Cast(IsNull(sum(mbs.PriceWithoutTax),0) as decimal(18,2)) NonMenusGst_Sale,Cast(0 as decimal) NonMenusGst_Discount,Cast(0 as decimal) NonMenusGst_GST,Cast(IsNull(sum(mbs.PriceWithoutTax),0) as decimal(18,2)) NonMenusGst_Total from tbl_NonGST_MenusBillingSection mbs where Cast(mbs.PaymentDate as date)>=Cast('" + sDate + "' as date) and Cast(mbs.PaymentDate as date)<=Cast('" + eDate + "' as date)";
            var nonGstMenusData = context.Database.SqlQuery<NonMenusGstReports>(nonGstMenusReport).FirstOrDefault();

            var roomReport = "select Cast(IsNull(Sum(rb.Amount-rb.Discount-rb.GST),0) as decimal(18,2)) Room_Sale,Cast(IsNull(sum(rb.Discount),0) as decimal(18,2)) Room_Discount, Cast(IsNull(Sum(rb.GST),0) as decimal(18,2)) Room_GST,Cast(IsNull(Sum(rb.Amount-rb.Discount),0) as decimal(18,2)) Room_Total from tbl_RoomBooking rb  where cast(rb.Check_In as date)>=cast('" + sDate + "' as date) and cast(rb.Check_In as date)<=cast('" + eDate + "' as date)  and rb.Bill_Number IS NOT NULL";
            var roomData = context.Database.SqlQuery<RoomReports>(roomReport).FirstOrDefault();

            var membersMemShipReport = "select Cast(IsNull(sum((mbs.TotalAmount*100)/118),0) as decimal(18,2)) MemBill_Sale, Cast((IsNull(sum((mbs.TotalAmount*100)/118),0)*18/100) as decimal(18,2)) MemBill_GST,Cast(0 as decimal) MemBill_Discount,Cast(IsNull(sum(mbs.TotalAmount),0) as decimal(18,2)) MemBill_Total from tbl_MembersBillingDetails mbs join tbl_Profile prf on mbs.UserID=prf.ID where Cast(mbs.Payment_Date as date)>=Cast('" + sDate + "' as date) and Cast(mbs.Payment_Date as date)<=Cast('" + eDate + "' as date)";
            var membersMemShipData = context.Database.SqlQuery<Members_MemShipReport>(membersMemShipReport).FirstOrDefault();

            var barReport = "select Cast(IsNull(sum(mbs.PriceWithoutTax),0) as decimal(18,2)) Bar_Sale, Cast(IsNull(Sum(mbs.GST),0) as decimal(18,2)) Bar_GST,Cast(IsNull(Sum(mbs.Discount),0) as decimal(18,2)) Bar_Discount,Cast((IsNull(sum(mbs.PriceWithoutTax),0)+IsNull(Sum(mbs.GST),0))-IsNull(Sum(mbs.Discount),0) as decimal(18,2)) Bar_Total from tbl_BarBillingSection mbs where Cast(mbs.PaymentDate as date)>=Cast('" + sDate + "' as date) and Cast(mbs.PaymentDate as date)<=Cast('" + eDate + "' as date) and mbs.Table_Status is null";
            var barData = context.Database.SqlQuery<BarReports>(barReport).FirstOrDefault();

            Reports sale_Report = new Reports();

            sale_Report.Menus_Sale = menusData.Menus_Sale;
            sale_Report.Menus_Discount = menusData.Menus_Discount;
            sale_Report.Menus_GST = menusData.Menus_GST;
            sale_Report.Menus_Total = menusData.Menus_Total;


            sale_Report.NonMenusGst_Sale = nonGstMenusData.NonMenusGst_Sale;
            sale_Report.NonMenusGst_Discount = nonGstMenusData.NonMenusGst_Discount;
            sale_Report.NonMenusGst_GST = nonGstMenusData.NonMenusGst_GST;
            sale_Report.NonMenusGst_Total = nonGstMenusData.NonMenusGst_Total;

            sale_Report.Room_Sale = roomData.Room_Sale;
            sale_Report.Room_Discount = roomData.Room_Discount;
            sale_Report.Room_GST = roomData.Room_GST;
            sale_Report.Room_Total = roomData.Room_Total;

            sale_Report.MemBill_Sale = membersMemShipData.MemBill_Sale;
            sale_Report.MemBill_Discount = membersMemShipData.MemBill_Discount;
            sale_Report.MemBill_GST = membersMemShipData.MemBill_GST;
            sale_Report.MemBill_Total = membersMemShipData.MemBill_Total;

            sale_Report.Bar_Sale = barData.Bar_Sale;
            sale_Report.Bar_Discount = barData.Bar_Discount;
            sale_Report.Bar_GST = barData.Bar_GST;
            sale_Report.Bar_Total = barData.Bar_Total;

            sale_Report.Total_Sale = (menusData.Menus_Sale + nonGstMenusData.NonMenusGst_Sale + roomData.Room_Sale + membersMemShipData.MemBill_Sale + barData.Bar_Sale);
            sale_Report.Total_Discount = (menusData.Menus_Discount + nonGstMenusData.NonMenusGst_Discount + roomData.Room_Discount + membersMemShipData.MemBill_Discount + barData.Bar_Discount);
            sale_Report.Total_GST = (menusData.Menus_GST + nonGstMenusData.NonMenusGst_GST + roomData.Room_GST + membersMemShipData.MemBill_GST + barData.Bar_GST);
            sale_Report.Total_Amount = (menusData.Menus_Total + nonGstMenusData.NonMenusGst_Total + roomData.Room_Total + membersMemShipData.MemBill_Total + barData.Bar_Total);

            sale_Report.StartDate = sDate;
            sale_Report.EndDate = eDate;

            return PartialView("~/Views/Report/_ReportsdataAccToCustomDates.cshtml", sale_Report);

        }

        public ActionResult Members_Membership_Info(string sDate = "", string eDate = "")
        {
            var query = "select mbs.ID,mbs.TotalAmount as Amount_Paid,mbs.Mode_Of_Payment,mbs.Cheque_No,mbs.BankName,mbs.Payment_Date,mbs.UserID,prf.F_Name,prf.L_Name from tbl_MembersBillingDetails mbs join tbl_Profile prf on prf.ID=mbs.UserID where Cast(mbs.Payment_Date as date)>=Cast('" + sDate + "' as date) and Cast(mbs.Payment_Date as date)<=Cast('" + eDate + "' as date)";
            var billingDet = context.Database.SqlQuery<BillingDetails>(query).ToList();
            List<BillingDetails> billingList = new List<BillingDetails>();
            if (billingDet.Count() > 0)
            {
                foreach (var i in billingDet)
                {
                    billingList.Add(new BillingDetails()
                    {
                        ID = i.ID,
                        Amount_Paid = i.Amount_Paid,
                        Mode_Of_Payment = i.Mode_Of_Payment,
                        Cheque_No = i.Cheque_No,
                        BankName = i.BankName,
                        Payment_Date = i.Payment_Date,
                        UserID = i.UserID,
                        F_Name = i.F_Name,
                        L_Name = i.L_Name
                    });
                }
            }
            ViewBag.StartDate = sDate;
            ViewBag.EndDate = eDate;
            return View(billingList);
        }

    }
}
