using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WildCrest.Models.WildCrestModels;

namespace WildCrest.Controllers.SuperAdmin
{
    public class VendorsController : Controller
    {
        ClubWildCrestEntities context = new ClubWildCrestEntities();
        // GET: Vendors
        [Authorize(Roles = "1,2")]
        public ActionResult Index()
        {
            List<Vendors> lst = new List<Vendors>();
            if (Request.Cookies["PageSetting"] != null)
            {
                if (Request.Cookies["PageSetting"]["VendorsPermission"] != "None")
                {
                    var VendorsList = context.tbl_Vendors.OrderByDescending(a => a.ID).Where(s => s.IsNewApproved != false && s.isDeleted != true).ToList();
                    foreach (var i in VendorsList)
                    {
                        lst.Add(new Vendors()
                        {
                            ID = i.ID,
                            Vendor_First_Name = i.Vendor_First_Name,
                            Vendor_Last_Name = i.Vendor_Last_Name,
                            Address = i.Address,
                            EmailID = i.EmailID,
                            Contact_No = i.Contact_No,
                            City = i.City,
                            State = i.State,
                            AddedBy = i.AddedBy
                        });
                    }
                    return View(lst);
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                var VendorsList = context.tbl_Vendors.OrderByDescending(a => a.ID).Where(s => s.IsNewApproved != false && s.isDeleted != true).ToList();
                foreach (var i in VendorsList)
                {
                    lst.Add(new Vendors()
                    {
                        ID = i.ID,
                        Vendor_First_Name = i.Vendor_First_Name,
                        Vendor_Last_Name = i.Vendor_Last_Name,
                        Address = i.Address,
                        EmailID = i.EmailID,
                        Contact_No = i.Contact_No,
                        City = i.City,
                        State = i.State,
                        AddedBy = i.AddedBy
                    });
                }
                return View(lst);
            }
            
           
        }

        [Authorize(Roles = "1,2")]
        public ActionResult AddNewVendor()
        {
            if (Request.Cookies["PageSetting"] != null)
            {
                if (Request.Cookies["PageSetting"]["VendorsPermission"] == "All")
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
        public JsonResult AddNewVendor(Vendors vendor)
        {
            int currentUserTypeLoggedin = Convert.ToInt32(Request.Cookies["UserType"].Value);
            tbl_Vendors prf = new tbl_Vendors();
            prf.Vendor_First_Name = vendor.Vendor_First_Name;
            prf.Vendor_Last_Name = vendor.Vendor_Last_Name;
            prf.Address = vendor.Address;
            prf.EmailID = vendor.EmailID;
            prf.Contact_No = vendor.Contact_No;
            prf.City = vendor.City;
            prf.State = vendor.State;
            prf.AddedBy= Request.Cookies["AddedBy"].Value;
            //if (currentUserTypeLoggedin == 2)
            //{
            //    prf.IsNewApproved = false;
            //}
            //else
            //{
            //    prf.IsNewApproved = true;
            //}
            prf.IsNewApproved = true;
            prf.isDeleted = false;
            context.tbl_Vendors.Add(prf);
            context.SaveChanges();
            
            return Json(currentUserTypeLoggedin);
        }

        [Authorize(Roles = "1,2")]
        public ActionResult EditVendorByID(int id,string editDetail)
        {
            if (Request.Cookies["PageSetting"] != null)
            {
                
                var data = context.tbl_Vendors.Find(id);
                Vendors prf = new Vendors();
                prf.ID = data.ID;
                prf.Vendor_First_Name = data.Vendor_First_Name;
                prf.Vendor_Last_Name = data.Vendor_Last_Name;
                prf.Address = data.Address;
                prf.EmailID = data.EmailID;
                prf.Contact_No = data.Contact_No;
                prf.City = data.City;
                prf.State = data.State;
                if (Request.Cookies["PageSetting"]["VendorsPermission"] == "All")
                {
                    if (editDetail == "Details")
                    {
                        return View("~/Views/Vendors/VendorDetailsByID.cshtml", prf);
                    }
                    else
                    {
                        return View("~/Views/Vendors/EditVendorByID.cshtml", prf);
                    }
                }
                else if (Request.Cookies["PageSetting"]["VendorsPermission"] == "View")
                {
                    if (editDetail == "Details")
                    {
                        return View("~/Views/Vendors/VendorDetailsByID.cshtml", prf);
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
                var data = context.tbl_Vendors.Find(id);
                Vendors prf = new Vendors();
                prf.ID = data.ID;
                prf.Vendor_First_Name = data.Vendor_First_Name;
                prf.Vendor_Last_Name = data.Vendor_Last_Name;
                prf.Address = data.Address;
                prf.EmailID = data.EmailID;
                prf.Contact_No = data.Contact_No;
                prf.City = data.City;
                prf.State = data.State;
                if (editDetail == "Details")
                {
                    return View("~/Views/Vendors/VendorDetailsByID.cshtml", prf);
                }
                else
                {
                    return View("~/Views/Vendors/EditVendorByID.cshtml", prf);
                }
            }
                      
        }

        [Authorize(Roles = "1,2")]
        public ActionResult NewDelVendorByID(int id, string editDetail)
        {
            var data = context.tbl_Vendors.Find(id);
            Vendors prf = new Vendors();
            prf.ID = data.ID;
            prf.Vendor_First_Name = data.Vendor_First_Name;
            prf.Vendor_Last_Name = data.Vendor_Last_Name;
            prf.Address = data.Address;
            prf.EmailID = data.EmailID;
            prf.Contact_No = data.Contact_No;
            prf.City = data.City;
            prf.State = data.State;
            if (editDetail == "New")
            {
                ViewBag.NewDelVendor = true;
            }
            else
            {
                ViewBag.NewDelVendor = false;
            }
            return View(prf);
        }

        [HttpPost]
        public JsonResult EditVendorDetailsByID(Vendors vendor)
        {
            var data = context.tbl_Vendors.Find(vendor.ID);
            if (data != null)
            {
                data.Vendor_First_Name = vendor.Vendor_First_Name;
                data.Vendor_Last_Name = vendor.Vendor_Last_Name;
                data.Address = vendor.Address;
                data.EmailID = vendor.EmailID;
                data.Contact_No = vendor.Contact_No;
                data.City = vendor.City;
                data.State = vendor.State;
                context.Entry(data).State = EntityState.Modified;
                context.SaveChanges();
            }
            return Json("Updated.");
        }

        [HttpPost]
        public JsonResult DeleteVendorByID(int id)
        {
            int currentUserTypeLoggedin = Convert.ToInt32(Request.Cookies["UserType"].Value);
            if (currentUserTypeLoggedin == 1)
            {
                var findUser = context.tbl_Vendors.SingleOrDefault(u => u.ID == id);
                if (findUser != null)
                {
                    var inventory = context.tbl_Inventory.Where(w => w.VendorID == id).ToList();
                    foreach(var inv in inventory)
                    {
                        context.Entry(inv).State = EntityState.Deleted;
                        context.SaveChanges();
                    }
                    var vendorOrders = context.tbl_VendorOrders.Where(o => o.VendorID == id).ToList();
                    if (vendorOrders != null)
                    {
                        foreach (var vo in vendorOrders)
                        {
                            context.Entry(vo).State = EntityState.Deleted;
                            context.SaveChanges();
                        }
                    }
                    context.Entry(findUser).State = EntityState.Deleted;
                    context.SaveChanges();
                }
            }
            else
            {
                var findUser = context.tbl_Vendors.SingleOrDefault(u => u.ID == id);
                if (findUser != null)
                {
                    findUser.isDeleted = true;
                    findUser.DeletedBy= Request.Cookies["AddedBy"].Value;
                    context.Entry(findUser).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            return Json(currentUserTypeLoggedin);
        }

        [Authorize(Roles = "1")]
        public ActionResult ApproveNewVendor()
        {
            List<Vendors> lst = new List<Vendors>();
            var VendorsList = context.tbl_Vendors.OrderByDescending(a => a.ID).Where(s => s.IsNewApproved == false).ToList();
            foreach (var i in VendorsList)
            {
                lst.Add(new Vendors()
                {
                    ID = i.ID,
                    Vendor_First_Name = i.Vendor_First_Name,
                    Vendor_Last_Name = i.Vendor_Last_Name,
                    Address = i.Address,
                    EmailID = i.EmailID,
                    Contact_No = i.Contact_No,
                    City = i.City,
                    State = i.State,
                    AddedBy=i.AddedBy
                });
            }
            return View(lst);
        }

        [HttpPost]
        public JsonResult ApproveVendorByID(int id)
        {
            var data = context.tbl_Vendors.SingleOrDefault(s => s.ID == id);
            if (data != null)
            {
                data.IsNewApproved = true;
                context.Entry(data).State = EntityState.Modified;
                context.SaveChanges();
            }
            return Json("Approved");
        }

        [Authorize(Roles = "1")]
        public ActionResult ApproveToDeleteVendor()
        {
            List<Vendors> lst = new List<Vendors>();
            var VendorsList = context.tbl_Vendors.OrderByDescending(a => a.ID).Where(s => s.isDeleted == true).ToList();
            foreach (var i in VendorsList)
            {
                lst.Add(new Vendors()
                {
                    ID = i.ID,
                    Vendor_First_Name = i.Vendor_First_Name,
                    Vendor_Last_Name = i.Vendor_Last_Name,
                    Address = i.Address,
                    EmailID = i.EmailID,
                    Contact_No = i.Contact_No,
                    City = i.City,
                    State = i.State,
                    DeletedBy=i.DeletedBy
                });
            }
            return View(lst);
        }

        [HttpPost]
        public JsonResult CancelToDeleteVendorById(int id)
        {
            var findUser = context.tbl_Vendors.SingleOrDefault(u => u.ID == id);
            if (findUser != null)
            {
                findUser.isDeleted = false;
                context.Entry(findUser).State = EntityState.Modified;
                context.SaveChanges();
            }
            return Json("NotDeleted.");
        }

        public ActionResult OrderProductByVendorId(int id)
        {
            ViewBag.VendorID = id;
            return View();
        }

        [HttpPost]
        public JsonResult OrderProductBy_VendorId(VendorOrders vendorOrder)
        {
            tbl_VendorOrders order = new tbl_VendorOrders();
            order.VendorID = vendorOrder.VendorID;
            order.Product_Name = vendorOrder.Product_Name;
            order.Quantity = vendorOrder.Quantity;
            order.Price = vendorOrder.Price;
            context.tbl_VendorOrders.Add(order);
            context.SaveChanges();
            return Json("Ordered");
        }

        public ActionResult ViewOrders()
        {
            var data = from o in context.tbl_VendorOrders
                       join vd in context.tbl_Vendors on o.VendorID equals vd.ID
                       select new
                       {
                           VendorOrderID = o.ID,
                           VendorName = vd.Vendor_First_Name + " " + vd.Vendor_Last_Name,
                           ContactNo = vd.Contact_No,
                           ProductName = o.Product_Name,
                           Quantity = o.Quantity,
                           Price = o.Price
                       };
            List<ViewOrders> lst = new List<ViewOrders>();
            foreach(var i in data)
            {
                lst.Add(new ViewOrders()
                {
                    VendorOrderID = i.VendorOrderID,
                    Vendor_Full_Name = i.VendorName,
                    Contact_No = i.ContactNo,
                    Product_Name = i.ProductName,
                    Quantity = i.Quantity,
                    Price = i.Price
                });
            }
            return View(lst);
        }

        public ActionResult TrackOrderByVendorId(int id)
        {
            var data = context.tbl_Inventory.Where(w => w.VendorID == id).ToList();
            List<Inventory> invList = new List<Inventory>();
            if (data != null)
            {
                foreach (var i in data)
                {
                    invList.Add(new Inventory()
                    {
                        ID = i.ID,
                        Item_Name = i.Item_Name,
                        Type = i.Type,
                        Price = i.Price,
                        Quantity = i.Quantity,
                        VendorID = i.VendorID,
                        VendorName = context.tbl_Vendors.SingleOrDefault(s => s.ID == i.VendorID).Vendor_First_Name + " " + context.tbl_Vendors.SingleOrDefault(s => s.ID == i.VendorID).Vendor_Last_Name
                    });
                }
            }
            return View(invList);
        }
    }
}