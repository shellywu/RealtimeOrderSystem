namespace OrderSystem.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addOrderForeignKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "CompleteUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Orders", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.Orders", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Orders", new[] { "CompleteUser_Id" });
            DropIndex("dbo.Orders", new[] { "Customer_Id" });
            DropColumn("dbo.Orders", "CreateUser_Id");
            RenameColumn(table: "dbo.Orders", name: "Customer_Id", newName: "CustomerId");
            RenameColumn(table: "dbo.Orders", name: "ApplicationUser_Id", newName: "CreateUser_Id");
            AlterColumn("dbo.Orders", "CustomerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Orders", "CustomerId");
            AddForeignKey("dbo.Orders", "CustomerId", "dbo.Customers", "Id", cascadeDelete: true);
            DropColumn("dbo.Orders", "CompleteUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "CompleteUser_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            AlterColumn("dbo.Orders", "CustomerId", c => c.Int());
            RenameColumn(table: "dbo.Orders", name: "CreateUser_Id", newName: "ApplicationUser_Id");
            RenameColumn(table: "dbo.Orders", name: "CustomerId", newName: "Customer_Id");
            AddColumn("dbo.Orders", "CreateUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Orders", "Customer_Id");
            CreateIndex("dbo.Orders", "CompleteUser_Id");
            CreateIndex("dbo.Orders", "ApplicationUser_Id");
            AddForeignKey("dbo.Orders", "Customer_Id", "dbo.Customers", "Id");
            AddForeignKey("dbo.Orders", "CompleteUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
