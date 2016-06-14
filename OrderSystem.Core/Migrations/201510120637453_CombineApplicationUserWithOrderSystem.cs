namespace OrderSystem.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CombineApplicationUserWithOrderSystem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ComboProductItems",
                c => new
                    {
                        ComboProductId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        ProductPrice = c.Single(nullable: false),
                        ProductCommision = c.Single(nullable: false),
                        ProductSettlement = c.Single(nullable: false),
                        ProductContractPrice = c.Single(nullable: false),
                    })
                .PrimaryKey(t => new { t.ComboProductId, t.ProductId })
                .ForeignKey("dbo.ComboProducts", t => t.ComboProductId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ComboProductId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.ComboProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ComboCode = c.String(maxLength: 20),
                        ComboName = c.String(maxLength: 256),
                        Price = c.Single(nullable: false),
                        ProductPrice = c.Single(nullable: false),
                        ProductCommision = c.Single(nullable: false),
                        ProductSettlement = c.Single(nullable: false),
                        ProductContractPrice = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductName = c.String(maxLength: 128),
                        ProductType = c.Int(nullable: false),
                        City = c.String(maxLength: 256),
                        Spot = c.String(maxLength: 128),
                        ValidStartDate = c.DateTime(nullable: false),
                        ValidEndDate = c.DateTime(nullable: false),
                        ProductCode = c.String(nullable: false, maxLength: 20),
                        NeedFeedBack = c.Int(nullable: false),
                        Price = c.Single(nullable: false),
                        Commision = c.Single(nullable: false),
                        Settlement = c.Single(nullable: false),
                        ContractPrice = c.Single(nullable: false),
                        Cons = c.Single(nullable: false),
                        PartnerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Partners", t => t.PartnerId, cascadeDelete: true)
                .Index(t => t.PartnerId);
            
            CreateTable(
                "dbo.Partners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 20),
                        BeginDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        PartnerContract = c.String(maxLength: 20),
                        ParnerPhone = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CName = c.String(nullable: false, maxLength: 20),
                        CPhone = c.String(nullable: false, maxLength: 13),
                        CIdentity = c.String(maxLength: 18),
                        PassportNumber = c.String(maxLength: 30),
                        others = c.String(maxLength: 64),
                        Level = c.Int(nullable: false),
                        CEmail = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProductId = c.String(),
                        ProductType = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Quantity = c.Int(nullable: false),
                        TotalPrice = c.Single(nullable: false),
                        Remark = c.String(),
                        Order_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.Order_Id)
                .Index(t => t.Order_Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OrderCode = c.String(maxLength: 20),
                        OrderDate = c.DateTime(nullable: false),
                        ComplteDate = c.DateTime(nullable: false),
                        OrderStatus = c.Int(nullable: false),
                        Remark = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        CompleteUser_Id = c.String(maxLength: 128),
                        CreateUser_Id = c.String(maxLength: 128),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CompleteUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreateUser_Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.CompleteUser_Id)
                .Index(t => t.CreateUser_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.PersonInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PersonName = c.String(nullable: false, maxLength: 20),
                        PersonPhone = c.String(nullable: false, maxLength: 20),
                        PersonIdentity = c.String(nullable: false, maxLength: 30),
                        CredentialType = c.Int(nullable: false),
                        OrderItemId = c.Guid(nullable: false),
                        Remark = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrderItems", t => t.OrderItemId, cascadeDelete: true)
                .Index(t => t.OrderItemId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.PersonInfoes", "OrderItemId", "dbo.OrderItems");
            DropForeignKey("dbo.OrderItems", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.Orders", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Orders", "CreateUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Orders", "CompleteUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Orders", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Products", "PartnerId", "dbo.Partners");
            DropForeignKey("dbo.ComboProductItems", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ComboProductItems", "ComboProductId", "dbo.ComboProducts");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.PersonInfoes", new[] { "OrderItemId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Orders", new[] { "Customer_Id" });
            DropIndex("dbo.Orders", new[] { "CreateUser_Id" });
            DropIndex("dbo.Orders", new[] { "CompleteUser_Id" });
            DropIndex("dbo.Orders", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.OrderItems", new[] { "Order_Id" });
            DropIndex("dbo.Products", new[] { "PartnerId" });
            DropIndex("dbo.ComboProductItems", new[] { "ProductId" });
            DropIndex("dbo.ComboProductItems", new[] { "ComboProductId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.PersonInfoes");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderItems");
            DropTable("dbo.Customers");
            DropTable("dbo.Partners");
            DropTable("dbo.Products");
            DropTable("dbo.ComboProducts");
            DropTable("dbo.ComboProductItems");
        }
    }
}
