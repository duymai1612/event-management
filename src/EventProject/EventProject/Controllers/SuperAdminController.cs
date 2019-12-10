using EventProject.App_Start;
using EventProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EventProject.Controllers
{
    [Authorization]
    [SuperAdminAuthorization]
    public class SuperAdminController : Controller
    {
        // GET: SuperAdmin
        EventContext db = new EventContext();
        #region Users         
        public ActionResult UsersList()
        {
            var usrLs = new List<UserViewModel>();
            db.Users.ToList().ForEach(usr =>
            {
                usrLs.Add(new UserViewModel(usr.id, usr.password, usr.firstName, usr.lastName, usr.dob,
                    usr.gender, usr.role, usr.imageUrl, usr.isInactive, usr.majorId));
            });
            return View(usrLs);
        }
        [HttpGet]
        public ActionResult CreateUser()
        {
            ViewBag.majorList = new SelectList(
                db.Majors.Select(x => new { Text = x.name, Value = x.id }), "Value", "Text");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser([Bind(Include = "UserID, Password, FirstName, LastName," +
            "DateOfBirth, Gender, Role, ImageFile, MajorID, IsInActive")] UserViewModel usr)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newUsr = new User();

                    if (usr.ImageFile != null)
                    {
                        string relativePath = "~/Images/" + DateTime.Now.Ticks.ToString() + "_" + usr.ImageFile.FileName;
                        string physicalPath = Server.MapPath(relativePath);
                        string imageFolder = Path.GetDirectoryName(physicalPath);
                        if (!Directory.Exists(imageFolder))
                        {
                            Directory.CreateDirectory(imageFolder);
                        }
                        usr.ImageFile.SaveAs(physicalPath);
                        usr.ImageURL = relativePath;
                    }

                    usr.updateUser(newUsr);
                    newUsr.password = getMD5(newUsr.password);
                    db.Users.Add(newUsr);
                    db.SaveChanges();
                    return RedirectToAction("UsersList");
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult UpdateUser(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            User usr = db.Users.Find(id);

            if (usr == null)
            {
                return HttpNotFound();
            }

            UserViewModel edituser = new UserViewModel(usr.id, usr.password, usr.firstName, usr.lastName,
                usr.dob, usr.gender, usr.role, usr.imageUrl, usr.isInactive, usr.majorId);

            ViewBag.majorList = new SelectList(
                db.Majors.Select(x => new { Text = x.name, Value = x.id }), "Value", "Text", usr.majorId);

            return View(edituser);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateUser([Bind(Include = "UserID, Password, FirstName, LastName," +
            "DateOfBirth, Gender, Role, ImageFile, MajorID, IsInActive")] UserViewModel usr)
        {
            if (!string.IsNullOrEmpty(usr.UserID) || !string.IsNullOrEmpty(usr.FirstName) ||
                !string.IsNullOrEmpty(usr.LastName))
            {
                User updateusr = new User();
                if (usr.ImageFile != null)
                {
                    string relativePath = "~/Images/" + DateTime.Now.Ticks.ToString() + "_" + usr.ImageFile.FileName;
                    string physicalPath = Server.MapPath(relativePath);
                    string imageFolder = Path.GetDirectoryName(physicalPath);
                    if (!Directory.Exists(imageFolder))
                    {
                        Directory.CreateDirectory(imageFolder);
                    }
                    usr.ImageFile.SaveAs(physicalPath);
                    usr.ImageURL = relativePath;
                }
                else
                {
                    var curUsr = (from x in db.Users
                                  where x.id == usr.UserID
                                  select x.imageUrl).FirstOrDefault();
                    usr.ImageURL = curUsr;
                }

                if (usr.Password == null)
                {
                    var curUsr = (from x in db.Users
                                  where x.id == usr.UserID
                                  select x.password).FirstOrDefault();
                    usr.Password = getMD5(curUsr);
                }

                usr.updateUser(updateusr);
                db.Entry(updateusr).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UsersList");
            }
            return View();
        }
        #endregion
        #region Majors
        public ActionResult MajorsList()
        {
            var mjLs = new List<MajorViewModel>();
            db.Majors.ToList().ForEach(major =>
            {
                mjLs.Add(new MajorViewModel(major.id, major.facultyId, major.name, major.isInactive));
            });
            return View(mjLs);
        }
        [HttpGet]
        public ActionResult CreateMajor()
        {
            ViewBag.facultyList = new SelectList(
                db.Faculties.Select(x => new { Text = x.name, Value = x.id }), "Value", "Text");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMajor(MajorViewModel major)
        {
            if (ModelState.IsValid)
            {
                var newmajor = new Major();
                major.updateMajor(newmajor);
                db.Majors.Add(newmajor);
                db.SaveChanges();
                return RedirectToAction("MajorsList");
            }
            return View();
        }
        [HttpGet]
        public ActionResult UpdateMajor(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Major mj = db.Majors.Find(id);

            if (mj == null)
            {
                return HttpNotFound();
            }

            MajorViewModel major = new MajorViewModel(mj.id, mj.facultyId, mj.name, mj.isInactive);

            ViewBag.facultyList = new SelectList(
                db.Faculties.Select(x => new { Text = x.name, Value = x.id }), "Value", "Text", mj.facultyId);
            return View(major);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateMajor(MajorViewModel major)
        {
            if (ModelState.IsValid)
            {
                var mjor = new Major();
                major.updateMajor(mjor);
                db.Entry(mjor).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MajorsList");
            }
            return View();
        }
        #endregion
        #region Faculties
        public ActionResult FacultiesList()
        {
            var facLs = new List<FacultyViewModel>();
            db.Faculties.ToList().ForEach(fac =>
            {
                facLs.Add(new FacultyViewModel(fac.id, fac.name, fac.isInactive));
            });
            return View(facLs);
        }
        [HttpGet]
        public ActionResult CreateFaculty()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFaculty(FacultyViewModel faculty)
        {
            if (ModelState.IsValid)
            {
                var newfaculty = new Faculty();
                faculty.updateFaculty(newfaculty);
                db.Faculties.Add(newfaculty);
                db.SaveChanges();
                return RedirectToAction("FacultiesList");
            }
            return View();
        }
        [HttpGet]
        public ActionResult UpdateFaculty(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Faculty fac = db.Faculties.Find(id);

            if (fac == null)
            {
                return HttpNotFound();
            }

            FacultyViewModel faculty = new FacultyViewModel(fac.id, fac.name, fac.isInactive);
            return View(faculty);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateFaculty(FacultyViewModel faculty)
        {
            if (ModelState.IsValid)
            {
                var fct = new Faculty();
                faculty.updateFaculty(fct);
                db.Entry(fct).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("FacultiesList");
            }
            return View();
        }
        #endregion
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