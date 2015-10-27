using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using BugTracker.Helpers;

namespace BugTracker.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        

        // GET: Projects
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return View(db.Projects.ToList());
            } else if (User.IsInRole("Project Manager"))
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                return View(user.Project.ToList());
            }
            return View(db.Projects.ToList());

        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.Projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] Projects projects)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(projects);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(projects);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            var usersOnProject = project.ProjectMembers.Select(u=>u.Id);
            ProjectAndUsersModels p = new ProjectAndUsersModels();
            p.Project = project;
            if (User.IsInRole("Admin"))
            {
                p.Users = new MultiSelectList(db.Users, "Id", "DisplayName", usersOnProject);
            } else
            {
                var onlyDevs = "Developer".UsersInRole();
                p.Users = new MultiSelectList(onlyDevs, "Id", "DisplayName", usersOnProject);
            }

            return View(p);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProjectAndUsersModels projectAndUsers)
        {
            if (ModelState.IsValid)
            {
                var project = db.Projects.Find(projectAndUsers.Project.ID);
                project.ProjectMembers.Clear();

                project.ProjectMembers = db.Users.Where(u => projectAndUsers.SelectedUsers.Contains(u.Id)).ToList();

                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            return View(projectAndUsers);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.Projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Projects projects = db.Projects.Find(id);
            db.Projects.Remove(projects);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
