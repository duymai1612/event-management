using EventProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EventProject.Controllers
{
    public class EventController : Controller
    {
        // GET: Event
        EventContext1 db = new EventContext1();
        //List
        public ActionResult Index()
        {
            var eventLs = new List<EventViewModel>();
            db.Events.ToList().ForEach(eve =>
            {
                eventLs.Add(new EventViewModel(eve.id, eve.name,eve.address, eve.shortDescription, eve.description, eve.startDate,
                    eve.endDate, eve.status, eve.isInactive));
            });

            var model = from x in eventLs
                        orderby x.EventStartDate descending
                        select x;

            return View(model);
        }

        //create
        [HttpGet]
        public ActionResult CreateEvent()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEvent(Event eve)
        {
            if (ModelState.IsValid)
            {
                db.Events.Add(eve);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eve);
        }

        //edit
        [HttpGet]
        public ActionResult UpdateEvent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Event eve = db.Events.Find(id);

            if (eve == null)
            {
                return HttpNotFound();
            }

            return View(eve);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateEvent(Event Event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Event);
        }

        //Detail
        public ActionResult DetailsEvent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event eve = db.Events.Find(id);
            if (eve == null)
            {
                return HttpNotFound();
            }
            return View(eve);
        }

        //delete
        [HttpGet]
        public ActionResult DeleteEvent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event eve = db.Events.Find(id);
            if (eve == null)
            {
                return HttpNotFound();
            }
            return View(eve);
        }

        [HttpPost, ActionName("DeleteEvent")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletedEvent(int id)
        {
            Event eve = db.Events.Find(id);
            db.Events.Remove(eve);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}