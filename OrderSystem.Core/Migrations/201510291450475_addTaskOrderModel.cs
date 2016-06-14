namespace OrderSystem.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTaskOrderModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TaskOrderModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductName = c.String(maxLength: 128),
                        ProductCode = c.String(maxLength: 32),
                        ProductType = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        UsedStartDate = c.DateTime(nullable: false),
                        UsedEndDate = c.DateTime(nullable: false),
                        OrderItemId = c.Guid(nullable: false),
                        ProductFrom = c.String(maxLength: 10),
                        OrderCode = c.String(maxLength: 20),
                        Remark = c.String(maxLength: 256),
                        CreateDate = c.DateTime(nullable: false),
                        CompleteDate = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        TaskRemark = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.PersonInfoes", "TaskOrderModel_Id", c => c.Int());
            CreateIndex("dbo.PersonInfoes", "TaskOrderModel_Id");
            AddForeignKey("dbo.PersonInfoes", "TaskOrderModel_Id", "dbo.TaskOrderModels", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PersonInfoes", "TaskOrderModel_Id", "dbo.TaskOrderModels");
            DropIndex("dbo.PersonInfoes", new[] { "TaskOrderModel_Id" });
            DropColumn("dbo.PersonInfoes", "TaskOrderModel_Id");
            DropTable("dbo.TaskOrderModels");
        }
    }
}
