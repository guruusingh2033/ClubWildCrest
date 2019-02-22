using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WildCrest.Models.WildCrestModels
{
    public class Reports
    {
        public decimal Menus_Sale { get; set; }
        public decimal Menus_GST { get; set; }
        public decimal Menus_Discount { get; set; }
        public decimal Menus_Total { get; set; }

        public decimal NonMenusGst_Sale { get; set; }
        public decimal NonMenusGst_Discount { get; set; }
        public decimal NonMenusGst_GST { get; set; }
        public decimal NonMenusGst_Total { get; set; }

        public decimal Room_Sale { get; set; }
        public decimal Room_GST { get; set; }
        public decimal Room_Discount { get; set; }
        public decimal Room_Total { get; set; }

        public decimal Total_Sale { get; set; }
        public decimal Total_Discount { get; set; }
        public decimal Total_GST { get; set; }
        public decimal Total_Amount { get; set; }

        public decimal MemBill_Sale { get; set; }
        public decimal MemBill_Discount { get; set; }
        public decimal MemBill_GST { get; set; }
        public decimal MemBill_Total { get; set; }

        public decimal Bar_Sale { get; set; }
        public decimal Bar_GST { get; set; }
        public decimal Bar_Discount { get; set; }
        public decimal Bar_Total { get; set; }

        public decimal NonGst_BarSale { get; set; }
        public decimal NonGst_BarDiscount { get; set; }
        public decimal NonGst_BarGST { get; set; }
        public decimal NonGst_BarTotal { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class RoomReports
    {        
        public decimal Room_Sale { get; set; }
        public decimal Room_GST { get; set; }
        public decimal Room_Discount { get; set; }
        public decimal Room_Total { get; set; }
    }

    public class MenusReports
    {
        public decimal Menus_Sale { get; set; }
        public decimal Menus_GST { get; set; }
        public decimal Menus_Discount { get; set; }
        public decimal Menus_Total { get; set; }        
    }

    public class NonMenusGstReports
    {
        public decimal NonMenusGst_Sale { get; set; }
        public decimal NonMenusGst_Discount { get; set; }
        public decimal NonMenusGst_GST { get; set; }
        public decimal NonMenusGst_Total { get; set; }
    }

    public class Members_MemShipReport
    {
        public decimal MemBill_Sale { get; set; }
        public decimal MemBill_Discount { get; set; }
        public decimal MemBill_GST { get; set; }
        public decimal MemBill_Total { get; set; }
    }

    public class BarReports
    {
        public decimal Bar_Sale { get; set; }
        public decimal Bar_GST { get; set; }
        public decimal Bar_Discount { get; set; }
        public decimal Bar_Total { get; set; }
    }

    public class NonMenusBarReports
    {
        public decimal NonGst_BarSale { get; set; }
        public decimal NonGst_BarDiscount { get; set; }
        public decimal NonGst_BarGST { get; set; }
        public decimal NonGst_BarTotal { get; set; }
    }
}