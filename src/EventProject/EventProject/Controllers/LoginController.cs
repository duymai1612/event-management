
using EventProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EventManagement.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        EventContext1 db = new EventContext1();
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["UserID"] != null) Session["UserID"] = null;
            if (Session["Role"] != null) Session["Role"] = null;
            return View();
        }
        [HttpPost]
        public ActionResult Index(string Username, string Password)
        {
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
            {
                string pwd = getMD5(Password);
                var userList = (from x in db.Users
                                where x.id == Username && x.password == pwd && x.isInactive==false
                                select x).FirstOrDefault();

                if (userList != null)
                {
                    Session["UserID"] = Username;
                    Session["Role"] = userList.role;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                }
            }
            return View();
        }
        [NonAction]
        public string getMD5(string password)
        {
            if (password != null)
            {
                string outMD5 = string.Empty;
                using (MD5 md5 = MD5.Create())
                {
                    byte[] md5_in = Encoding.UTF8.GetBytes(password);
                    byte[] md5_out = md5.ComputeHash(md5_in);
                    outMD5 = BitConverter.ToString(md5_out);
                    outMD5 = outMD5.Replace("-", "");
                }
                return outMD5;
            }
            return null;
        }
    }
}