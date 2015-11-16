using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using BugTracker.Helpers;
using Microsoft.AspNet.Identity;
using System.IO;

namespace BugTracker.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tickets
        [Authorize]
        public ActionResult Index()
        {
            var tickets = db.Tickets.Include(t => t.Priority).Include(t => t.Project).Include(t => t.Status).Include(t => t.TicketType);
            return View(tickets.ToList());
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = new DetailsViewModel();
            model.ThisTicket = db.Tickets.Find(id);
            var user = db.Users.Find(User.Identity.GetUserId());
            model.IsAssignedUser = model.ThisTicket.AssignedToID == user.Id;
            var userProjects = user.Project;
            model.IsUserOnTicketProject = user.Id.IsOnProject(model.ThisTicket.ProjectID);
            model.IsUserOwner = model.ThisTicket.OwnerID == user.Id;
            if (model.ThisTicket == null)
            {
                return HttpNotFound();
            }
            
            return View(model);
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            ViewBag.TicketPrioritiesID = new SelectList(db.Priorities, "ID", "Name");
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "Name");
            ViewBag.TicketStatusesID = new SelectList(db.Statuses, "ID", "Name");
            ViewBag.TicketTypeID = new SelectList(db.Types, "ID", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Description,MediaURL,Created,Updated,ProjectID,TicketTypeID,TicketPrioritiesID,TicketStatusesID,OwnerID,AssignedToID")] Tickets ticket, HttpPostedFileBase image)
        {
            if (image != null && image.ContentLength > 0)
            {
                var ext = Path.GetExtension(image.FileName).ToLower();
                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif" && ext != ".bmp")
                {
                    ModelState.AddModelError("image", "Invalid Format");
                }
            }
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var filePath = "/Uploads/";
                    var absPath = Server.MapPath("~" + filePath);
                    ticket.MediaURL = filePath + image.FileName;
                    Directory.CreateDirectory(absPath);
                    image.SaveAs(Path.Combine(absPath, image.FileName));
                }

                ticket.Created = System.DateTimeOffset.Now;
                ticket.OwnerID = User.Identity.GetUserId();
                ticket.TicketStatusesID = 1; //Set status to New
                ticket.Status = db.Statuses.Find(1);
                ticket.TicketPrioritiesID = 1002; // Set priority as "to be set"
                ticket.Priority = db.Priorities.Find(1002);

                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Details","Tickets", new { id = ticket.ID });
            }

            ViewBag.TicketPrioritiesID = new SelectList(db.Priorities, "ID", "Name", ticket.TicketPrioritiesID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "Name", ticket.ProjectID);
            ViewBag.TicketStatusesID = new SelectList(db.Statuses, "ID", "Name", ticket.TicketStatusesID);
            ViewBag.TicketTypeID = new SelectList(db.Types, "ID", "Name", ticket.TicketTypeID);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            var priorities = db.Priorities.Where(p => !(p.Name == "To Be Set"));
            var statuses = db.Statuses.Where(s => !(s.Name == "New"));
            ViewBag.TicketPrioritiesID = new SelectList(priorities, "ID", "Name", tickets.TicketPrioritiesID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "Name", tickets.ProjectID);
            ViewBag.TicketStatusesID = new SelectList(statuses, "ID", "Name", tickets.TicketStatusesID);
            ViewBag.TicketTypeID = new SelectList(db.Types, "ID", "Name", tickets.TicketTypeID);
            ViewBag.AssignedToID = new SelectList("Developer".UsersInRole(), "ID", "DisplayName", tickets.AssignedToID);
            return View(tickets);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "ID,Title,Description,MediaURL,Created,Updated,ProjectID,TicketTypeID,TicketPrioritiesID,TicketStatusesID,OwnerID,AssignedToID")] Tickets ticket, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null && image.ContentLength > 0)
                {
                    var ext = Path.GetExtension(image.FileName).ToLower();
                    if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif" && ext != ".bmp")
                    {
                        ModelState.AddModelError("image", "Invalid Format");
                    }
                }
                if (image != null)
                {
                    var filePath = "/Uploads/";
                    var absPath = Server.MapPath("~" + filePath);
                    ticket.MediaURL = filePath + image.FileName;
                    Directory.CreateDirectory(absPath);
                    image.SaveAs(Path.Combine(absPath, image.FileName));
                }
                ticket.CreateHistory(User.Identity.GetUserId());
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Tickets", new { id = ticket.ID });
            }
            ViewBag.TicketPrioritiesID = new SelectList(db.Priorities, "ID", "Name", ticket.TicketPrioritiesID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "Name", ticket.ProjectID);
            ViewBag.TicketStatusesID = new SelectList(db.Statuses, "ID", "Name", ticket.TicketStatusesID);
            ViewBag.TicketTypeID = new SelectList(db.Types, "ID", "Name", ticket.TicketTypeID);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            return View(tickets);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tickets tickets = db.Tickets.Find(id);
            db.Tickets.Remove(tickets);
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

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddComment(TicketComments newComment, HttpPostedFileBase image)
        {
            var ticket = db.Tickets.Find(newComment.TicketID);
            if (image != null && image.ContentLength > 0)
            {
                var ext = Path.GetExtension(image.FileName).ToLower();
                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif" && ext != ".bmp")
                {
                    ModelState.AddModelError("image", "Invalid Format");
                }
            }
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var filePath = "/Uploads/";
                    var absPath = Server.MapPath("~" + filePath);
                    newComment.MediaURL = filePath + image.FileName;
                    Directory.CreateDirectory(absPath);
                    image.SaveAs(Path.Combine(absPath, image.FileName));
                }
                new EmailService().SendAsync(new IdentityMessage()
                {
                    Destination = ticket.AssignedTo.Email,
                    Subject = "Comment Notification for " + ticket.Title,
                    Body = User.Identity.Name + " added: " + newComment.Body
                });
                TicketNotifications notification = new TicketNotifications();
                notification.NotifiedUserID = ticket.AssignedToID;
                notification.TicketID = ticket.ID;
                notification.Property = "Comment";
                db.Notifications.Add(notification);
                newComment.Created = System.DateTimeOffset.Now;
                newComment.AuthorID = User.Identity.GetUserId();
                db.Comments.Add(newComment);
                db.SaveChanges();
            }
            return RedirectToAction("Details", "Tickets", new { id = ticket.ID });
        }
    }
}
