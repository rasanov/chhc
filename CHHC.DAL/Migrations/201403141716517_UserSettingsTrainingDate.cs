namespace CHHC.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserSettingsTrainingDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserSettingsTrainings", "CompletedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserSettingsTrainings", "CompletedDate");
        }
    }
}
