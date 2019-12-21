using EventProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EventProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        EventContext1 db = new EventContext1();

        [HttpGet]
        public ActionResult Profile()
        {
            int id = Convert.ToInt32((Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey);
            User urs = db.Users.Find(id);
            if (urs == null)
            {
                return HttpNotFound();
            }
            return View(urs);
        }

        [HttpPost]
        public ActionResult Profile(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }
    }
}