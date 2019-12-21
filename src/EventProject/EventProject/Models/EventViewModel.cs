using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventProject.Models
{
    public class EventViewModel
    {
        public EventViewModel (int eventId, string eventName,string eventAddress, string shortDesc, string eventDesc, 
            DateTime start, DateTime end, string status, bool? isInActive)
        {
            this.EventID = eventId;
            this.EventName = eventName;
            this.EventAddress = eventAddress;
            this.EventShortDescription = shortDesc;
            this.EventDescription = eventDesc;
            this.EventStartDate = start;
            this.EventEndDate = end;
            this.Status = status;
            this.IsInActive = isInActive;
        }

        public void updateEvent(Event eve)
        {
            eve.id=this.EventID;
            eve.name=this.EventName;
            eve.address = this.EventAddress;
            eve.shortDescription=this.EventShortDescription;
            eve.description=this.EventDescription;
            eve.startDate=this.EventStartDate;
            eve.endDate=this.EventEndDate;
            eve.status = this.Status;
            eve.isInactive=this.IsInActive;
        }

        public EventViewModel()
        {

        }

        public int EventID { get; set; }
        public string EventName { get; set; }
        public string EventShortDescription { get; set; }
        public string EventDescription { get; set; }
        public string EventAddress { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public string Status { get; set; }
        public bool? IsInActive { get; set; }
    }
}