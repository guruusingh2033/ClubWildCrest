using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WildCrest.Models.WildCrestModels;

namespace WildCrest.Controllers.SuperAdmin
{
    public class EventsController : Controller
    {
        ClubWildCrestEntities context = new ClubWildCrestEntities();
        // GET: Events
        [Authorize(Roles = "1,2")]
        public ActionResult Index()
        {
            List<Event> evtList = new List<Event>();
            if (Request.Cookies["PageSetting"] != null)
            {
                if (Request.Cookies["PageSetting"]["EventsPermission"] != "None")
                {
                    var data = context.tbl_Events.OrderByDescending(s => s.EventDate).Where(a => a.NewEventsApprvFrmSuper != false && a.DelEventsApprvFrmSuper != true).ToList();

                    foreach (var i in data)
                    {
                        evtList.Add(new Event()
                        {
                            EventName = i.EventName,
                            EventDate = i.EventDate,
                            EventTime = i.EventTime,
                            Description = i.Description,
                            OrganisedBy = i.OrganisedBy,
                            Place = i.Place,
                            ID = i.ID,
                            ImagePath = i.ImageUrl,
                            AddedBy = i.AddedBy
                        });
                    }
                    return View(evtList);
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                var data = context.tbl_Events.OrderByDescending(s => s.EventDate).Where(a => a.NewEventsApprvFrmSuper != false && a.DelEventsApprvFrmSuper != true).ToList();

                foreach (var i in data)
                {
                    evtList.Add(new Event()
                    {
                        EventName = i.EventName,
                        EventDate = i.EventDate,
                        EventTime = i.EventTime,
                        Description = i.Description,
                        OrganisedBy = i.OrganisedBy,
                        Place = i.Place,
                        ID = i.ID,
                        ImagePath = i.ImageUrl,
                        AddedBy = i.AddedBy
                    });
                }
                return View(evtList);
            }
           
        }

        [Authorize(Roles = "1")]
        public ActionResult EventsForApproval()
        {
            var data = context.tbl_Events.OrderByDescending(s => s.ID).Where(a => a.NewEventsApprvFrmSuper == false).ToList();
            List<Event> evtList = new List<Event>();
            foreach (var i in data)
            {
                evtList.Add(new Event()
                {
                    EventName = i.EventName,
                    EventDate = i.EventDate,
                    EventTime = i.EventTime,
                    Description = i.Description,
                    OrganisedBy = i.OrganisedBy,
                    Place = i.Place,
                    ID = i.ID,
                    ImagePath = i.ImageUrl,
                    AddedBy = i.AddedBy
                });
            }
            return View(evtList);
        }

        [HttpPost]
        public JsonResult ApproveEvent(int id)
        {
            var data = context.tbl_Events.SingleOrDefault(s => s.ID == id);
            if (data != null)
            {
                data.NewEventsApprvFrmSuper = true;
                context.Entry(data).State = EntityState.Modified;
                context.SaveChanges();
            }
            return Json("Approved");
        }

        [Authorize(Roles = "1")]
        public ActionResult DeleteEventBySuperAdminApproval()
        {
            var data = context.tbl_Events.OrderByDescending(s => s.ID).Where(a => a.DelEventsApprvFrmSuper == true).ToList();
            List<Event> evtList = new List<Event>();
            foreach (var i in data)
            {
                evtList.Add(new Event()
                {
                    EventName = i.EventName,
                    EventDate = i.EventDate,
                    EventTime = i.EventTime,
                    Description = i.Description,
                    OrganisedBy = i.OrganisedBy,
                    Place = i.Place,
                    ID = i.ID,
                    ImagePath = i.ImageUrl,
                    AddedBy = i.AddedBy,
                    DeletedBy=i.DeletedBy
                });
            }
            return View(evtList);
        }

        [HttpPost]
        public JsonResult CancelDeleteEventById(int id)
        {
            var findUser = context.tbl_Events.SingleOrDefault(u => u.ID == id);
            if (findUser != null)
            {
                findUser.DelEventsApprvFrmSuper = false;
                context.Entry(findUser).State = EntityState.Modified;
                context.SaveChanges();
            }
            return Json("NotDeleted.");
        }

        [Authorize(Roles = "1,2")]
        public ActionResult AddNewEvent(Event evt)
        {
            if (Request.Cookies["PageSetting"] != null)
            {
                if (Request.Cookies["PageSetting"]["EventsPermission"] == "All")
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
        public JsonResult CreateNewEvent(Event evt)
        {
            int currentUserTypeLoggedin = Convert.ToInt32(Request.Cookies["UserType"].Value);
            tbl_Events e = new tbl_Events();
            e.EventName = evt.EventName;
            e.Description = evt.Description;
            e.EventDate = evt.EventDate;
            e.EventTime = evt.EventTime;
            e.OrganisedBy = evt.OrganisedBy;
            e.Place = evt.Place;
            e.AddedBy= Request.Cookies["AddedBy"].Value;
            foreach (string file in Request.Files)
            {
                var pic = System.Web.HttpContext.Current.Request.Files[file];
                var fileName = Path.GetFileName(pic.FileName);
                var _ext = Path.GetExtension(pic.FileName);
                string _name = Guid.NewGuid().ToString() + fileName;
                var comPath = Server.MapPath("/Upload/") + _name;
                var path = "/Upload/" + _name;
                e.ImageUrl = path;
                pic.SaveAs(comPath);
            }
            if (currentUserTypeLoggedin != 1)
            {
                e.NewEventsApprvFrmSuper = false;
            }
            else
            {
                e.NewEventsApprvFrmSuper = true;
            }
            e.DelEventsApprvFrmSuper = false;
            context.tbl_Events.Add(e);
            context.SaveChanges();
            return Json(currentUserTypeLoggedin);
        }

        [HttpPost]
        public JsonResult DeleteEventByID(int id)
        {
            int currentUserTypeLoggedin = Convert.ToInt32(Request.Cookies["UserType"].Value);
            if (currentUserTypeLoggedin == 1)
            {
                var data = context.tbl_Events.Find(id);
                var evtImagesdata = context.tbl_EventsImgVideo.Where(a => a.EventID == id).ToList();
                if (data != null)
                {
                    foreach (var i in evtImagesdata)
                    {
                        context.Entry(i).State = EntityState.Deleted;
                    }
                    context.Entry(data).State = EntityState.Deleted;
                    context.SaveChanges();
                }
            }
            else
            {
                var data = context.tbl_Events.Find(id);
                if (data != null)
                {
                    data.DelEventsApprvFrmSuper = true;
                    data.DeletedBy= Request.Cookies["AddedBy"].Value;
                    context.Entry(data).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            return Json(currentUserTypeLoggedin);
        }

        [Authorize(Roles = "1,2")]
        public ActionResult EventDetailsByID(int id,string editDetail)
        {
           
            if (Request.Cookies["PageSetting"] != null)
            {

                var data = context.tbl_Events.SingleOrDefault(a => a.ID == id);
                Event evt = new Event();
                evt.ID = data.ID;
                evt.EventName = data.EventName;
                evt.EventDate = data.EventDate;
                evt.EventTime = data.EventTime;
                evt.Place = data.Place;
                evt.AddedBy = data.AddedBy;
                evt.Description = data.Description;
                evt.OrganisedBy = data.OrganisedBy;
                var imgGallery = context.tbl_EventsImgVideo.Where(a => a.EventID == id).ToList();
                List<EventsImgVideo> galleryList = new List<EventsImgVideo>();

                foreach (var gallery in imgGallery)
                {
                    galleryList.Add(new EventsImgVideo()
                    {
                        ID = gallery.ID,
                        Img_Video_Path = gallery.Img_Video_Path
                    });
                }
                ViewBag.ImageGallery = galleryList;
                if (Request.Cookies["PageSetting"]["EventsPermission"] == "All")
                {
                    if (editDetail == "Details")
                    {
                        return View("~/Views/Events/EventDetailByID.cshtml", evt);
                    }
                    else
                    {
                        return View("~/Views/Events/EditEventByID.cshtml", evt);
                    }
                }
                else if (Request.Cookies["PageSetting"]["EventsPermission"] == "View")
                {
                    if (editDetail == "Details")
                    {
                        return View("~/Views/Events/EventDetailByID.cshtml", evt);
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
                var data = context.tbl_Events.SingleOrDefault(a => a.ID == id);
                Event evt = new Event();
                evt.ID = data.ID;
                evt.EventName = data.EventName;
                evt.EventDate = data.EventDate;
                evt.EventTime = data.EventTime;
                evt.Place = data.Place;
                evt.AddedBy = data.AddedBy;
                evt.Description = data.Description;
                evt.OrganisedBy = data.OrganisedBy;
                var imgGallery = context.tbl_EventsImgVideo.Where(a => a.EventID == id).ToList();
                List<EventsImgVideo> galleryList = new List<EventsImgVideo>();

                foreach (var gallery in imgGallery)
                {
                    galleryList.Add(new EventsImgVideo()
                    {
                        ID = gallery.ID,
                        Img_Video_Path = gallery.Img_Video_Path
                    });
                }
                ViewBag.ImageGallery = galleryList;


                if (editDetail == "Details")
                {
                    return View("~/Views/Events/EventDetailByID.cshtml", evt);
                }
                else
                {
                    return View("~/Views/Events/EditEventByID.cshtml", evt);
                }
            }
            
        }

        [Authorize(Roles = "1,2")]
        public ActionResult NewDelEventDetailsByID(int id, string editDetail)
        {
            var data = context.tbl_Events.SingleOrDefault(a => a.ID == id);
            Event evt = new Event();
            evt.ID = data.ID;
            evt.EventName = data.EventName;
            evt.EventDate = data.EventDate;
            evt.EventTime = data.EventTime;
            evt.Place = data.Place;
            evt.AddedBy = data.AddedBy;
            evt.Description = data.Description;
            evt.OrganisedBy = data.OrganisedBy;
            var imgGallery = context.tbl_EventsImgVideo.Where(a => a.EventID == id).ToList();
            List<EventsImgVideo> galleryList = new List<EventsImgVideo>();

            foreach (var gallery in imgGallery)
            {
                galleryList.Add(new EventsImgVideo()
                {
                    ID = gallery.ID,
                    Img_Video_Path = gallery.Img_Video_Path
                });
            }
            ViewBag.ImageGallery = galleryList;


            if (editDetail == "New")
            {
                ViewBag.NewDelEvents = true;
            }
            else
            {
                ViewBag.NewDelEvents = false;
            }
            return View(evt);
        }

        [HttpPost]
        public JsonResult UpdateEventByID(Event evt)
        {
            var data = context.tbl_Events.Find(evt.ID);
            data.EventName = evt.EventName;
            data.EventDate = evt.EventDate;
            data.EventTime = evt.EventTime;
            data.Place = evt.Place;
            data.OrganisedBy = evt.OrganisedBy;
            data.Description = evt.Description;
            context.Entry(data).State = EntityState.Modified;
            context.SaveChanges();
            return Json("Updated.");
        }

        [Authorize(Roles = "1,2")]
        public ActionResult AddImagesToPastEvents(int id,string eventName)
        {
            ViewBag.EventID = id;
            ViewBag.EventName = eventName;
            return View();
        }

        [HttpPost]
        public JsonResult AddImagesByEvent(EventsImgVideo e)
        {
            if (Request.Files.Count > 0)
            {
                tbl_EventsImgVideo tbl_attachment = new tbl_EventsImgVideo();
                foreach (string file in Request.Files)
                {
                    var pic = System.Web.HttpContext.Current.Request.Files[file];
                    var fileName = Path.GetFileName(pic.FileName);
                    var _ext = Path.GetExtension(pic.FileName);
                    string _name = Guid.NewGuid().ToString() + fileName;
                    var comPath = Server.MapPath("/Upload/") + _name;
                    var path = "/Upload/" + _name;
                    tbl_attachment.EventID = e.EventID;
                    tbl_attachment.Img_Video_Path = path;
                    pic.SaveAs(comPath);
                    context.tbl_EventsImgVideo.Add(tbl_attachment);
                    context.SaveChanges();
                }
            }
            return Json("Added.");
        }

        [HttpPost]
        public JsonResult DeleteImageByID(int imgID)
        {
            var data = context.tbl_EventsImgVideo.Find(imgID);
            if (data != null)
            {
                context.Entry(data).State = EntityState.Deleted;
                context.SaveChanges();
            }
            return Json("ImageDeleted");
        }
    }
}