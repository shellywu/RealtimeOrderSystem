namespace OrderSystem.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class taskorderAddTaskDateTakePerson : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskOrderModels", "TakeDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.TaskOrderModels", "TakePerson", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaskOrderModels", "TakePerson");
            DropColumn("dbo.TaskOrderModels", "TakeDate");
        }
    }
}
