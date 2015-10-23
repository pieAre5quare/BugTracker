using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;

namespace BugTracker.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tickets
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
            Tickets tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            return View(tickets);
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
        public ActionResult Create([Bind(Include = "ID,Title,Description,MediaURL,Created,Updated,ProjectID,TicketTypeID,TicketPrioritiesID,TicketStatusesID,OwnerID,AssignedToID")] Tickets ticket)
        {
            if (ModelState.IsValid)
            {
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
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
            ViewBag.TicketPrioritiesID = new SelectList(db.Priorities, "ID", "Name", tickets.TicketPrioritiesID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "Name", tickets.ProjectID);
            ViewBag.TicketStatusesID = new SelectList(db.Statuses, "ID", "Name", tickets.TicketStatusesID);
            ViewBag.TicketTypeID = new SelectList(db.Types, "ID", "Name", tickets.TicketTypeID);
            return View(tickets);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Description,MediaURL,Created,Updated,ProjectID,TicketTypeID,TicketPrioritiesID,TicketStatusesID,OwnerID,AssignedToID")] Tickets tickets)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tickets).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketPrioritiesID = new SelectList(db.Priorities, "ID", "Name", tickets.TicketPrioritiesID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "Name", tickets.ProjectID);
            ViewBag.TicketStatusesID = new SelectList(db.Statuses, "ID", "Name", tickets.TicketStatusesID);
            ViewBag.TicketTypeID = new SelectList(db.Types, "ID", "Name", tickets.TicketTypeID);
            return View(tickets);
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
    }
}
