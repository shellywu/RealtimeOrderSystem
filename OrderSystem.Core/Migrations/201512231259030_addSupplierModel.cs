namespace OrderSystem.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSupplierModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SupplierName = c.String(),
                        Contract = c.String(),
                        Phone = c.String(),
                        Call = c.String(),
                        FixNumber = c.String(),
                        Remark = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
          
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
