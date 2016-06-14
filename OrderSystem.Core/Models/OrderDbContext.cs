using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

using OrderSystem.Core.Extend.Model;

namespace OrderSystem.Core.Models
{
    public class OrderDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<ComboProduct> ComboProducts { get; set; }

        public DbSet<ComboProductItem> ComboProductItems { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<PersonInfo> PersonInfo { get; set; }

        public DbSet<TaskOrderModel> Tasks { get; set; }

        public DbSet<AccessToken> AccessTokens { get; set; }

        public DbSet<MessageModel> Messages { get; set; }

        public OrderDbContext()
            : base("CoreConnection", throwIfV1Schema: false)
        {
        }

        public static OrderDbContext Create() {
            return new OrderDbContext();
        }

    }
}