using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketComments
    {
        public int ID { get; set; }
        [Required]
        public string Body { get; set; }
        [DisplayFormat(DataFormatString = "{0:MMM dd yyyy hh:mm tt}")]
        public DateTimeOffset Created { get; set; }
        public string MediaURL { get; set; }
        public int TicketID { get; set; }
        public string AuthorID { get; set; }

        public virtual Tickets Ticket { get; set; }
        public virtual ApplicationUser Author { get; set; }
    }
}