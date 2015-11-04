using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public static class TicketHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        
        public static void CreateHistory(this Tickets newTicket, string userid)
        {
            var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.ID == newTicket.ID);
            var changed = System.DateTimeOffset.Now;
            var assignedEmail = oldTicket.AssignedToID == null ? "" : db.Users.Find(oldTicket.AssignedToID).Email;
            if(oldTicket.Title != newTicket.Title)
            {
                TicketHistories th1 = new TicketHistories();
                th1.TicketID = newTicket.ID;
                th1.Property = "Title";
                th1.OldValue = oldTicket.Title;
                th1.NewValue = newTicket.Title;
                th1.Changed = changed;
                th1.ChangerID = userid;
                db.Histories.Add(th1);
                if(!(oldTicket.AssignedToID == null))
                { 
                    new EmailService().SendAsync(new IdentityMessage()
                    {
                        Destination = assignedEmail,
                        Subject = "Ticket Title Change Notification for " + oldTicket.Title,
                        Body = "Old Title: " + oldTicket.Title + "\nNew Title: " + newTicket.Title
                    });
                TicketNotifications notification1 = new TicketNotifications();
                notification1.NotifiedUserID = oldTicket.AssignedToID;
                notification1.TicketID = th1.TicketID;
                notification1.Property = "Title";
                db.Notifications.Add(notification1);
                }
            }

            if(oldTicket.Description != newTicket.Description)
            {
                TicketHistories th2 = new TicketHistories();
                th2.TicketID = newTicket.ID;
                th2.Property = "Description";
                th2.OldValue = oldTicket.Description;
                th2.NewValue = newTicket.Description;
                th2.Changed = changed;
                th2.ChangerID = userid;
                db.Histories.Add(th2);
                if (!(oldTicket.AssignedToID == null))
                {
                    new EmailService().SendAsync(new IdentityMessage()
                    {
                        Destination = assignedEmail,
                        Subject = "Ticket Description Change Notification for" + newTicket.Title,
                        Body = "Old Description: " + oldTicket.Description + "\nNew Description: " + newTicket.Description
                    });
                    TicketNotifications notification2 = new TicketNotifications();
                    notification2.NotifiedUserID = oldTicket.AssignedToID;
                    notification2.TicketID = th2.TicketID;
                    notification2.Property = "Description";
                    db.Notifications.Add(notification2);
                }
            }
            if(oldTicket.MediaURL != newTicket.MediaURL)
            {
                TicketHistories th3 = new TicketHistories();
                th3.TicketID = newTicket.ID;
                th3.Property = "Attachment";
                th3.OldValue = oldTicket.MediaURL;
                th3.NewValue = newTicket.MediaURL;
                th3.Changed = changed;
                th3.ChangerID = userid;
                db.Histories.Add(th3);
                if (!(oldTicket.AssignedToID == null))
                {
                    new EmailService().SendAsync(new IdentityMessage()
                    {
                        Destination = assignedEmail,
                        Subject = "Ticket Attachment Change Notification for " + newTicket.Title,
                        Body = "Please see new attachment on " + newTicket.Title
                    });
                    TicketNotifications notification3 = new TicketNotifications();
                    notification3.NotifiedUserID = oldTicket.AssignedToID;
                    notification3.TicketID = th3.TicketID;
                    notification3.Property = "Attachment";
                    db.Notifications.Add(notification3);
                }
            }
            if(oldTicket.TicketPrioritiesID != newTicket.TicketPrioritiesID)
            {
                TicketHistories th4 = new TicketHistories();
                th4.TicketID = newTicket.ID;
                th4.Property = "Priority";
                th4.OldValue = oldTicket.Priority.Name;
                th4.NewValue = db.Priorities.Find(newTicket.TicketPrioritiesID).Name;
                th4.Changed = changed;
                th4.ChangerID = userid;
                db.Histories.Add(th4);
                if (!(oldTicket.AssignedToID == null))
                {
                    new EmailService().SendAsync(new IdentityMessage()
                    {
                        Destination = assignedEmail,
                        Subject = "Ticket Priority Change Notification for " + newTicket.Title,
                        Body = "Old Priority: " + oldTicket.Priority.Name + "\nNew Priority: " + th4.NewValue
                    });
                    TicketNotifications notification4 = new TicketNotifications();
                    notification4.NotifiedUserID = oldTicket.AssignedToID;
                    notification4.TicketID = th4.TicketID;
                    notification4.Property = "Priority";
                    db.Notifications.Add(notification4);
                }
            }

            if(oldTicket.AssignedToID != newTicket.AssignedToID)
            {
                TicketHistories th5 = new TicketHistories();
                th5.TicketID = newTicket.ID;
                th5.Property = "Assigned To";
                th5.OldValue = oldTicket.AssignedToID == null ? "" : db.Users.Find(oldTicket.AssignedToID).DisplayName;
                th5.NewValue = db.Users.Find(newTicket.AssignedToID).DisplayName;
                th5.Changed = changed;
                th5.ChangerID = userid;
                db.Histories.Add(th5);
                if (!(oldTicket.AssignedToID == null))
                {
                    new EmailService().SendAsync(new IdentityMessage()
                    {
                        Destination = assignedEmail,
                        Subject = "Ticket Assignment Change Notification for " + newTicket.Title,
                        Body = "Old Assignment: " + oldTicket.AssignedTo.DisplayName + "\nNew Assignment: " + th5.NewValue
                    });
                    TicketNotifications notification5 = new TicketNotifications();
                    notification5.NotifiedUserID = oldTicket.AssignedToID;
                    notification5.TicketID = th5.TicketID;
                    notification5.Property = "Assignment";
                    db.Notifications.Add(notification5);
                }
            }

            if(oldTicket.TicketStatusesID != newTicket.TicketStatusesID)
            {
                TicketHistories th6 = new TicketHistories();
                th6.TicketID = newTicket.ID;
                th6.Property = "Status";
                th6.OldValue = oldTicket.Status.Name;
                th6.NewValue = db.Statuses.Find(newTicket.TicketStatusesID).Name;
                th6.Changed = changed;
                th6.ChangerID = userid;
                db.Histories.Add(th6);
                if (!(oldTicket.AssignedToID == null))
                {
                    new EmailService().SendAsync(new IdentityMessage()
                    {
                        Destination = assignedEmail,
                        Subject = "Ticket Status Change Notification for " + newTicket.Title,
                        Body = "Old Status: " + oldTicket.Status.Name + "\nNew Priority: " + th6.NewValue
                    });
                    TicketNotifications notification6 = new TicketNotifications();
                    notification6.NotifiedUserID = oldTicket.AssignedToID;
                    notification6.TicketID = th6.TicketID;
                    notification6.Property = "Status";
                    db.Notifications.Add(notification6);
                }
            }
            db.SaveChanges();
        }
    }
}