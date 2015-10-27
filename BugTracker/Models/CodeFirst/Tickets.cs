using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class Tickets
    {
        public Tickets()
        {
            this.Comments = new HashSet<TicketComments>();
            this.History = new HashSet<TicketHistories>();
            this.Notifications = new HashSet<TicketNotifications>();
        }

        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string MediaURL { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public int ProjectID { get; set; }
        public int TicketTypeID { get; set; }
        public int TicketPrioritiesID { get; set; }
        public int TicketStatusesID { get; set; }
        public string OwnerID { get; set; }
        public string AssignedToID { get; set; }

        public virtual Projects Project { get; set; }
        public virtual TicketTypes TicketType { get; set; }
        public virtual TicketPriorities Priority { get; set; }
        public virtual TicketStatuses Status { get; set; }
        public virtual ApplicationUser Owner { get; set; }
        public virtual ApplicationUser AssignedTo { get; set; }


        public virtual ICollection<TicketComments> Comments { get; set; }
        public virtual ICollection<TicketHistories> History { get; set; }
        public virtual ICollection<TicketNotifications> Notifications { get; set; }

    }

}