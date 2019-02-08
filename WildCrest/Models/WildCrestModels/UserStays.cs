using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WildCrest.Models.WildCrestModels
{
    public class UserStays
    {
        ClubWildCrestEntities context = new ClubWildCrestEntities();

        public int ID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string CheckInDate { get; set; }
        public string CheckoutDate { get; set; }
        public string Username { get; set; }
        public Nullable<int> PreviousStays { get; set; }
        public Nullable<int> LapseStays { get; set; }
        public Nullable<int> MemberID { get; set; }
        public string MembershipName { get; set; }
        public Nullable<int> NoOfMembers { get; set; }
        public Nullable<int> TotalStays { get; set; }

        public Nullable<int> NoOfNightStays { get; set; }
        public Nullable<int> ComplementaryStays { get; set; }

        public Nullable<double> TotalAmountWithoutTax { get; set; }
        public int? RoomId { get; set; }
        public string RoomNo { get; set; }

        public string MemberOrNot { get; set; }
        public Nullable<double> AdvancedPayment { get; set; }

        public UserStays getMembershipDetailsbyUserID(int? id, int stayID)
        {
            UserStays us = new UserStays();
            int? TotalStays = 0;
            int? pStay = 0;
            int? lStay = 0;
            var usermember = context.tbl_UserMembership.SingleOrDefault(s => s.UserID == id);
            if (usermember != null)
            {
                var date = DateTime.Today;
                string currentDate = date.ToString(@"MM\/dd\/yyyy");

                string[] arrDate = usermember.MembershipExpiryDate.Split('/');

                DateTime MemShipExpiryDate = new DateTime(Convert.ToInt32(arrDate[2]), Convert.ToInt32(arrDate[0]), Convert.ToInt32(arrDate[1]));

                if (DateTime.Today > MemShipExpiryDate)
                {
                    lStay = -2;
                }
                else
                {

                    TotalStays = context.tbl_MembershipPlans.SingleOrDefault(a => a.ID == usermember.MembershipID).NoOfStays;
                    var data1 = context.Database.SqlQuery<UserStays>("exec staysAccToYearlyMembership @userID={0},@membershipID={1}", id, usermember.MembershipID).ToList<UserStays>();

                    var filteredData = (data1.Where(p => p.ID != stayID)).ToList();
                    foreach (var i in filteredData)
                    {
                        pStay = pStay + i.ComplementaryStays;
                    }

                    if ((TotalStays - pStay) > 0)
                    {
                        lStay = TotalStays - pStay;
                    }
                }

            }
            else
            {
                lStay = -1;
            }
            us.LapseStays = lStay;
            us.PreviousStays = pStay;
            us.TotalStays = TotalStays;
            us.MemberOrNot = (context.tbl_Profile.SingleOrDefault(w => w.ID == id).Walk_In_Member) == false ? "Yes" : "No";

            return us;
        }
    }

    public class MultipleRoomBookings
    {
        public int roomId { get; set; }
        public int noOfPerson { get; set; }
        public string complementaryStays { get; set; }
        public double amtWithoutTax { get; set; }
    }
}