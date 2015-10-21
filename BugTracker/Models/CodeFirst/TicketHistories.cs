using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketHistories
    {
        public int ID { get; set; }
        public int TicketID { get; set; }
        public string Property { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTimeOffset Changed { get; set; }
        public int ChangerID { get; set; }

        public virtual Tickets Ticket { get; set; }
        public virtual ApplicationUser Changer { get; set; }
    }
}