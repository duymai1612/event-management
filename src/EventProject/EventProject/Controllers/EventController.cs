using EventProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventProject.Controllers
{
    public class EventController : Controller
    {
        // GET: Event
        EventContext db = new EventContext();
        public ActionResult Index()
        {
            var eventLs = new List<EventViewModel>();
            db.Events.ToList().ForEach(eve =>
            {
                eventLs.Add(new EventViewModel(eve.id, eve.name, eve.shortDescription, eve.description, eve.startDate,
                    eve.endDate, eve.status, eve.isInactive));
            });

            var model = from x in eventLs
                        orderby x.EventStartDate descending
                        select x;
            return View(model);
        }
    }
}