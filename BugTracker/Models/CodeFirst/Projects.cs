using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class Projects
    {
        public Projects()
        {
            this.ProjectMembers = new HashSet<ApplicationUser>();
        }
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ApplicationUser> ProjectMembers { get; set; }
        public virtual ICollection<Tickets> Tickets { get; set; }
    }
}