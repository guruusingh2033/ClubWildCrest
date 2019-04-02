﻿using System;
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

            var nonGstBarReport = "select Cast(IsNull(sum(mbs.PriceWithoutTax),0) as decimal(18,2)) NonGst_BarSale,Cast(0 as decimal) NonGst_BarDiscount,Cast(0 as decimal) NonGst_BarGST,Cast(IsNull(sum(mbs.PriceWithoutTax),0) as decimal(18,2)) NonGst_BarTotal from tbl_NonGST_BarBillingSection mbs where Cast(mbs.PaymentDate as date)>=Cast('" + sDate + "' as date) and Cast(mbs.PaymentDate as date)<=Cast('" + eDate + "' as date)";
            var nonGstBarData = context.Database.SqlQuery<NonMenusBarReports>(nonGstBarReport).FirstOrDefault();

            var WineReport = "select Cast(IsNull(sum(mbs.PriceWithoutTax),0) as decimal(18,2)) Wine_Sale, Cast(IsNull(Sum(mbs.GST),0) as decimal(18,2)) Wine_GST,Cast(IsNull(Sum(mbs.Discount),0) as decimal(18,2)) Wine_Discount,Cast((IsNull(sum(mbs.PriceWithoutTax),0)+IsNull(Sum(mbs.GST),0))-IsNull(Sum(mbs.Discount),0) as decimal(18,2)) Wine_Total from tbl_WineBillingSection mbs where Cast(mbs.PaymentDate as date)>=Cast('" + sDate + "' as date) and Cast(mbs.PaymentDate as date)<=Cast('" + eDate + "' as date) and mbs.Table_Status is null";
            var WineData = context.Database.SqlQuery<WineReports>(WineReport).FirstOrDefault();
            var nonGstWineReport = "select Cast(IsNull(sum(mbs.PriceWithoutTax),0) as decimal(18,2)) NonGst_WineSale,Cast(0 as decimal) NonGst_WineDiscount,Cast(0 as decimal) NonGst_WineGST,Cast(IsNull(sum(mbs.PriceWithoutTax),0) as decimal(18,2)) NonGst_WineTotal from tbl_NonGST_WineBillingSection mbs where Cast(mbs.PaymentDate as date)>=Cast('" + sDate + "' as date) and Cast(mbs.PaymentDate as date)<=Cast('" + eDate + "' as date)";
            var nonGstWineData = context.Database.SqlQuery<NonMenusWineReports>(nonGstWineReport).FirstOrDefault();




            Reports sale_Report = new Reports();
            #region Menus definition
            var Food_Cash_Payment = "select Cast((IsNull(sum(mbs.PriceWithoutTax),0)+IsNull(Sum(mbs.GST),0))-IsNull(Sum(mbs.Discount),0) as decimal(18,2)) Food_CashPay from tbl_MenusBillingSection mbs where Cast(mbs.PaymentDate as date)>=Cast('" + sDate + "' as date) and Cast(mbs.PaymentDate as date)<=Cast('" + eDate + "' as date) and mbs.Table_Status is null and mbs.Mode_Of_Payment='Cash'";
            var FoodCashapay = context.Database.SqlQuery<FoodPayment>(Food_Cash_Payment).FirstOrDefault();
            var Food_Paytm_Payment = "select Cast((IsNull(sum(mbs.PriceWithoutTax),0)+IsNull(Sum(mbs.GST),0))-IsNull(Sum(mbs.Discount),0) as decimal(18,2)) Food_PaytmPay from tbl_MenusBillingSection mbs where Cast(mbs.PaymentDate as date)>=Cast('" + sDate + "' as date) and Cast(mbs.PaymentDate as date)<=Cast('" + eDate + "' as date) and mbs.Table_Status is null and mbs.Mode_Of_Payment='Paytm'";
            var FoodPaytmapay = context.Database.SqlQuery<FoodPayment>(Food_Paytm_Payment).FirstOrDefault();
            var Food_Card_Payment = "select Cast((IsNull(sum(mbs.PriceWithoutTax),0)+IsNull(Sum(mbs.GST),0))-IsNull(Sum(mbs.Discount),0) as decimal(18,2)) Food_CardPay from tbl_MenusBillingSection mbs where Cast(mbs.PaymentDate as date)>=Cast('" + sDate + "' as date) and Cast(mbs.PaymentDate as date)<=Cast('" + eDate + "' as date) and mbs.Table_Status is null and mbs.Mode_Of_Payment='Card'";
            var FoodCardapay = context.Database.SqlQuery<FoodPayment>(Food_Card_Payment).FirstOrDefault();
            var Food_Cheque_Payment = "select Cast((IsNull(sum(mbs.PriceWithoutTax),0)+IsNull(Sum(mbs.GST),0))-IsNull(Sum(mbs.Discount),0) as decimal(18,2)) Food_ChequePay from tbl_MenusBillingSection mbs where Cast(mbs.PaymentDate as date)>=Cast('" + sDate + "' as date) and Cast(mbs.PaymentDate as date)<=Cast('" + eDate + "' as date) and mbs.Table_Status is null and mbs.Mode_Of_Payment='Cheque'";
            var FoodChequepay = context.Database.SqlQuery<FoodPayment>(Food_Cheque_Payment).FirstOrDefault();
            
            sale_Report.Menus_Sale = menusData.Menus_Sale;
            sale_Report.Menus_Discount = menusData.Menus_Discount;
            sale_Report.Menus_GST = menusData.Menus_GST;
            sale_Report.Food_CashPay = Convert.ToDecimal(FoodCashapay.Food_CashPay);
            sale_Report.Food_CardPay = Convert.ToDecimal(FoodCardapay.Food_CardPay);
            sale_Report.Food_PaytmPay = Convert.ToDecimal(FoodPaytmapay.Food_PaytmPay);
            sale_Report.Food_ChequePay = Convert.ToDecimal(FoodChequepay.Food_ChequePay);
            sale_Report.Menus_Total = menusData.Menus_Total;

            var NonGst_Food_CashPay = "select Cast(IsNull(sum(mbs.PriceWithoutTax),0) as decimal(18,2)) NonGst_Food_CashPay from tbl_NonGST_MenusBillingSection mbs where Cast(mbs.PaymentDate as date)>=Cast('" + sDate + "' as date) and Cast(mbs.PaymentDate as date)<=Cast('" + eDate + "' as date)  and mbs.Mode_Of_Payment='Cash'";
            var NonGstFoodCashapay = context.Database.SqlQuery<NonGstFoodPayment>(NonGst_Food_CashPay).FirstOrDefault();
            var NonGst_Food_Paytm_Payment = "select Cast(IsNull(sum(mbs.PriceWithoutTax),0) as decimal(18,2)) NonGst_Food_PaytmPay from tbl_NonGST_MenusBillingSection mbs where Cast(mbs.PaymentDate as date)>=Cast('" + sDate + "' as date) and Cast(mbs.PaymentDate as date)<=Cast('" + eDate + "' as date)  and mbs.Mode_Of_Payment='Paytm'";
            var NonGstFoodPaytmapay = context.Database.SqlQuery<NonGstFoodPayment>(NonGst_Food_Paytm_Payment).FirstOrDefault();
            var NonGst_Food_Card_Payment = "select Cast(IsNull(sum(mbs.PriceWithoutTax),0) as decimal(18,2)) NonGst_Food_CardPay from tbl_NonGST_MenusBillingSection mbs where Cast(mbs.PaymentDate as date)>=Cast('" + sDate + "' as date) and Cast(mbs.PaymentDate as date)<=Cast('" + eDate + "' as date)  and mbs.Mode_Of_Payment='Card'";
            var NonGstFoodCardapay = context.Database.SqlQuery<NonGstFoodPayment>(NonGst_Food_Card_Payment).FirstOrDefault();
            var NonGst_Food_Cheque_Payment = "select Cast(IsNull(sum(mbs.PriceWithoutTax),0) as decimal(18,2)) NonGst_Food_ChequePay from tbl_NonGST_MenusBillingSection mbs where Cast(mbs.PaymentDate as date)>=Cast('" + sDate + "' as date) and Cast(mbs.PaymentDate as date)<=Cast('" + eDate + "' as date)  and mbs.Mode_Of_Payment='Cheque'";
            var NonGstFoodChequepay = context.Database.SqlQuery<NonGstFoodPayment>(NonGst_Food_Cheque_Payment).FirstOrDefault();


            sale_Report.NonMenusGst_Sale = nonGstMenusData.NonMenusGst_Sale;
            sale_Report.NonMenusGst_Discount = nonGstMenusData.NonMenusGst_Discount;
            sale_Report.NonMenusGst_GST = nonGstMenusData.NonMenusGst_GST;
            sale_Report.NonGst_Food_CashPay = Convert.ToDecimal(NonGstFoodCashapay.NonGst_Food_CashPay);
            sale_Report.NonGst_Food_CardPay = Convert.ToDecimal(NonGstFoodCardapay.NonGst_Food_CardPay);
            sale_Report.NonGst_Food_PaytmPay = Convert.ToDecimal(NonGstFoodPaytmapay.NonGst_Food_PaytmPay);
            sale_Report.NonGst_Food_ChequePay = Convert.ToDecimal(NonGstFoodChequepay.NonGst_Food_ChequePay);
            sale_Report.NonMenusGst_Total = nonGstMenusData.NonMenusGst_Total;
           
            #endregion

            #region Room definition
            var Room_Cash_Payment = "select Cast(IsNull(Sum(rb.GST),0) as decimal(18,2)) Room_GST,Cast(IsNull(Sum(rb.Amount-rb.Discount),0) as decimal(18,2)) Room_CashPay from tbl_RoomBooking rb  where cast(rb.Check_In as date)>=cast('" + sDate + "' as date) and cast(rb.Check_In as date)<=cast('" + eDate + "' as date)  and rb.Bill_Number IS NOT NULL and rb.Mode_Of_Payment='Cash'";
            var RoomCashapay = context.Database.SqlQuery<RoomPayment>(Room_Cash_Payment).FirstOrDefault();
            var Room_Paytm_Payment = "select Cast(IsNull(Sum(rb.GST),0) as decimal(18,2)) Room_GST,Cast(IsNull(Sum(rb.Amount-rb.Discount),0) as decimal(18,2)) Room_PatymPay from tbl_RoomBooking rb  where cast(rb.Check_In as date)>=cast('" + sDate + "' as date) and cast(rb.Check_In as date)<=cast('" + eDate + "' as date)  and rb.Bill_Number IS NOT NULL and rb.Mode_Of_Payment='Paytm'";
            var RoomPaytmapay = context.Database.SqlQuery<RoomPayment>(Room_Paytm_Payment).FirstOrDefault();
            var Room_Card_Payment = "select Cast(IsNull(Sum(rb.GST),0) as decimal(18,2)) Room_GST,Cast(IsNull(Sum(rb.Amount-rb.Discount),0) as decimal(18,2)) Room_CardPay from tbl_RoomBooking rb  where cast(rb.Check_In as date)>=cast('" + sDate + "' as date) and cast(rb.Check_In as date)<=cast('" + eDate + "' as date)  and rb.Bill_Number IS NOT NULL  and rb.Mode_Of_Payment='Card'";
            var RoomCardapay = context.Database.SqlQuery<RoomPayment>(Room_Card_Payment).FirstOrDefault();
            var Room_Cheque_Payment = "select Cast(IsNull(Sum(rb.GST),0) as decimal(18,2)) Room_GST,Cast(IsNull(Sum(rb.Amount-rb.Discount),0) as decimal(18,2)) Room_ChequePay from tbl_RoomBooking rb  where cast(rb.Check_In as date)>=cast('" + sDate + "' as date) and cast(rb.Check_In as date)<=cast('" + eDate + "' as date)  and rb.Bill_Number IS NOT NULL  and rb.Mode_Of_Payment='Cheque'";
            var RoomChequepay = context.Database.SqlQuery<RoomPayment>(Room_Cheque_Payment).FirstOrDefault();
            sale_Report.Room_Sale = roomData.Room_Sale;
            sale_Report.Room_Discount = roomData.Room_Discount;
            sale_Report.Room_GST = roomData.Room_GST;
            sale_Report.Room_CashPay = Convert.ToDecimal(RoomCashapay.Room_CashPay);
            sale_Report.Room_CardPay = Convert.ToDecimal(RoomCardapay.Room_CardPay);
            sale_Report.Room_PaytmPay = Convert.ToDecimal(RoomPaytmapay.Room_PaytmPay);
            sale_Report.Room_ChequePay = Convert.ToDecimal(RoomChequepay.Room_ChequePay);
            sale_Report.Room_Total = roomData.Room_Total;
            #endregion

            #region Member definition
            var MemBill_Cash_Payment = " select Cast(IsNull(sum(mbs.TotalAmount),0) as decimal(18,2)) MemBill_CashPay from tbl_MembersBillingDetails mbs join tbl_Profile prf on mbs.UserID=prf.ID where Cast(mbs.Payment_Date as date)>=Cast('" + sDate + "' as date) and Cast(mbs.Payment_Date as date)<=Cast('" + eDate + "' as date) and mbs.Mode_Of_Payment='Cash'";
            var MemBillCashapay = context.Database.SqlQuery<MemberPayment>(MemBill_Cash_Payment).FirstOrDefault();
            var MemBill_Paytm_Payment = "select Cast(IsNull(sum(mbs.TotalAmount),0) as decimal(18,2)) MemBill_PaytmPay from tbl_MembersBillingDetails mbs join tbl_Profile prf on mbs.UserID=prf.ID where Cast(mbs.Payment_Date as date)>=Cast('" + sDate + "' as date) and Cast(mbs.Payment_Date as date)<=Cast('" + eDate + "' as date) and mbs.Mode_Of_Payment='Paytm'";
            var MemBillPaytmapay = context.Database.SqlQuery<MemberPayment>(MemBill_Paytm_Payment).FirstOrDefault();
            var MemBill_Card_Payment = "select Cast(IsNull(sum(mbs.TotalAmount),0) as decimal(18,2)) MemBill_CardPay from tbl_MembersBillingDetails mbs join tbl_Profile prf on mbs.UserID=prf.ID where Cast(mbs.Payment_Date as date)>=Cast('" + sDate + "' as date) and Cast(mbs.Payment_Date as date)<=Cast('" + eDate + "' as date) and mbs.Mode_Of_Payment='Card'";
            var MemBillCardapay = context.Database.SqlQuery<MemberPayment>(MemBill_Card_Payment).FirstOrDefault();
            var MemBill_Cheque_Payment = "select Cast(IsNull(sum(mbs.TotalAmount),0) as decimal(18,2)) MemBill_ChequePay from tbl_MembersBillingDetails mbs join tbl_Profile prf on mbs.UserID=prf.ID where Cast(mbs.Payment_Date as date)>=Cast('" + sDate + "' as date) and Cast(mbs.Payment_Date as date)<=Cast('" + eDate + "' as date) and mbs.Mode_Of_Payment='Cheque'";
            var MemBillChequepay = context.Database.SqlQuery<MemberPayment>(MemBill_Cheque_Payment).FirstOrDefault();
            sale_Report.MemBill_Sale = membersMemShipData.MemBill_Sale;
            sale_Report.MemBill_Discount = membersMemShipData.MemBill_Discount;
            sale_Report.MemBill_GST = membersMemShipData.MemBill_GST;

            sale_Report.MemBill_CashPay = Convert.ToDecimal(MemBillCashapay.MemBill_CashPay);
            sale_Report.MemBill_CardPay = Convert.ToDecimal(MemBillCardapay.MemBill_CardPay);
            sale_Report.MemBill_PaytmPay = Convert.ToDecimal(MemBillPaytmapay.MemBill_PaytmPay);
            sale_Report.MemBill_ChequePay = Convert.ToDecimal(MemBillChequepay.MemBill_ChequePay);
            sale_Report.MemBill_Total = membersMemShipData.MemBill_Total;
            #endregion
            #region Bar definition
            var Bar_Cash_Payment = "select Cast((IsNull(sum(mbs.PriceWithoutTax),0)+IsNull(Sum(mbs.GST),0))-IsNull(Sum(mbs.Discount),0) as decimal(18,2)) Bar_CashPay from tbl_BarBillingSection mbs where Cast(mbs.PaymentDate as date)>=Cast('" + sDate + "' as date) and Cast(mbs.PaymentDate as date)<=Cast('" + eDate + "' as date) and mbs.Table_Status is null and mbs.Mode_Of_Payment='Cash'";
            var BarCashapay = context.Database.SqlQuery<BarPayment>(Bar_Cash_Payment).FirstOrDefault();
            var Bar_Paytm_Payment = "select Cast((IsNull(sum(mbs.PriceWithoutTax),0)+IsNull(Sum(mbs.GST),0))-IsNull(Sum(mbs.Discount),0) as decimal(18,2)) Bar_PaytmPay from tbl_BarBillingSection mbs where Cast(mbs.PaymentDate as date)>=Cast('" + sDate + "' as date) and Cast(mbs.PaymentDate as date)<=Cast('" + eDate + "' as date) and mbs.Table_Status is null and mbs.Mode_Of_Payment='Paytm'";
            var BarPaytmapay = context.Database.SqlQuery<BarPayment>(Bar_Paytm_Payment).FirstOrDefault();
            var Bar_Card_Payment = "select Cast((IsNull(sum(mbs.PriceWithoutTax),0)+IsNull(Sum(mbs.GST),0))-IsNull(Sum(mbs.Discount),0) as decimal(18,2)) Bar_CardPay from tbl_BarBillingSection mbs where Cast(mbs.PaymentDate as date)>=Cast('" + sDate + "' as date) and Cast(mbs.PaymentDate as date)<=Cast('" + eDate + "' as date) and mbs.Table_Status is null and mbs.Mode_Of_Payment='Card'";
            var BarCardapay = context.Database.SqlQuery<BarPayment>(Bar_Card_Payment).FirstOrDefault();
            var Bar_Cheque_Payment = "select Cast((IsNull(sum(mbs.PriceWithoutTax),0)+IsNull(Sum(mbs.GST),0))-IsNull(Sum(mbs.Discount),0) as decimal(18,2)) Bar_ChequePay from tbl_BarBillingSection mbs where Cast(mbs.PaymentDate as date)>=Cast('" + sDate + "' as date) and Cast(mbs.PaymentDate as date)<=Cast('" + eDate + "' as date) and mbs.Table_Status is null and mbs.Mode_Of_Payment='Cheque'";
            var BarChequepay = context.Database.SqlQuery<BarPayment>(Bar_Cheque_Payment).FirstOrDefault();
            sale_Report.Bar_Sale = barData.Bar_Sale;
            sale_Report.Bar_Discount = barData.Bar_Discount;
            sale_Report.Bar_GST = barData.Bar_GST;
            sale_Report.Bar_CashPay = Convert.ToDecimal(BarCashapay.Bar_CashPay);
            sale_Report.Bar_CardPay = Convert.ToDecimal(BarCardapay.Bar_CardPay);
            sale_Report.Bar_PaytmPay = Convert.ToDecimal(BarPaytmapay.Bar_PaytmPay);
            sale_Report.Bar_ChequePay = Convert.ToDecimal(BarChequepay.Bar_ChequePay);
            sale_Report.Bar_Total = barData.Bar_Total;

            sale_Report.NonGst_BarSale = nonGstBarData.NonGst_BarSale;
            sale_Report.NonGst_BarDiscount = nonGstBarData.NonGst_BarDiscount;
            sale_Report.NonGst_BarGST = nonGstBarData.NonGst_BarGST;
            sale_Report.NonGst_BarTotal = nonGstBarData.NonGst_BarTotal;
            #endregion
            #region Wine definition

            var Wine_Cash_Payment ="select Cast((IsNull(sum(mbs.PriceWithoutTax),0)+IsNull(Sum(mbs.GST),0))-IsNull(Sum(mbs.Discount),0) as decimal(18,2)) Wine_CashPay from tbl_WineBillingSection mbs where Cast(mbs.PaymentDate as date)>=Cast('" + sDate + "' as date) and Cast(mbs.PaymentDate as date)<=Cast('" + eDate + "' as date) and mbs.Table_Status is null and mbs.Mode_Of_Payment='Cash'";
           var WineCashapay = context.Database.SqlQuery<WinePayment>(Wine_Cash_Payment).FirstOrDefault();
            var Wine_Paytm_Payment = "select Cast((IsNull(sum(mbs.PriceWithoutTax),0)+IsNull(Sum(mbs.GST),0))-IsNull(Sum(mbs.Discount),0) as decimal(18,2)) Wine_PaytmPay from tbl_WineBillingSection mbs where Cast(mbs.PaymentDate as date)>=Cast('" + sDate + "' as date) and Cast(mbs.PaymentDate as date)<=Cast('" + eDate + "' as date) and mbs.Table_Status is null and mbs.Mode_Of_Payment='Paytm'";
            var WinePaytmapay = context.Database.SqlQuery<WinePayment>(Wine_Paytm_Payment).FirstOrDefault();
            var Wine_Card_Payment = "select Cast((IsNull(sum(mbs.PriceWithoutTax),0)+IsNull(Sum(mbs.GST),0))-IsNull(Sum(mbs.Discount),0) as decimal(18,2)) Wine_CardPay from tbl_WineBillingSection mbs where Cast(mbs.PaymentDate as date)>=Cast('" + sDate + "' as date) and Cast(mbs.PaymentDate as date)<=Cast('" + eDate + "' as date) and mbs.Table_Status is null and mbs.Mode_Of_Payment='Card'";
            var WineCardapay = context.Database.SqlQuery<WinePayment>(Wine_Card_Payment).FirstOrDefault();
            var Wine_Cheque_Payment = "select Cast((IsNull(sum(mbs.PriceWithoutTax),0)+IsNull(Sum(mbs.GST),0))-IsNull(Sum(mbs.Discount),0) as decimal(18,2)) Wine_ChequePay from tbl_WineBillingSection mbs where Cast(mbs.PaymentDate as date)>=Cast('" + sDate + "' as date) and Cast(mbs.PaymentDate as date)<=Cast('" + eDate + "' as date) and mbs.Table_Status is null and mbs.Mode_Of_Payment='Cheque'";
            var WineChequepay = context.Database.SqlQuery<WinePayment>(Wine_Cheque_Payment).FirstOrDefault();
            

            sale_Report.Wine_Sale = WineData.Wine_Sale;
            sale_Report.Wine_Discount = WineData.Wine_Discount;
            sale_Report.Wine_GST = WineData.Wine_GST;
            sale_Report.Wine_CashPay =Convert.ToDecimal(WineCashapay.Wine_CashPay);
            sale_Report.Wine_CardPay = Convert.ToDecimal(WineCardapay.Wine_CardPay);
            sale_Report.Wine_PaytmPay = Convert.ToDecimal(WinePaytmapay.Wine_PaytmPay);
            sale_Report.Wine_ChequePay = Convert.ToDecimal(WineChequepay.Wine_ChequePay);
            sale_Report.Wine_Total = WineData.Wine_Total;
           
            sale_Report.NonGst_WineSale = nonGstWineData.NonGst_WineSale;
            sale_Report.NonGst_WineDiscount = nonGstWineData.NonGst_WineDiscount;
            sale_Report.NonGst_WineGST = nonGstWineData.NonGst_WineGST;
           
            sale_Report.NonGst_WineTotal = nonGstWineData.NonGst_WineTotal;

            #endregion

            sale_Report.Total_Sale = (menusData.Menus_Sale + nonGstMenusData.NonMenusGst_Sale + roomData.Room_Sale + membersMemShipData.MemBill_Sale + barData.Bar_Sale + nonGstBarData.NonGst_BarSale + WineData.Wine_Sale + nonGstWineData.NonGst_WineSale);
            sale_Report.Total_Discount = (menusData.Menus_Discount + nonGstMenusData.NonMenusGst_Discount + roomData.Room_Discount + membersMemShipData.MemBill_Discount + barData.Bar_Discount + nonGstBarData.NonGst_BarDiscount + WineData.Wine_Discount + nonGstWineData.NonGst_WineDiscount);
            sale_Report.Total_GST = (menusData.Menus_GST + nonGstMenusData.NonMenusGst_GST + roomData.Room_GST + membersMemShipData.MemBill_GST + barData.Bar_GST + nonGstBarData.NonGst_BarGST + WineData.Wine_GST + nonGstWineData.NonGst_WineGST);

            sale_Report.Total_Cash = (FoodCashapay.Food_CashPay + NonGstFoodCashapay.NonGst_Food_CashPay + RoomCashapay.Room_CashPay + MemBillCashapay.MemBill_CashPay + BarCashapay.Bar_CashPay + WineCashapay.Wine_CashPay);
            sale_Report.Total_Card = (FoodCardapay.Food_CardPay + NonGstFoodCardapay.NonGst_Food_CardPay + RoomCardapay.Room_CardPay + MemBillCardapay.MemBill_CardPay + BarCardapay.Bar_CardPay + WineCardapay.Wine_CardPay);
            sale_Report.Total_Paytm = (FoodPaytmapay.Food_PaytmPay + NonGstFoodPaytmapay.NonGst_Food_PaytmPay + RoomPaytmapay.Room_PaytmPay + MemBillPaytmapay.MemBill_PaytmPay + BarPaytmapay.Bar_PaytmPay + WinePaytmapay.Wine_PaytmPay);
            sale_Report.Total_ChequePay = (FoodChequepay.Food_ChequePay + NonGstFoodChequepay.NonGst_Food_ChequePay + RoomChequepay.Room_ChequePay + MemBillChequepay.MemBill_ChequePay + BarChequepay.Bar_ChequePay + WineChequepay.Wine_ChequePay);

            sale_Report.Total_Amount = (menusData.Menus_Total + nonGstMenusData.NonMenusGst_Total + roomData.Room_Total + membersMemShipData.MemBill_Total + barData.Bar_Total + nonGstBarData.NonGst_BarTotal + WineData.Wine_Total + nonGstWineData.NonGst_WineTotal);

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
                        L_Name = i.L_Name,

                    });
                }
            }
            ViewBag.StartDate = sDate;
            ViewBag.EndDate = eDate;
            return View(billingList);
        }

    }
}
