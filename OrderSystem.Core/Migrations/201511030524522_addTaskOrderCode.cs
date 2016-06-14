namespace OrderSystem.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTaskOrderCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskOrderModels", "TaskCode", c => c.String(maxLength: 64));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaskOrderModels", "TaskCode");
        }
    }
}
