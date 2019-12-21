using EventProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EventProject.Models
{
    public class MajorViewModel
    {
        public MajorViewModel(int majorId, int facultyId, string majorName, bool isInactive)
        {
            this.MajorID = majorId;
            this.FacultyID = facultyId;
            this.MajorName = majorName;
            this.IsInActive = isInactive;
            this.FacultyName = getFacultyName(facultyId);
        }

        public MajorViewModel()
        {

        }
        
        public void updateMajor(Major major)
        {
            major.id = this.MajorID;
            major.name = this.MajorName;
            major.facultyId = this.FacultyID;
            major.isInactive = this.IsInActive;
        }

        public string getFacultyName(int facId)
        {
            using (EventContext1 db = new EventContext1())
            {
                var name = (from x in db.Faculties
                            where x.id == facId
                            select x.name).FirstOrDefault();
                return name;
            }
        }

        [Required]
        public int MajorID { get; set; }
        [Required]
        public int FacultyID { get; set; }
        [Required]
        public string MajorName { get; set; }
        [Required]
        public bool IsInActive { get; set; }
        public string FacultyName { get; set; }
        public List<Faculty> FacultyList()
        {
            EventContext1 db = new EventContext1();
            return db.Faculties.ToList();
        }
    }
}