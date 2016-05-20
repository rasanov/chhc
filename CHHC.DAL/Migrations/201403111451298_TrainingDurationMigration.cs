namespace CHHC.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrainingDurationMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trainings", "DurationMinutes", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Trainings", "DurationMinutes");
        }
    }
}
