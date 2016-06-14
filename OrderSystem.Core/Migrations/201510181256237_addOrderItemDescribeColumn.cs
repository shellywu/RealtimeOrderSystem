namespace OrderSystem.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addOrderItemDescribeColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderItems", "Describe", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderItems", "Describe");
        }
    }
}
