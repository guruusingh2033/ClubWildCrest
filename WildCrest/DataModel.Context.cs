﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WildCrest
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ClubWildCrestEntities : DbContext
    {
        public ClubWildCrestEntities()
            : base("name=ClubWildCrestEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<tbl_BillingDetails> tbl_BillingDetails { get; set; }
        public DbSet<tbl_BookingTable> tbl_BookingTable { get; set; }
        public DbSet<tbl_Events> tbl_Events { get; set; }
        public DbSet<tbl_EventsImgVideo> tbl_EventsImgVideo { get; set; }
        public DbSet<tbl_Groups> tbl_Groups { get; set; }
        public DbSet<tbl_MembersWhileStayingWithUser> tbl_MembersWhileStayingWithUser { get; set; }
        public DbSet<tbl_Roles> tbl_Roles { get; set; }
        public DbSet<tbl_UserGroup> tbl_UserGroup { get; set; }
        public DbSet<tbl_UsersOrder> tbl_UsersOrder { get; set; }
        public DbSet<tbl_VendorOrders> tbl_VendorOrders { get; set; }
        public DbSet<tbl_Vendors> tbl_Vendors { get; set; }
        public DbSet<tbl_Voucher> tbl_Voucher { get; set; }
        public DbSet<tbl_VoucherUsedByUser> tbl_VoucherUsedByUser { get; set; }
        public DbSet<tbl_FoodType> tbl_FoodType { get; set; }
        public DbSet<tbl_Menu> tbl_Menu { get; set; }
        public DbSet<tbl_Rooms> tbl_Rooms { get; set; }
        public DbSet<tbl_UserMembership> tbl_UserMembership { get; set; }
        public DbSet<tbl_MembershipPlans> tbl_MembershipPlans { get; set; }
        public DbSet<tbl_MembersBillingDetails> tbl_MembersBillingDetails { get; set; }
        public DbSet<tbl_MembersPhoto> tbl_MembersPhoto { get; set; }
        public DbSet<tbl_MembersDocs> tbl_MembersDocs { get; set; }
        public DbSet<tbl_Inventory> tbl_Inventory { get; set; }
        public DbSet<tbl_MenusBillingDetailsWithBillNo> tbl_MenusBillingDetailsWithBillNo { get; set; }
        public DbSet<tbl_UsersStay> tbl_UsersStay { get; set; }
        public DbSet<tbl_RoomBilling> tbl_RoomBilling { get; set; }
        public DbSet<tbl_NonGST_MenusBillDetWithBillNo> tbl_NonGST_MenusBillDetWithBillNo { get; set; }
        public DbSet<tbl_TablesForBooking> tbl_TablesForBooking { get; set; }
        public DbSet<tbl_RoomBooking_Details> tbl_RoomBooking_Details { get; set; }
        public DbSet<tbl_NonGST_MenusBillingSection> tbl_NonGST_MenusBillingSection { get; set; }
        public DbSet<tbl_PermissionsToStaff> tbl_PermissionsToStaff { get; set; }
        public DbSet<tbl_Profile> tbl_Profile { get; set; }
        public DbSet<tbl_ConsumableItems> tbl_ConsumableItems { get; set; }
        public DbSet<tbl_RoomBooking> tbl_RoomBooking { get; set; }
        public DbSet<tbl_InventoryUsage> tbl_InventoryUsage { get; set; }
        public DbSet<tbl_MenusBillingSection> tbl_MenusBillingSection { get; set; }
    }
}
