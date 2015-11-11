using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class ProjectsDetailsModel
    {
        public Projects ThisProject { get; set; }
        public IEnumerable<Tickets> OpenTickets { get; set;}
    }
}