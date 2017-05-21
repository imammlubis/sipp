namespace Sipp.Core.Migrations
{
    using Data.Infrastructure;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Sipp.Data.Entity.CoreIdentity;
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<
        Sipp.Core.Data.Infrastructure.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Sipp.Core.Data.Infrastructure.ApplicationDbContext context)
        {
            //InitRoles();
            //InitUsers();
        }
        
        public void InitRoles()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            roleManager.Create(new ApplicationRole { Name = "administrator", CustomRoleName = "administrator" });
            roleManager.Create(new ApplicationRole { Name = "company", CustomRoleName = "company" });

        }
        private void InitUsers()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(
                new ApplicationDbContext()));

            var user = new ApplicationUser()
            {
                UserName = "admin@admin.com",
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin@admin.com",
                EmailConfirmed = true,
            };
            manager.Create(user, "pass123");
            var findUser = manager.FindByName(user.UserName);
            manager.AddToRoles(findUser.Id, new string[] { "administrator" });


            //var user1 = new ApplicationUser()
            //{
            //    UserName = "joko@gmail.com",
            //    FirstName = "Joko",
            //    LastName = "Digdoyo",
            //    Email = "joko@gmail.com",
            //    EmailConfirmed = true,
            //};
            //manager.Create(user1, "pass123");
            //var findUser1 = manager.FindByName(user1.UserName);
            //manager.AddToRoles(findUser1.Id, new string[] { "user" });

            //var user2 = new ApplicationUser()
            //{
            //    UserName = "gatot@gmail.com",
            //    FirstName = "Gatot",
            //    LastName = "Nursanto",
            //    Email = "gatot@gmail.com",
            //    EmailConfirmed = true,
            //};
            //manager.Create(user2, "pass123");
            //var findUser2 = manager.FindByName(user2.UserName);
            //manager.AddToRoles(findUser2.Id, new string[] { "user" });

            //var user3 = new ApplicationUser()
            //{
            //    UserName = "jusuf@gmail.com",
            //    FirstName = "Jusuf",
            //    LastName = "Kallo",
            //    Email = "jusuf@gmail.com",
            //    EmailConfirmed = true,
            //};
            //manager.Create(user3, "pass123");
            //var findUser3 = manager.FindByName(user3.UserName);
            //manager.AddToRoles(findUser3.Id, new string[] { "user" });

            //var user4 = new ApplicationUser()
            //{
            //    UserName = "luhur@gmail.com",
            //    FirstName = "Luhur",
            //    LastName = "Patrialis",
            //    Email = "luhur@gmail.com",
            //    EmailConfirmed = true,
            //};
            //manager.Create(user4, "pass123");

            //var findUser4 = manager.FindByName(user4.UserName);
            //manager.AddToRoles(findUser4.Id, new string[] { "user" });
            //var user4 = new ApplicationUser()
            //{
            //    UserName = "admin@litt.ly",
            //    FirstName = "admin",
            //    LastName = "littly",
            //    Email = "admin@litt.ly",
            //    EmailConfirmed = true,
            //};
            //manager.Create(user4, "pass123");
            //var findUser4 = manager.FindByName(user4.UserName);
            //manager.AddToRoles(findUser4.Id, new string[] { "littly" });


        }


    }
}
