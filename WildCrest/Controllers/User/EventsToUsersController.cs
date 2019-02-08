using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WildCrest.Models.WildCrestModels;

namespace WildCrest.Controllers.User
{
    public class EventsToUsersController : Controller
    {
        ClubWildCrestEntities context = new ClubWildCrestEntities();
        // GET: EventsToUsers
        [Authorize(Roles = "1,2,3")]
        public ActionResult Index()
        {
            var data = context.tbl_Events.OrderByDescending(s => s.EventDate).Where(a => a.NewEventsApprvFrmSuper != false && a.DelEventsApprvFrmSuper != true).ToList();
            List<Event> upcomingEvents = new List<Event>();
            List<Event> pastEvents = new List<Event>();    

            foreach (var i in data)
            {
                string[] arrDate = i.EventDate.Split('/');
                DateTime EventDate = new DateTime(Convert.ToInt32(arrDate[2]), Convert.ToInt32(arrDate[0]), Convert.ToInt32(arrDate[1]));
                if (EventDate >= DateTime.Today)
                {
                    upcomingEvents.Add(new Event()
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
                else
                {
                    pastEvents.Add(new Event()
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
            }
            ViewBag.UpcomingEvents = upcomingEvents;
            ViewBag.PastEvents = pastEvents;
            return View();
        }
        [Authorize(Roles = "1,2,3")]
        public ActionResult ViewEventDetailsWithImages(int id)
        {
            var data = context.tbl_Events.FirstOrDefault(s => s.ID == id);
            Event evtDetails = new Event();
            evtDetails.EventName = data.EventName;
            evtDetails.EventDate = data.EventDate;
            evtDetails.EventTime = data.EventTime;
            evtDetails.Description = data.Description;
            evtDetails.OrganisedBy = data.OrganisedBy;
            evtDetails.Place = data.Place;
            evtDetails.ID = data.ID;

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
            return View(evtDetails);
        }
    }
}