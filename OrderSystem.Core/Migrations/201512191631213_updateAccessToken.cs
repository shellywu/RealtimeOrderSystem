namespace OrderSystem.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAccessToken : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccessTokens", "Access_token", c => c.String());
            AddColumn("dbo.AccessTokens", "Expires_in", c => c.Double(nullable: false));
            AddColumn("dbo.AccessTokens", "Session_key", c => c.String());
            AddColumn("dbo.AccessTokens", "Session_secret", c => c.String());
            DropColumn("dbo.AccessTokens", "AccessTokenStr");
            DropColumn("dbo.AccessTokens", "ExpiresDate");
            DropColumn("dbo.AccessTokens", "SessionKey");
            DropColumn("dbo.AccessTokens", "SessionSecret");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AccessTokens", "SessionSecret", c => c.String());
            AddColumn("dbo.AccessTokens", "SessionKey", c => c.String());
            AddColumn("dbo.AccessTokens", "ExpiresDate", c => c.Double(nullable: false));
            AddColumn("dbo.AccessTokens", "AccessTokenStr", c => c.String());
            DropColumn("dbo.AccessTokens", "Session_secret");
            DropColumn("dbo.AccessTokens", "Session_key");
            DropColumn("dbo.AccessTokens", "Expires_in");
            DropColumn("dbo.AccessTokens", "Access_token");
        }
    }
}
