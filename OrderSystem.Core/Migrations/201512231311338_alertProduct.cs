namespace OrderSystem.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alertProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "SupplierId", c => c.Int(nullable: true));
            CreateIndex("dbo.Products", "SupplierId");
            AddForeignKey("dbo.Products", "SupplierId", "dbo.Suppliers", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "SupplierId", "dbo.Suppliers");
            DropIndex("dbo.Products", new[] { "SupplierId" });
            DropColumn("dbo.Products", "SupplierId");
            DropTable("dbo.Suppliers");
        }
    }
}
