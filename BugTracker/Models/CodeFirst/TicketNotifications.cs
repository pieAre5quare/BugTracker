using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketNotifications
    {
        public TicketNotifications()
        {
            Acknowledged = false;
        }
        public int ID { get; set; }
        public int TicketID { get; set; }
        public bool Acknowledged { get; set; }
        public string Property { get; set; }
        public string NotifiedUserID { get; set; }
        

        public virtual Tickets Ticket { get; set; }
        public virtual ApplicationUser NotifiedUser { get; set; }
    }
}