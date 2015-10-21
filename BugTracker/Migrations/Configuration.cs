namespace BugTracker.Migrations
{
    using Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Configuration;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));


            //Admin account
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var AdminUser = ConfigurationManager.AppSettings["AdminUser"];
            var Passwd = ConfigurationManager.AppSettings["AdminPassword"];
            if (!context.Users.Any(u => u.Email == AdminUser))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = AdminUser,
                    Email = AdminUser,
                    FirstName = "Allan",
                    LastName = "Clark",
                    DisplayName = "aclark"
                }, Passwd);
            }

            var userId = userManager.FindByEmail(AdminUser).Id;
            userManager.AddToRole(userId, "Admin");


            // Project manager test account
            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }

            
            var PMUser = ConfigurationManager.AppSettings["PMUser"];
            Passwd = ConfigurationManager.AppSettings["PMPassword"];
            if (!context.Users.Any(u => u.Email == PMUser))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = PMUser,
                    Email = PMUser,
                    FirstName = "PMtest",
                    LastName = "PMtest",
                    DisplayName = "testPM"
                }, Passwd);
            }

            userId = userManager.FindByEmail(PMUser).Id;
            userManager.AddToRole(userId, "Project Manager");

            //Developer test account
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }


            var DevUser = ConfigurationManager.AppSettings["DevUser"];
            Passwd = ConfigurationManager.AppSettings["DevPassword"];
            if (!context.Users.Any(u => u.Email == DevUser))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = DevUser,
                    Email = DevUser,
                    FirstName = "Devtest",
                    LastName = "Devtest",
                    DisplayName = "testDev"
                }, Passwd);
            }

            userId = userManager.FindByEmail(DevUser).Id;
            userManager.AddToRole(userId, "Developer");


            //Submitter test account

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }


            var SubUser = ConfigurationManager.AppSettings["SubUser"];
            Passwd = ConfigurationManager.AppSettings["SubPassword"];
            if (!context.Users.Any(u => u.Email == SubUser))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = SubUser,
                    Email = SubUser,
                    FirstName = "Subtest",
                    LastName = "Subtest",
                    DisplayName = "testSub"
                }, Passwd);
            }

            userId = userManager.FindByEmail(SubUser).Id;
            userManager.AddToRole(userId, "Submitter");
        }
    }
}
