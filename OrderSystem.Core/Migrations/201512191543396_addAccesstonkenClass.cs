namespace OrderSystem.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAccesstonkenClass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccessTokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccessTokenStr = c.String(),
                        ExpiresDate = c.Double(nullable: false),
                        RefreshToken = c.String(),
                        SessionKey = c.String(),
                        SessionSecret = c.String(),
                        GetDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AccessTokens");
        }
    }
}
