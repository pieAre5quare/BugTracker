using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public static class ProjectHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static void AddUserToProject(this string userId, int projectId)
        {
            var project = db.Projects.Find(projectId);
            project.ProjectMembers.Add(db.Users.Find(userId));
            db.Entry(project).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        public static void RemoveUserFromProject(this string userId, int projectId)
        {
            if (userId.IsOnProject(projectId))
            {
                var project = db.Projects.Find(projectId);
                project.ProjectMembers.Remove(db.Users.Find(userId));
                db.Entry(project).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public static bool IsOnProject(this string userId, int projectId)
        {
            return db.Projects.Find(projectId).ProjectMembers.Contains(db.Users.Find(userId));
        }

        public static IList<ApplicationUser> ListUsersOnProject(this int projectId)
        {
            var project = db.Projects.Find(projectId);
            return project.ProjectMembers.ToList();
        }

        public static IList<Projects> ListProjectsForUser(this string userId)
        {
            return db.Users.Find(userId).Project.ToList();
        }

        public static IList<ApplicationUser> ListUsersNotOnProject(this int projectId)
        {
            return db.Users.Where(u => u.Project.All(p => p.ID != projectId)).ToList();
        }
    }
}