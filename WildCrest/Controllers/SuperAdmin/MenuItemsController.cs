using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WildCrest.Models.WildCrestModels;

namespace WildCrest.Controllers.SuperAdmin
{
    public class MenuItemsController : Controller
    {
        ClubWildCrestEntities context = new ClubWildCrestEntities();
        // GET: MenuItems
        [Authorize(Roles = "1,2")]
        public ActionResult Index()
        {
            List<MenuItems> items = new List<MenuItems>();
            if (Request.Cookies["PageSetting"] != null)
            {
                if (Request.Cookies["PageSetting"]["MenuItemsPermission"] != "None")
                {
                    var data = context.tbl_Menu.OrderByDescending(a => a.ID).Where(s => s.NewItemApprvFrmSuperAdm != false && s.DelItemApprvFromSuperAdm != true).ToList();

                    foreach (var i in data)
                    {
                        items.Add(new MenuItems()
                        {
                            ID = i.ID,
                            Food_Item_Name = i.Food_Item_Name,
                            Details = i.Details,
                            Price = i.Price,
                            FoodType = i.FoodType,
                            AddedBy = i.AddedBy
                        });
                    }
                    return View(items);
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                var data = context.tbl_Menu.OrderByDescending(a => a.ID).Where(s => s.NewItemApprvFrmSuperAdm != false && s.DelItemApprvFromSuperAdm != true).ToList();

                foreach (var i in data)
                {
                    items.Add(new MenuItems()
                    {
                        ID = i.ID,
                        Food_Item_Name = i.Food_Item_Name,
                        Details = i.Details,
                        Price = i.Price,
                        FoodType = i.FoodType,
                        AddedBy = i.AddedBy
                    });
                }
                return View(items);
            }

        }

        [Authorize(Roles = "1")]
        public ActionResult NewFoodItemApprovalBySuperAdmin()
        {
            var data = context.tbl_Menu.OrderByDescending(a => a.ID).Where(s => s.NewItemApprvFrmSuperAdm == false).ToList();
            List<MenuItems> items = new List<MenuItems>();
            foreach (var i in data)
            {
                items.Add(new MenuItems()
                {
                    ID = i.ID,
                    Food_Item_Name = i.Food_Item_Name,
                    Details = i.Details,
                    Price = i.Price,
                    FoodType = i.FoodType,
                    AddedBy = i.AddedBy
                });
            }
            return View(items);
        }

        [Authorize(Roles = "1")]
        public ActionResult DeleteFoodItemBySuperAdminApproval()
        {
            var data = context.tbl_Menu.OrderByDescending(a => a.ID).Where(s => s.DelItemApprvFromSuperAdm == true).ToList();
            List<MenuItems> items = new List<MenuItems>();
            foreach (var i in data)
            {
                items.Add(new MenuItems()
                {
                    ID = i.ID,
                    Food_Item_Name = i.Food_Item_Name,
                    Details = i.Details,
                    Price = i.Price,
                    FoodType = i.FoodType,
                    DeletedBy = i.DeletedBy
                });
            }
            return View(items);
        }

        [HttpGet]
        [Authorize(Roles = "1,2")]
        public ActionResult AddNewFoodItem()
        {
            List<FoodItemType> typeList = new List<FoodItemType>();
            var data = context.tbl_FoodType.OrderBy(d => d.FoodItemTypeName).ToList();
            foreach (var i in data)
            {
                typeList.Add(new FoodItemType()
                {
                    ID = i.ID,
                    FoodItemTypeName = i.FoodItemTypeName
                });
            }

            List<Inventory> invList = new List<Inventory>();
            //var invData = context.tbl_Inventory.ToList();
            var invData = context.tbl_Inventory.Where(c => !context.tbl_Menu.Select(b => b.InventoryID).Contains(c.ID)).OrderBy(s => s.Item_Name);
            foreach (var inv in invData)
            {
                invList.Add(new Inventory()
                {
                    ID = inv.ID,
                    Item_Name = inv.Item_Name,
                    Type = inv.Type
                });
            }
            ViewBag.InventoryList = invList;

            if (Request.Cookies["PageSetting"] != null)
            {
                if (Request.Cookies["PageSetting"]["MenuItemsPermission"] == "All")
                {
                    return View(typeList);
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                return View(typeList);
            }

        }

        [HttpPost]
        public JsonResult AddNewFoodItem(MenuItems menu)
        {
            int currentUserTypeLoggedin = Convert.ToInt32(Request.Cookies["UserType"].Value);
            tbl_Menu item = new tbl_Menu();
            item.Food_Item_Name = menu.Food_Item_Name;
            item.Details = menu.Details;
            item.Price = menu.Price;
            item.FoodType = menu.FoodType;
            item.AddedBy = Request.Cookies["AddedBy"].Value;
            item.InventoryID = menu.InventoryID;
            if (currentUserTypeLoggedin == 2)
            {
                item.NewItemApprvFrmSuperAdm = false;
            }
            else
            {
                item.NewItemApprvFrmSuperAdm = true;
            }
            item.DelItemApprvFromSuperAdm = false;
            context.tbl_Menu.Add(item);
            context.SaveChanges();
            return Json(currentUserTypeLoggedin);
        }

        [Authorize(Roles = "1,2")]
        public ActionResult FoodItemEditByID(int id, string editDetail)
        {
            List<FoodItemType> typeList = new List<FoodItemType>();
            var data1 = context.tbl_FoodType.ToList();
            foreach (var i in data1)
            {
                typeList.Add(new FoodItemType()
                {
                    ID = i.ID,
                    FoodItemTypeName = i.FoodItemTypeName
                });
            }
            ViewBag.FoodType = typeList;
            if (Request.Cookies["PageSetting"] != null)
            {

                var data = context.tbl_Menu.SingleOrDefault(i => i.ID == id);
                MenuItems item = new MenuItems();
                item.ID = data.ID;
                item.Food_Item_Name = data.Food_Item_Name;
                item.Details = data.Details;
                item.Price = data.Price;
                item.FoodType = data.FoodType;
                if (Request.Cookies["PageSetting"]["MenuItemsPermission"] == "All")
                {
                    if (editDetail == "Details")
                    {
                        return View("~/Views/MenuItems/MenuItemDetailsByID.cshtml", item);
                    }
                    else
                    {
                        return View("~/Views/MenuItems/FoodItemEditByID.cshtml", item);
                    }
                }
                else if (Request.Cookies["PageSetting"]["MenuItemsPermission"] == "View")
                {
                    if (editDetail == "Details")
                    {
                        return View("~/Views/MenuItems/MenuItemDetailsByID.cshtml", item);
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
                var data = context.tbl_Menu.SingleOrDefault(i => i.ID == id);
                MenuItems item = new MenuItems();
                item.ID = data.ID;
                item.Food_Item_Name = data.Food_Item_Name;
                item.Details = data.Details;
                item.Price = data.Price;
                item.FoodType = data.FoodType;
                if (editDetail == "Details")
                {
                    return View("~/Views/MenuItems/MenuItemDetailsByID.cshtml", item);
                }
                else
                {
                    return View("~/Views/MenuItems/FoodItemEditByID.cshtml", item);
                }
            }

        }

        public ActionResult NewDelFoodItemEditByID(int id, string editDetail)
        {
            var data = context.tbl_Menu.SingleOrDefault(i => i.ID == id);
            MenuItems item = new MenuItems();
            item.ID = data.ID;
            item.Food_Item_Name = data.Food_Item_Name;
            item.Details = data.Details;
            item.Price = data.Price;
            item.FoodType = data.FoodType;
            if (editDetail == "New")
            {
                ViewBag.NewDelMenu = true;
            }
            else
            {
                ViewBag.NewDelMenu = false;
            }
            return View(item);
        }

        [HttpPost]
        public JsonResult UpdateDetailsByID(MenuItems menu)
        {
            var data = context.tbl_Menu.Find(menu.ID);
            if (data != null)
            {
                data.Food_Item_Name = menu.Food_Item_Name;
                data.Details = menu.Details;
                data.Price = menu.Price;
                data.FoodType = menu.FoodType;
                context.Entry(data).State = EntityState.Modified;
                context.SaveChanges();
            }
            return Json("Updated.");
        }

        [HttpPost]
        public JsonResult DeleteFoodItemById(int id)
        {
            int currentUserTypeLoggedin = Convert.ToInt32(Request.Cookies["UserType"].Value);
            if (currentUserTypeLoggedin == 1)
            {
                var findUser = context.tbl_Menu.SingleOrDefault(u => u.ID == id);

                if (findUser != null)
                {
                    context.Entry(findUser).State = EntityState.Deleted;
                    context.SaveChanges();
                }
            }
            else
            {
                var findUser = context.tbl_Menu.SingleOrDefault(u => u.ID == id);
                if (findUser != null)
                {
                    findUser.DelItemApprvFromSuperAdm = true;
                    findUser.DeletedBy = Request.Cookies["AddedBy"].Value;
                    context.Entry(findUser).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            return Json(currentUserTypeLoggedin);
        }

        [HttpPost]
        public JsonResult ApproveFoodItem(int id)
        {
            var data = context.tbl_Menu.SingleOrDefault(s => s.ID == id);
            if (data != null)
            {
                data.NewItemApprvFrmSuperAdm = true;
                context.Entry(data).State = EntityState.Modified;
                context.SaveChanges();
            }
            return Json("Approved");
        }

        [HttpPost]
        public JsonResult CancelDeleteFoodItemByID(int id)
        {
            var findUser = context.tbl_Menu.SingleOrDefault(u => u.ID == id);
            if (findUser != null)
            {
                findUser.DelItemApprvFromSuperAdm = false;
                context.Entry(findUser).State = EntityState.Modified;
                context.SaveChanges();
            }
            return Json("NotDeleted.");
        }

        public JsonResult AddNewFoodItemType(string typeName)
        {
            tbl_FoodType typ = new tbl_FoodType();
            typ.FoodItemTypeName = typeName;
            context.tbl_FoodType.Add(typ);
            context.SaveChanges();
            var data = context.tbl_FoodType.OrderBy(s => s.FoodItemTypeName).ToList();
            List<FoodItemType> list = new List<FoodItemType>();
            foreach (var i in data)
            {
                list.Add(new FoodItemType()
                {
                    ID = i.ID,
                    FoodItemTypeName = i.FoodItemTypeName
                });
            }
            return Json(list);
        }

        public JsonResult InventoryDetailsByID(int id)
        {
            var data = context.tbl_Inventory.Find(id);
            Inventory inv = new Inventory();
            if (data != null)
            {
                inv.ID = data.ID;
                inv.Item_Name = data.Item_Name;
                inv.Type = data.Type;
                inv.Price = data.Price;
                inv.Quantity = data.Quantity;
                inv.VendorID = data.VendorID;
                inv.VendorName = context.tbl_Vendors.SingleOrDefault(s => s.ID == data.VendorID).Vendor_First_Name + " " + context.tbl_Vendors.SingleOrDefault(s => s.ID == data.VendorID).Vendor_Last_Name;
            }
            return Json(inv);
        }

        public ActionResult ConsumablesItem()
        {
            //var consumableItemsList = (from menu in context.tbl_Menu
            //                           join consumable in context.tbl_ConsumableItems on menu.ID equals consumable.MenuItem_ID
            //                           select new MenuItems
            //                           {
            //                               ID = menu.ID,
            //                               Food_Item_Name = menu.Food_Item_Name
            //                           }).Distinct().ToList();

            var consumableItemsList = (from menu in context.tbl_Menu
                                       join consumable in context.tbl_ConsumableItems on menu.ID equals consumable.MenuItem_ID
                                       select new MenuItems
                                       {
                                           ID = menu.ID,
                                           Food_Item_Name = menu.Food_Item_Name,
                                           ConsumableID = consumable.ID
                                       }).ToList();
            var lst = consumableItemsList.GroupBy(s => s.ID).OrderByDescending(d => d.Max(a => a.ConsumableID)).Select(f => f.FirstOrDefault()).ToList();


            ViewBag.MenuItemsConsumableList = lst;
            return View();
        }

        public ActionResult AddNewConsumableItem()
        {
            var menuItems_Data = context.tbl_Menu.Where(m => m.InventoryID == 0).ToList();
            var inventoryItems = context.tbl_Inventory.ToList();
            var menuItems = (from m in menuItems_Data
                             where !context.tbl_ConsumableItems.Any(i => i.MenuItem_ID == m.ID)
                             select m).ToList();
            List<MenuItems> menusList = new List<MenuItems>();
            foreach (var v in menuItems)
            {
                menusList.Add(new MenuItems()
                {
                    ID = v.ID,
                    Food_Item_Name = v.Food_Item_Name
                });
            }
            ViewBag.MenuItems = menusList.OrderBy(s=>s.Food_Item_Name).ToList();
            ViewBag.InventoryItems = inventoryItems.OrderBy(s => s.Item_Name).ToList();
            return View();
        }

        [HttpPost]
        public JsonResult SaveConsumableQuantity(List<ConsumableItems> foodList)
        {
            foreach (var i in foodList)
            {
                //var d = context.tbl_ConsumableItems.SingleOrDefault(s => s.MenuItem_ID == i.MenuItem_ID && s.Inventory_ID == i.Inventory_ID);
                //if (d != null)
                //{
                //    d.Quantity += i.Quantity;
                //    context.Entry(d).State = EntityState.Modified;
                //    context.SaveChanges();
                //}
                //else
                //{

                tbl_ConsumableItems consumable = new tbl_ConsumableItems();
                consumable.Inventory_ID = i.Inventory_ID;
                consumable.MenuItem_ID = i.MenuItem_ID;
                consumable.MeasurementUnit = i.Measurement;
                consumable.Quantity = i.Quantity;
                context.tbl_ConsumableItems.Add(consumable);
                context.SaveChanges();
                //}
            }
            return Json("Saved");
        }

        [HttpPost]
        public JsonResult DeleteConsumableItemByMenuId(int menuId)
        {
            var menuItemConsumable = context.tbl_ConsumableItems.Where(a => a.MenuItem_ID == menuId).ToList();
            foreach (var i in menuItemConsumable)
            {
                context.Entry(i).State = EntityState.Deleted;
                context.SaveChanges();
            }
            return Json("Deleted");
        }

        [HttpPost]
        public JsonResult DetailsOfConsumableItemByMenuId(int menuId)
        {
            try
            {
                var menuItemConsumable = context.tbl_ConsumableItems.Where(a => a.MenuItem_ID == menuId).ToList();
                List<ConsumableItems> items = new List<ConsumableItems>();
                foreach (var i in menuItemConsumable)
                {
                    var invName = context.tbl_Inventory.SingleOrDefault(s => s.ID == i.Inventory_ID);
                    //items.Add(new ConsumableItems()
                    //{
                    //    Inventory_ItemName = (invName != null) ? invName.Item_Name : "",
                    //    Quantity = i.Quantity,
                    //    Measurement = i.MeasurementUnit
                    //});
                    bool alreadyExists = items.Any(x => x.MenuItem_ID == i.MenuItem_ID && x.Inventory_ID == i.Inventory_ID);
                    if (!alreadyExists)
                    {
                        items.Add(new ConsumableItems()
                        {
                            Inventory_ItemName = (invName != null) ? invName.Item_Name : "",
                            Quantity = i.Quantity,
                            Measurement = i.MeasurementUnit,
                            Inventory_ID = i.Inventory_ID,
                            MenuItem_ID = i.MenuItem_ID,
                            Price = (context.tbl_Inventory.FirstOrDefault(a => a.ID == i.Inventory_ID).Price * i.Quantity).ToString()
                        });

                    }
                    else
                    {
                        items.Find(p => p.MenuItem_ID == i.MenuItem_ID && p.Inventory_ID == i.Inventory_ID).Quantity += i.Quantity;
                    }
                }
                return Json(items);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return Json("");
        }

        public ActionResult ConsumableFoodItemEditByID(int menuId)
        {
            var menuItemConsumable = context.tbl_ConsumableItems.Where(a => a.MenuItem_ID == menuId).ToList();
            List<ConsumableItems> items = new List<ConsumableItems>();
            string mName = "";
            foreach (var i in menuItemConsumable)
            {
                var invName = context.tbl_Inventory.SingleOrDefault(s => s.ID == i.Inventory_ID);
                var menuName = context.tbl_Menu.SingleOrDefault(s => s.ID == i.MenuItem_ID);
                items.Add(new ConsumableItems()
                {
                    Inventory_ItemName = (invName != null) ? invName.Item_Name : "",
                    Quantity = i.Quantity,
                    Measurement = i.MeasurementUnit,
                    ID = i.ID,
                    MenuItem_ID = i.MenuItem_ID,
                });
                mName = (menuName != null) ? menuName.Food_Item_Name : "";
            }
            ViewBag.Menu_Item_Name = mName;
            ViewBag.Menu_Item_ID = menuId;
            //------------------------------------------------------------------------------
            var inventoryItems = context.tbl_Inventory.ToList();
            var invItems = (from m in inventoryItems
                            where !context.tbl_ConsumableItems.Any(i => i.MenuItem_ID == menuId && i.Inventory_ID == m.ID)
                             select m).ToList();
            List<MenuItems> invList = new List<MenuItems>();
            foreach (var v in invItems)
            {
                invList.Add(new MenuItems()
                {
                    ID = v.ID,
                    Food_Item_Name = v.Item_Name
                });
            }
            ViewBag.InventoryItems = invList.OrderBy(s => s.Food_Item_Name).ToList();
            return View(items);
        }

        [HttpPost]
        public JsonResult UpdateQuantityOfConsumableItem(int idOfConsumableItem, double? quantity)
        {
            var item = context.tbl_ConsumableItems.Find(idOfConsumableItem);
            if (item != null)
            {
                item.Quantity = (quantity != null) ? quantity : 0;
                context.Entry(item).State = EntityState.Modified;
                context.SaveChanges();
                return Json("Quantity Updated.");
            }
            return Json("Data not Found.");
        }

        [HttpPost]
        public JsonResult DeleteQuantityOfConsumableItem(List<int> selectedItems)
        {
            foreach (var id in selectedItems)
            {
                var data = context.tbl_ConsumableItems.SingleOrDefault(s => s.ID == id);
                if (data != null)
                {
                    context.Entry(data).State = EntityState.Deleted;
                    context.SaveChanges();
                }
            }
            return Json("Deleted.");
        }

        [HttpPost]
        public JsonResult getMeasurementUnit(int inventoryID)
        {
            var mUnit = context.tbl_Inventory.SingleOrDefault(s => s.ID == inventoryID);
            string unit = "";
            if (mUnit != null)
            {
                unit = mUnit.Measurement;
            }
            return Json(unit);
        }
    }
}