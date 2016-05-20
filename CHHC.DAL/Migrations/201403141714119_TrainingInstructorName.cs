namespace CHHC.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrainingInstructorName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trainings", "InstructorName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Trainings", "InstructorName");
        }
    }
}
