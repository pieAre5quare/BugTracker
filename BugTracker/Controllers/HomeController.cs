using BugTracker.Helpers;
using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class HomeController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
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
        public ActionResult EditUser(string id = "a06aeeb6-bbaf-4d47-88cb-b49dfd1d9ef4")
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