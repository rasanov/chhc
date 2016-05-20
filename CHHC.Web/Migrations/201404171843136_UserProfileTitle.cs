namespace CHHC.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserProfileTitle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "Title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "Title");
        }
    }
}
