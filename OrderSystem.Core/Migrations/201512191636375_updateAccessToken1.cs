namespace OrderSystem.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAccessToken1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccessTokens", "Refresh_token", c => c.String());
            DropColumn("dbo.AccessTokens", "RefreshToken");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AccessTokens", "RefreshToken", c => c.String());
            DropColumn("dbo.AccessTokens", "Refresh_token");
        }
    }
}
