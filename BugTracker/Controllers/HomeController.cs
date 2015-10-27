using BugTracker.Helpers;
using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    public class HomeController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var homeModel = new HomeViewModel();
            if (User.IsInRole("Admin"))
            {
                homeModel.Projects = db.Projects.ToList();
                homeModel.Tickets = db.Tickets.Where(t => t.TicketStatusesID == 1);
                return View(homeModel);
            }
            else if (User.IsInRole("Project Manager") || User.IsInRole("Developer"))
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                homeModel.Projects = user.Project.ToList();
                homeModel.Tickets = db.Tickets.Where(t => t.AssignedToID.Equals(user));
                return View();
            }
            return View(homeModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        //Get
        public ActionResult EditUser(string id)
        {
            
            var user = db.Users.Find(id);
            AdminUserViewModel AdminModel = new AdminUserViewModel();
            var selected = id.ListUserRoles();
            AdminModel.Roles = new MultiSelectList(db.Roles, "Name", "Name", selected);
            AdminModel.User = user;

            return View(AdminModel);
        }
        // Post
        [HttpPost]
        public ActionResult EditUser(string[] selectedRoles, AdminUserViewModel model)
        {
            var rolesBeforeAdd = model.User.Id.ListUserRoles();
            foreach(string role in rolesBeforeAdd)
            {
                if(!selectedRoles.Contains(role))
                {
                    model.User.Id.RemoveUserFromRole(role);
                }
            }
            foreach(string role in selectedRoles)
            {
                model.User.Id.AddUserToRole(role);
            }
            return RedirectToAction("Index");
        }
       
        //get
        public ActionResult SelectUser()
        {
            return View(db.Users.ToList());
        }

        
    }
}