using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class HomeViewModel
    {
        public IEnumerable<Projects> Projects { get; set; }
        public IEnumerable<Tickets> Tickets { get; set; }
        public IEnumerable<Tickets> DevProjectTickets { get; set; }
    }
}