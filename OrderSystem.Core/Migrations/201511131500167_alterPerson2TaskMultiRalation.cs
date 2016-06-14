namespace OrderSystem.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterPerson2TaskMultiRalation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PersonInfoes", "TaskOrderModel_Id", "dbo.TaskOrderModels");
            DropIndex("dbo.PersonInfoes", new[] { "TaskOrderModel_Id" });
            CreateTable(
                "dbo.TaskOrderModelPersonInfoes",
                c => new
                    {
                        TaskOrderModel_Id = c.Int(nullable: false),
                        PersonInfo_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TaskOrderModel_Id, t.PersonInfo_Id })
                .ForeignKey("dbo.TaskOrderModels", t => t.TaskOrderModel_Id, cascadeDelete: true)
                .ForeignKey("dbo.PersonInfoes", t => t.PersonInfo_Id, cascadeDelete: true)
                .Index(t => t.TaskOrderModel_Id)
                .Index(t => t.PersonInfo_Id);
            
            DropColumn("dbo.PersonInfoes", "TaskOrderModel_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PersonInfoes", "TaskOrderModel_Id", c => c.Int());
            DropForeignKey("dbo.TaskOrderModelPersonInfoes", "PersonInfo_Id", "dbo.PersonInfoes");
            DropForeignKey("dbo.TaskOrderModelPersonInfoes", "TaskOrderModel_Id", "dbo.TaskOrderModels");
            DropIndex("dbo.TaskOrderModelPersonInfoes", new[] { "PersonInfo_Id" });
            DropIndex("dbo.TaskOrderModelPersonInfoes", new[] { "TaskOrderModel_Id" });
            DropTable("dbo.TaskOrderModelPersonInfoes");
            CreateIndex("dbo.PersonInfoes", "TaskOrderModel_Id");
            AddForeignKey("dbo.PersonInfoes", "TaskOrderModel_Id", "dbo.TaskOrderModels", "Id");
        }
    }
}
