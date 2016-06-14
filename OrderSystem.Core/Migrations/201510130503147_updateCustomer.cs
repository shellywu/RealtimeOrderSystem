namespace OrderSystem.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateCustomer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "IdentityType", c => c.Int(nullable: false));
            AlterColumn("dbo.Customers", "CIdentity", c => c.String(maxLength: 30));
            DropColumn("dbo.Customers", "PassportNumber");
            DropColumn("dbo.Customers", "others");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "others", c => c.String(maxLength: 64));
            AddColumn("dbo.Customers", "PassportNumber", c => c.String(maxLength: 30));
            AlterColumn("dbo.Customers", "CIdentity", c => c.String(maxLength: 18));
            DropColumn("dbo.Customers", "IdentityType");
        }
    }
}
