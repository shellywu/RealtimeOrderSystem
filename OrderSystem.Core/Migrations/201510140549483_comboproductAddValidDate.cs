namespace OrderSystem.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class comboproductAddValidDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ComboProducts", "ValidStartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.ComboProducts", "ValidEndDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ComboProducts", "ValidEndDate");
            DropColumn("dbo.ComboProducts", "ValidStartDate");
        }
    }
}
