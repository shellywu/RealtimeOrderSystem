namespace OrderSystem.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alertEmployeeGuid2String : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "EmployeeInfo_CreateBy");
            AddColumn("dbo.AspNetUsers", "EmployeeInfo_CreateBy", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "EmployeeInfo_CreateBy");
           // AlterColumn("dbo.AspNetUsers", "EmployeeInfo_CreateBy", c => c.Guid(nullable: false));
        }
    }
}
