using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketComments
    {
        public int ID { get; set; }
        public string Body { get; set; }
        public DateTimeOffset Created { get; set; }
        public int TicketID { get; set; }
        public string AuthorID { get; set; }

        public virtual Tickets Ticket { get; set; }
        public virtual ApplicationUser Author { get; set; }
    }
}