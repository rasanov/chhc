namespace CHHC.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Trainings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TrainingMedias",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        Training_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Trainings", t => t.Training_Id, cascadeDelete: true)
                .Index(t => t.Training_Id);
            
            CreateTable(
                "dbo.MultichoiceQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Question = c.String(),
                        Training_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Trainings", t => t.Training_Id)
                .Index(t => t.Training_Id);
            
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        IsCorrect = c.Boolean(nullable: false),
                        MultichoiceQuestion_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MultichoiceQuestions", t => t.MultichoiceQuestion_Id)
                .Index(t => t.MultichoiceQuestion_Id);
            
            CreateTable(
                "dbo.UserSettingsTrainings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsCompleted = c.Boolean(nullable: false),
                        Training_Id = c.Int(nullable: false),
                        UserSettings_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Trainings", t => t.Training_Id, cascadeDelete: true)
                .ForeignKey("dbo.UserSettings", t => t.UserSettings_Id, cascadeDelete: true)
                .Index(t => t.Training_Id)
                .Index(t => t.UserSettings_Id);
            
            CreateTable(
                "dbo.UserSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MembershipUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserSettingsTrainings", new[] { "UserSettings_Id" });
            DropIndex("dbo.UserSettingsTrainings", new[] { "Training_Id" });
            DropIndex("dbo.Answers", new[] { "MultichoiceQuestion_Id" });
            DropIndex("dbo.MultichoiceQuestions", new[] { "Training_Id" });
            DropIndex("dbo.TrainingMedias", new[] { "Training_Id" });
            DropForeignKey("dbo.UserSettingsTrainings", "UserSettings_Id", "dbo.UserSettings");
            DropForeignKey("dbo.UserSettingsTrainings", "Training_Id", "dbo.Trainings");
            DropForeignKey("dbo.Answers", "MultichoiceQuestion_Id", "dbo.MultichoiceQuestions");
            DropForeignKey("dbo.MultichoiceQuestions", "Training_Id", "dbo.Trainings");
            DropForeignKey("dbo.TrainingMedias", "Training_Id", "dbo.Trainings");
            DropTable("dbo.UserSettings");
            DropTable("dbo.UserSettingsTrainings");
            DropTable("dbo.Answers");
            DropTable("dbo.MultichoiceQuestions");
            DropTable("dbo.TrainingMedias");
            DropTable("dbo.Trainings");
        }
    }
}
