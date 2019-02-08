using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WildCrest.Models.WildCrestModels;

namespace WildCrest.Controllers.User
{
    public class FoodBeveragesController : Controller
    {
        ClubWildCrestEntities context = new ClubWildCrestEntities();
        // GET: FoodBeverages
        public ActionResult Index()
        {
            var food = context.tbl_Menu.ToList();
            List<MenuItems> items = new List<MenuItems>();
            if (food != null)
            {
                foreach (var f in food)
                {
                    items.Add(new MenuItems()
                    {
                        ID = f.ID,
                        FoodType = f.FoodType,
                        Food_Item_Name = f.Food_Item_Name,
                        Details = f.Details,
                        Price = f.Price
                    });
                }
            }            
            return View(items);
        }
    }
}