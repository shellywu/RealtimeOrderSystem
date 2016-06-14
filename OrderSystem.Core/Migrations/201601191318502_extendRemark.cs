namespace OrderSystem.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class extendRemark : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TaskOrderModels", "Remark", c => c.String(maxLength: 1024));
            AlterColumn("dbo.TaskOrderModels", "TaskRemark", c => c.String(maxLength: 1024));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TaskOrderModels", "TaskRemark", c => c.String(maxLength: 256));
            AlterColumn("dbo.TaskOrderModels", "Remark", c => c.String(maxLength: 256));
        }
    }
}
