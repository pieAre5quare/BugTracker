﻿using BugTracker.Helpers;
using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Net;

namespace BugTracker.Controllers
{
    public class HomeController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        public ActionResult Index()
        {
            var homeModel = new HomeViewModel();
            homeModel.DevProjectTickets = new List<Tickets>();
            var userid = User.Identity.GetUserId();
            var user = db.Users.Find(userid);
            homeModel.Notifications = db.Notifications.Where(n => n.NotifiedUserID == userid && n.Acknowledged == false).ToList();
            

            if (User.IsInRole("Admin"))
            {
                homeModel.Projects = db.Projects.ToList();
                homeModel.Tickets = db.Tickets.Where(t => t.TicketStatusesID == 1).ToList();
            } else if (User.IsInRole("Project Manager"))
            {
                
                homeModel.Tickets = user.Project.SelectMany(p => p.Tickets).ToList();
                homeModel.Projects = user.Project.ToList();
                homeModel.DevProjectTickets = user.Project.SelectMany(p => p.Tickets).ToList();
            }
            else if (User.IsInRole("Developer"))
            {
                
                homeModel.Projects = user.Project.ToList();
                homeModel.Tickets = db.Tickets.Where(t => t.AssignedToID.Equals(user.Id)).ToList();
                homeModel.DevProjectTickets = user.Project.SelectMany(p => p.Tickets).ToList();
            } else if (User.IsInRole("Submitter"))
            {
                
                homeModel.Tickets = db.Tickets.Where(t => t.OwnerID.Equals(user.Id)).ToList();
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
        public ActionResult EditRoles(string id)
        {

            if (string.IsNullOrWhiteSpace(id))
                return new HttpStatusCodeResult(HttpStatusCode.OK);

            var user = db.Users.Find(id);
            AdminUserViewModel AdminModel = new AdminUserViewModel();
            var selected = id.ListUserRoles();
            AdminModel.Roles = new MultiSelectList(db.Roles, "Name", "Name", selected);
            AdminModel.User = user;

            return PartialView(AdminModel);
        }
        // Post
        [HttpPost]
        [Authorize]
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

        public ActionResult Acknowledge(int id)
        {
            var notification = db.Notifications.Find(id);
            notification.Acknowledged = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}