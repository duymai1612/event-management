using EventProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventProject.Models
{
    public class FacultyViewModel
    {
        public FacultyViewModel(int facultyId, string facultyName, bool isInActive)
        {
            this.FacultyID = facultyId;
            this.FacultyName = facultyName;
            this.IsInActive = isInActive;
        }
        
        public FacultyViewModel()
        {

        }

        public void updateFaculty(Faculty fac)
        {
            fac.id = this.FacultyID;
            fac.name = this.FacultyName;
            fac.isInactive = this.IsInActive;
        }

        public int FacultyID { get; set; }
        public string FacultyName { get; set; }
        public bool IsInActive { get; set; }
    }
}