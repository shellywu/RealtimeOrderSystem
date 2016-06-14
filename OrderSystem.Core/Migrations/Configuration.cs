namespace OrderSystem.Core.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using OrderSystem.Core.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<OrderSystem.Core.Models.OrderDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(OrderSystem.Core.Models.OrderDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            var UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            string name = "admin@arcsin.cn";
            string roleName = "admin";
            string email = "admin@arcsin.cn";
            string passwd = "123!qwe";

            if (!RoleManager.RoleExists(roleName))
            {
                var roleResult = RoleManager.Create(new IdentityRole(roleName));
            }

            var superUser = new ApplicationUser { UserName = name, Email = email };
            Employee ee=new Employee();
            ee.CreateDate = DateTime.Now;
            ee.CreateBy = "admin";
            ee.FirstName = "π‹";
            ee.LastName = "¿Ì‘±";
            ee.ModifyDate = DateTime.Now;
            ee.Phone = "18612693628";
            superUser.EmployeeInfo = ee;

            var adminResult = UserManager.Create(superUser, passwd);

            if (adminResult.Succeeded) {
                UserManager.AddToRole(superUser.Id,roleName);
            }

            base.Seed(context);
        }
    }
}
