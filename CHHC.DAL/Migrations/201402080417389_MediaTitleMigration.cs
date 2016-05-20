namespace CHHC.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MediaTitleMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TrainingMedias", "Title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TrainingMedias", "Title");
        }
    }
}
