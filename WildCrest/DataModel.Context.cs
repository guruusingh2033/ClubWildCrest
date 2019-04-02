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
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
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
    
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<tbl_BarBillingDetailsWithBillNo> tbl_BarBillingDetailsWithBillNo { get; set; }
        public DbSet<tbl_BarBillingSection> tbl_BarBillingSection { get; set; }
        public DbSet<tbl_BarInventory> tbl_BarInventory { get; set; }
        public DbSet<tbl_BarInventoryUsage> tbl_BarInventoryUsage { get; set; }
        public DbSet<tbl_BarMenu> tbl_BarMenu { get; set; }
        public DbSet<tbl_BillingDetails> tbl_BillingDetails { get; set; }
        public DbSet<tbl_BookingTable> tbl_BookingTable { get; set; }
        public DbSet<tbl_Buffet_ConsumableItems> tbl_Buffet_ConsumableItems { get; set; }
        public DbSet<tbl_Buffet_Menu> tbl_Buffet_Menu { get; set; }
        public DbSet<tbl_Consumable_BarItems> tbl_Consumable_BarItems { get; set; }
        public DbSet<tbl_Consumable_WineItems> tbl_Consumable_WineItems { get; set; }
        public DbSet<tbl_ConsumableItems> tbl_ConsumableItems { get; set; }
        public DbSet<tbl_Events> tbl_Events { get; set; }
        public DbSet<tbl_EventsImgVideo> tbl_EventsImgVideo { get; set; }
        public DbSet<tbl_FoodType> tbl_FoodType { get; set; }
        public DbSet<tbl_Groups> tbl_Groups { get; set; }
        public DbSet<tbl_Inventory> tbl_Inventory { get; set; }
        public DbSet<tbl_InventoryUsage> tbl_InventoryUsage { get; set; }
        public DbSet<tbl_MembersBillingDetails> tbl_MembersBillingDetails { get; set; }
        public DbSet<tbl_MembersDocs> tbl_MembersDocs { get; set; }
        public DbSet<tbl_MembershipPlans> tbl_MembershipPlans { get; set; }
        public DbSet<tbl_MembersPhoto> tbl_MembersPhoto { get; set; }
        public DbSet<tbl_MembersWhileStayingWithUser> tbl_MembersWhileStayingWithUser { get; set; }
        public DbSet<tbl_Menu> tbl_Menu { get; set; }
        public DbSet<tbl_MenusBillingDetailsWithBillNo> tbl_MenusBillingDetailsWithBillNo { get; set; }
        public DbSet<tbl_MenusBillingSection> tbl_MenusBillingSection { get; set; }
        public DbSet<tbl_NonGST_BarBillDetWithBillNo> tbl_NonGST_BarBillDetWithBillNo { get; set; }
        public DbSet<tbl_NonGST_BarBillingSection> tbl_NonGST_BarBillingSection { get; set; }
        public DbSet<tbl_NonGST_MenusBillDetWithBillNo> tbl_NonGST_MenusBillDetWithBillNo { get; set; }
        public DbSet<tbl_NonGST_MenusBillingSection> tbl_NonGST_MenusBillingSection { get; set; }
        public DbSet<tbl_NonGST_WineBillDetWithBillNo> tbl_NonGST_WineBillDetWithBillNo { get; set; }
        public DbSet<tbl_NonGST_WineBillingSection> tbl_NonGST_WineBillingSection { get; set; }
        public DbSet<tbl_Party> tbl_Party { get; set; }
        public DbSet<tbl_Party_FoodMenu> tbl_Party_FoodMenu { get; set; }
        public DbSet<tbl_PartyBilling> tbl_PartyBilling { get; set; }
        public DbSet<tbl_PermissionsToStaff> tbl_PermissionsToStaff { get; set; }
        public DbSet<tbl_Profile> tbl_Profile { get; set; }
        public DbSet<tbl_Roles> tbl_Roles { get; set; }
        public DbSet<tbl_RoomBilling> tbl_RoomBilling { get; set; }
        public DbSet<tbl_RoomBooking> tbl_RoomBooking { get; set; }
        public DbSet<tbl_RoomBooking_Details> tbl_RoomBooking_Details { get; set; }
        public DbSet<tbl_Rooms> tbl_Rooms { get; set; }
        public DbSet<tbl_TablesForBooking> tbl_TablesForBooking { get; set; }
        public DbSet<tbl_UserGroup> tbl_UserGroup { get; set; }
        public DbSet<tbl_UserMembership> tbl_UserMembership { get; set; }
        public DbSet<tbl_UsersOrder> tbl_UsersOrder { get; set; }
        public DbSet<tbl_UsersStay> tbl_UsersStay { get; set; }
        public DbSet<tbl_VendorOrders> tbl_VendorOrders { get; set; }
        public DbSet<tbl_Vendors> tbl_Vendors { get; set; }
        public DbSet<tbl_Voucher> tbl_Voucher { get; set; }
        public DbSet<tbl_VoucherUsedByUser> tbl_VoucherUsedByUser { get; set; }
        public DbSet<tbl_WineBillingDetailsWithBillNo> tbl_WineBillingDetailsWithBillNo { get; set; }
        public DbSet<tbl_WineBillingSection> tbl_WineBillingSection { get; set; }
        public DbSet<tbl_WineInventory> tbl_WineInventory { get; set; }
        public DbSet<tbl_WineInventoryUsage> tbl_WineInventoryUsage { get; set; }
        public DbSet<tbl_WineMenu> tbl_WineMenu { get; set; }
    
        public virtual int checkAvailabilityOfRooms(string checkin, string checkout)
        {
            var checkinParameter = checkin != null ?
                new ObjectParameter("checkin", checkin) :
                new ObjectParameter("checkin", typeof(string));
    
            var checkoutParameter = checkout != null ?
                new ObjectParameter("checkout", checkout) :
                new ObjectParameter("checkout", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("checkAvailabilityOfRooms", checkinParameter, checkoutParameter);
        }
    
        public virtual ObjectResult<Members_Checked_Out_Rooms_Result> Members_Checked_Out_Rooms(string checkin, string checkout)
        {
            var checkinParameter = checkin != null ?
                new ObjectParameter("checkin", checkin) :
                new ObjectParameter("checkin", typeof(string));
    
            var checkoutParameter = checkout != null ?
                new ObjectParameter("checkout", checkout) :
                new ObjectParameter("checkout", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Members_Checked_Out_Rooms_Result>("Members_Checked_Out_Rooms", checkinParameter, checkoutParameter);
        }
    
        public virtual ObjectResult<staysAccToYearlyMembership_Result> staysAccToYearlyMembership(Nullable<int> userID, Nullable<int> membershipID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("userID", userID) :
                new ObjectParameter("userID", typeof(int));
    
            var membershipIDParameter = membershipID.HasValue ?
                new ObjectParameter("membershipID", membershipID) :
                new ObjectParameter("membershipID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<staysAccToYearlyMembership_Result>("staysAccToYearlyMembership", userIDParameter, membershipIDParameter);
        }
    }
}
