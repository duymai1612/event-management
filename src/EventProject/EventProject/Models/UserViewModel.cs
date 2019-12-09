using EventProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EventProject.Models
{
    public class UserViewModel
    {
        EventContext db = new EventContext();
        public UserViewModel(string id, string pwd, string fname, string lname, DateTime? dob, string gdr,
            string role, string url, bool isInActive, int? majorid)
        {
            this.UserID = id;
            this.Password = pwd;
            this.FirstName = fname;
            this.LastName = lname;
            this.DateOfBirth = dob;
            this.Gender = gdr;
            this.Role = role;
            this.ImageURL = url;
            this.IsInActive = isInActive;
            this.MajorID = majorid;
            this.MajorName = getMajorName(majorid);
        }

        public UserViewModel()
        {

        }

        public string getMajorName(int? majorid)
        {
            using (EventContext db = new EventContext())
            {
               var name  = (from x in db.Majors
                               where x.id == majorid
                               select x.name).FirstOrDefault();
                return name;
            };
        }

        public void updateUser(User usr)
        {
            usr.id = this.UserID;
            usr.password = this.Password;
            usr.firstName = this.FirstName;
            usr.lastName = this.LastName;
            usr.dob = this.DateOfBirth;
            usr.gender = this.Gender;
            usr.role = this.Role;
            usr.imageUrl=this.ImageURL;
            usr.isInactive=this.IsInActive;
            usr.majorId = this.MajorID;
        }

        [Required(ErrorMessage = "Required User ID")]
        public string UserID { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName{ get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Role { get; set; }
        public string ImageURL { get; set; }
        [Required]
        public bool IsInActive { get; set; }
        public int? MajorID { get; set; }
        public string MajorName { get; set; }
        public List<Major> MajorList()
        {
            return db.Majors.ToList();
        }
        public HttpPostedFileWrapper ImageFile { get; set; }
    }
}