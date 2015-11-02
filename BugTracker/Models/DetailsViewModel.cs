using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class DetailsViewModel
    {
        public Tickets ThisTicket { get; set; }
        public bool IsAssignedUser { get; set; }
        public bool IsUserOnTicketProject { get; set; }
        public bool IsUserOwner { get; set; }
    }
}