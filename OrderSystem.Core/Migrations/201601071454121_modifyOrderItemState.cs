namespace OrderSystem.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifyOrderItemState : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderItems", "State", c => c.Int(nullable: false));
            AddColumn("dbo.OrderItems", "CustomerPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderItems", "CustomerPrice");
            DropColumn("dbo.OrderItems", "State");
        }
    }
}
