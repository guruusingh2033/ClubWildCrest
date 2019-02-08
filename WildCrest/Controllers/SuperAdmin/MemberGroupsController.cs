using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WildCrest.Models.WildCrestModels;

namespace WildCrest.Controllers.SuperAdmin
{
    public class MemberGroupsController : Controller
    {
        ClubWildCrestEntities context = new ClubWildCrestEntities();
        // GET: MemberGroups
        [Authorize(Roles = "1,2")]
        public ActionResult Index()
        {
            var data = context.tbl_Groups.ToList();
            List<MemberGroup> grp = new List<MemberGroup>();
            foreach(var i in data)
            {
                grp.Add(new MemberGroup() {
                    ID = i.ID,
                    GroupName = i.GroupName,
                    GroupDetail = i.GroupDetail,
                    NoOfMembers = context.tbl_UserGroup.Where(s => s.GroupID == i.ID).Count()
                });
            }
            
            return View(grp);
        }

        [Authorize(Roles = "1,2")]
        public ActionResult AddNewGroup()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CreateNewGroup(MemberGroup group)
        {
            tbl_Groups grp = new tbl_Groups();
            grp.GroupName = group.GroupName;
            grp.GroupDetail = group.GroupDetail;
            context.tbl_Groups.Add(grp);
            context.SaveChanges();
            return Json("Added.");
        }

        [Authorize(Roles = "1,2")]
        public ActionResult GroupDetailsByID(int id,string editDetail)
        {
            var data = context.tbl_Groups.SingleOrDefault(s => s.ID == id);
            MemberGroup grp = new MemberGroup();
            grp.ID = data.ID;
            grp.GroupName = data.GroupName;
            grp.GroupDetail = data.GroupDetail;
            grp.NoOfMembers = context.tbl_UserGroup.Where(s => s.GroupID == data.ID).Count();
            var groupMembersName = context.tbl_UserGroup.Where(s => s.GroupID == data.ID).ToList();
            List<KeyValuePair<int, string>> names = new List<KeyValuePair<int, string>>();
            foreach (var item in groupMembersName)
            {
                names.Add(new KeyValuePair<int, string>(context.tbl_Profile.SingleOrDefault(a => a.ID == item.UserID).ID, context.tbl_Profile.SingleOrDefault(a => a.ID == item.UserID).F_Name + " " + context.tbl_Profile.SingleOrDefault(a => a.ID == item.UserID).L_Name));
            }
            grp.MembersNameWithId = names;
            if (editDetail== "Details")
            {
                return View("~/Views/MemberGroups/ViewGroupById.cshtml",grp);
            }
            else
            {
                return View("~/Views/MemberGroups/EditGroupById.cshtml", grp);
            }
        }

        [HttpPost]
        public JsonResult DeleteGroupById(int id)
        {
            var data = context.tbl_Groups.SingleOrDefault(s => s.ID == id);
            var usersgrp = context.tbl_UserGroup.Where(s => s.GroupID == id).ToList();
            if (data != null)
            {
                context.Entry(data).State = EntityState.Deleted;
                if (usersgrp != null)
                {
                    foreach(var i in usersgrp)
                    {
                        context.Entry(i).State = EntityState.Deleted;
                    }
                }
                context.SaveChanges();
            }
            return Json("Deleted.");
        }

        [HttpPost]
        public JsonResult UpdateGroupById(MemberGroup updGroup)
        {
            var data = context.tbl_Groups.Find(updGroup.ID);
            if (data != null)
            {
                data.GroupName = updGroup.GroupName;
                data.GroupDetail = updGroup.GroupDetail;
                context.Entry(data).State = EntityState.Modified;
                context.SaveChanges();
            }
            return Json("Updated.");
        }

        [Authorize(Roles = "1,2")]
        public ActionResult AddMemberInGroup(int groupID)
        {
            var data = context.tbl_Profile.Where(s => s.UserType == 3).OrderBy(d => d.F_Name).ToList();

            var usrData = (from user in data
                           where !context.tbl_UserGroup.Any(f => f.UserID == user.ID && f.GroupID == groupID)
                           select new
                           {
                               ID = user.ID,
                               F_Name = user.F_Name,
                               L_Name = user.L_Name
                           }).ToList();

            List<Profile> prf = new List<Profile>();
            foreach (var i in usrData)
            {
                prf.Add(new Profile()
                {
                    ID = i.ID,
                    F_Name = i.F_Name,
                    L_Name = i.L_Name
                });
            }
            var groupName = context.tbl_Groups.SingleOrDefault(a => a.ID == groupID).GroupName;
            ViewBag.GroupID = groupID;
            ViewBag.GroupName = groupName;
            return View(prf);
        }

        [HttpPost]
        public JsonResult addUserInGroup(string groupID,List<string> usersLst)
         {
            int gID = Convert.ToInt32(groupID);
            for(var i = 0; i < usersLst.Count(); i++)
            {
                tbl_UserGroup usrGrp = new tbl_UserGroup();
                usrGrp.GroupID = gID;
                usrGrp.UserID = Convert.ToInt32(usersLst[i]);
                context.tbl_UserGroup.Add(usrGrp);
                context.SaveChanges();
            }
            return Json("AddedInGroup");
        }
        [HttpPost]
        public JsonResult RemoveUserFromGroup(int userID, int GroupID)
        {
            var data = context.tbl_UserGroup.Where(u => u.UserID == userID && u.GroupID == GroupID).ToList();
            foreach (var d in data)
            {
                context.Entry(d).State = EntityState.Deleted;
                context.SaveChanges();
            }
            return Json("User Deleted from Group.");
        }
    }
}