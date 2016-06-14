namespace OrderSystem.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addProductBrokage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ComboProductItems", "Borkerage", c => c.Single(nullable: false));
            AddColumn("dbo.Products", "Borkerage", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Borkerage");
            DropColumn("dbo.ComboProductItems", "Borkerage");
        }
    }
}
