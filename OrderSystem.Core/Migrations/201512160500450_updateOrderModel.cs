namespace OrderSystem.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateOrderModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderItems", "CertificateDate", c => c.DateTime());
            AddColumn("dbo.OrderItems", "CertificateNum", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderItems", "CertificateNum");
            DropColumn("dbo.OrderItems", "CertificateDate");
        }
    }
}
