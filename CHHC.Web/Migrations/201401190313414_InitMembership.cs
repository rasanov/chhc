namespace CHHC.Web.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitMembership : DbMigration
    {
        public override void Up()
        {
            MvcApplication.InitMembership();

            AddColumn("dbo.UserProfile", "FullName", x => x.String());
            AddColumn("dbo.UserProfile", "Email", x => x.String());
            AddColumn("dbo.UserProfile", "Phone", x => x.String());
            AddColumn("dbo.UserProfile", "Notes", x => x.String());
            AddColumn("dbo.UserProfile", "IsDeleted", x => x.Boolean(nullable: false, defaultValue: false));
        }

        public override void Down()
        {
            DropColumn("dbo.UserProfile", "IsDeleted");
            DropColumn("dbo.UserProfile", "Notes");
            DropColumn("dbo.UserProfile", "Phone");
            DropColumn("dbo.UserProfile", "Email");
            DropColumn("dbo.UserProfile", "FullName");
        }
    }
}
