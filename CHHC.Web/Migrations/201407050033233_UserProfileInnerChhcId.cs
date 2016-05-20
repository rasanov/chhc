namespace CHHC.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserProfileInnerChhcId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "InnerChhcId", c => c.Int());
        }

        public override void Down()
        {
            DropColumn("dbo.UserProfile", "InnerChhcId");
        }
    }
}
