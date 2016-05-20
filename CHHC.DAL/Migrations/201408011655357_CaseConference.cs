namespace CHHC.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CaseConference : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserSettingsPatients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateAssigned = c.DateTime(nullable: false),
                        DateUnassigned = c.DateTime(),
                        Patient_Id = c.Int(),
                        UserSettings_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Patients", t => t.Patient_Id)
                .ForeignKey("dbo.UserSettings", t => t.UserSettings_Id)
                .Index(t => t.Patient_Id)
                .Index(t => t.UserSettings_Id);
            
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InnerChhcId = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TemplateCaseConferences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TemplateCaseConferenceQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ordinal = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        TemplateCaseConference_Id = c.Int(),
                        TemplateCaseConferenceTextQuestion_Id = c.Int(),
                        TemplateCaseConferenceDateQuestion_Id = c.Int(),
                        TemplateCaseConferenceMultichoiceQuestion_Id = c.Int(),
                        TemplateCaseConferenceMultichoiceTextQuestion_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TemplateCaseConferences", t => t.TemplateCaseConference_Id)
                .ForeignKey("dbo.TemplateCaseConferenceTextQuestions", t => t.TemplateCaseConferenceTextQuestion_Id)
                .ForeignKey("dbo.TemplateCaseConferenceDateQuestions", t => t.TemplateCaseConferenceDateQuestion_Id)
                .ForeignKey("dbo.TemplateCaseConferenceMultichoiceQuestions", t => t.TemplateCaseConferenceMultichoiceQuestion_Id)
                .ForeignKey("dbo.TemplateCaseConferenceMultichoiceTextQuestions", t => t.TemplateCaseConferenceMultichoiceTextQuestion_Id)
                .Index(t => t.TemplateCaseConference_Id)
                .Index(t => t.TemplateCaseConferenceTextQuestion_Id)
                .Index(t => t.TemplateCaseConferenceDateQuestion_Id)
                .Index(t => t.TemplateCaseConferenceMultichoiceQuestion_Id)
                .Index(t => t.TemplateCaseConferenceMultichoiceTextQuestion_Id);
            
            CreateTable(
                "dbo.TemplateCaseConferenceTextQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TemplateCaseConferenceDateQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TemplateCaseConferenceMultichoiceQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TemplateCaseConferenceMultichoiceQuestionAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        TemplateCaseConferenceMultichoiceQuestion_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TemplateCaseConferenceMultichoiceQuestions", t => t.TemplateCaseConferenceMultichoiceQuestion_Id)
                .Index(t => t.TemplateCaseConferenceMultichoiceQuestion_Id);
            
            CreateTable(
                "dbo.TemplateCaseConferenceMultichoiceTextQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TemplateCaseConferenceMultichoiceTextQuestionAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        TemplateCaseConferenceMultichoiceQuestion_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TemplateCaseConferenceMultichoiceTextQuestions", t => t.TemplateCaseConferenceMultichoiceQuestion_Id)
                .Index(t => t.TemplateCaseConferenceMultichoiceQuestion_Id);
            
            CreateTable(
                "dbo.CaseConferences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        IsSubmitted = c.Boolean(nullable: false),
                        UserSettings_Id = c.Int(),
                        Patient_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserSettings", t => t.UserSettings_Id)
                .ForeignKey("dbo.Patients", t => t.Patient_Id)
                .Index(t => t.UserSettings_Id)
                .Index(t => t.Patient_Id);
            
            CreateTable(
                "dbo.CaseConferenceQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ordinal = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        CaseConference_Id = c.Int(),
                        CaseConferenceTextQuestion_Id = c.Int(),
                        CaseConferenceDateQuestion_Id = c.Int(),
                        CaseConferenceMultichoiceQuestion_Id = c.Int(),
                        CaseConferenceMultichoiceTextQuestion_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CaseConferences", t => t.CaseConference_Id)
                .ForeignKey("dbo.CaseConferenceTextQuestions", t => t.CaseConferenceTextQuestion_Id)
                .ForeignKey("dbo.CaseConferenceDateQuestions", t => t.CaseConferenceDateQuestion_Id)
                .ForeignKey("dbo.CaseConferenceMultichoiceQuestions", t => t.CaseConferenceMultichoiceQuestion_Id)
                .ForeignKey("dbo.CaseConferenceMultichoiceTextQuestions", t => t.CaseConferenceMultichoiceTextQuestion_Id)
                .Index(t => t.CaseConference_Id)
                .Index(t => t.CaseConferenceTextQuestion_Id)
                .Index(t => t.CaseConferenceDateQuestion_Id)
                .Index(t => t.CaseConferenceMultichoiceQuestion_Id)
                .Index(t => t.CaseConferenceMultichoiceTextQuestion_Id);
            
            CreateTable(
                "dbo.CaseConferenceTextQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Answer = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CaseConferenceDateQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Answer = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CaseConferenceMultichoiceQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CaseConferenceMultichoiceQuestionAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        IsChosen = c.Boolean(nullable: false),
                        CaseConferenceMultichoiceQuestion_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CaseConferenceMultichoiceQuestions", t => t.CaseConferenceMultichoiceQuestion_Id)
                .Index(t => t.CaseConferenceMultichoiceQuestion_Id);
            
            CreateTable(
                "dbo.CaseConferenceMultichoiceTextQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        TextAnswer = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CaseConferenceMultichoiceTextQuestionAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        IsChosen = c.Boolean(nullable: false),
                        CaseConferenceMultichoiceQuestion_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CaseConferenceMultichoiceTextQuestions", t => t.CaseConferenceMultichoiceQuestion_Id)
                .Index(t => t.CaseConferenceMultichoiceQuestion_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.CaseConferenceMultichoiceTextQuestionAnswers", new[] { "CaseConferenceMultichoiceQuestion_Id" });
            DropIndex("dbo.CaseConferenceMultichoiceQuestionAnswers", new[] { "CaseConferenceMultichoiceQuestion_Id" });
            DropIndex("dbo.CaseConferenceQuestions", new[] { "CaseConferenceMultichoiceTextQuestion_Id" });
            DropIndex("dbo.CaseConferenceQuestions", new[] { "CaseConferenceMultichoiceQuestion_Id" });
            DropIndex("dbo.CaseConferenceQuestions", new[] { "CaseConferenceDateQuestion_Id" });
            DropIndex("dbo.CaseConferenceQuestions", new[] { "CaseConferenceTextQuestion_Id" });
            DropIndex("dbo.CaseConferenceQuestions", new[] { "CaseConference_Id" });
            DropIndex("dbo.CaseConferences", new[] { "Patient_Id" });
            DropIndex("dbo.CaseConferences", new[] { "UserSettings_Id" });
            DropIndex("dbo.TemplateCaseConferenceMultichoiceTextQuestionAnswers", new[] { "TemplateCaseConferenceMultichoiceQuestion_Id" });
            DropIndex("dbo.TemplateCaseConferenceMultichoiceQuestionAnswers", new[] { "TemplateCaseConferenceMultichoiceQuestion_Id" });
            DropIndex("dbo.TemplateCaseConferenceQuestions", new[] { "TemplateCaseConferenceMultichoiceTextQuestion_Id" });
            DropIndex("dbo.TemplateCaseConferenceQuestions", new[] { "TemplateCaseConferenceMultichoiceQuestion_Id" });
            DropIndex("dbo.TemplateCaseConferenceQuestions", new[] { "TemplateCaseConferenceDateQuestion_Id" });
            DropIndex("dbo.TemplateCaseConferenceQuestions", new[] { "TemplateCaseConferenceTextQuestion_Id" });
            DropIndex("dbo.TemplateCaseConferenceQuestions", new[] { "TemplateCaseConference_Id" });
            DropIndex("dbo.UserSettingsPatients", new[] { "UserSettings_Id" });
            DropIndex("dbo.UserSettingsPatients", new[] { "Patient_Id" });
            DropForeignKey("dbo.CaseConferenceMultichoiceTextQuestionAnswers", "CaseConferenceMultichoiceQuestion_Id", "dbo.CaseConferenceMultichoiceTextQuestions");
            DropForeignKey("dbo.CaseConferenceMultichoiceQuestionAnswers", "CaseConferenceMultichoiceQuestion_Id", "dbo.CaseConferenceMultichoiceQuestions");
            DropForeignKey("dbo.CaseConferenceQuestions", "CaseConferenceMultichoiceTextQuestion_Id", "dbo.CaseConferenceMultichoiceTextQuestions");
            DropForeignKey("dbo.CaseConferenceQuestions", "CaseConferenceMultichoiceQuestion_Id", "dbo.CaseConferenceMultichoiceQuestions");
            DropForeignKey("dbo.CaseConferenceQuestions", "CaseConferenceDateQuestion_Id", "dbo.CaseConferenceDateQuestions");
            DropForeignKey("dbo.CaseConferenceQuestions", "CaseConferenceTextQuestion_Id", "dbo.CaseConferenceTextQuestions");
            DropForeignKey("dbo.CaseConferenceQuestions", "CaseConference_Id", "dbo.CaseConferences");
            DropForeignKey("dbo.CaseConferences", "Patient_Id", "dbo.Patients");
            DropForeignKey("dbo.CaseConferences", "UserSettings_Id", "dbo.UserSettings");
            DropForeignKey("dbo.TemplateCaseConferenceMultichoiceTextQuestionAnswers", "TemplateCaseConferenceMultichoiceQuestion_Id", "dbo.TemplateCaseConferenceMultichoiceTextQuestions");
            DropForeignKey("dbo.TemplateCaseConferenceMultichoiceQuestionAnswers", "TemplateCaseConferenceMultichoiceQuestion_Id", "dbo.TemplateCaseConferenceMultichoiceQuestions");
            DropForeignKey("dbo.TemplateCaseConferenceQuestions", "TemplateCaseConferenceMultichoiceTextQuestion_Id", "dbo.TemplateCaseConferenceMultichoiceTextQuestions");
            DropForeignKey("dbo.TemplateCaseConferenceQuestions", "TemplateCaseConferenceMultichoiceQuestion_Id", "dbo.TemplateCaseConferenceMultichoiceQuestions");
            DropForeignKey("dbo.TemplateCaseConferenceQuestions", "TemplateCaseConferenceDateQuestion_Id", "dbo.TemplateCaseConferenceDateQuestions");
            DropForeignKey("dbo.TemplateCaseConferenceQuestions", "TemplateCaseConferenceTextQuestion_Id", "dbo.TemplateCaseConferenceTextQuestions");
            DropForeignKey("dbo.TemplateCaseConferenceQuestions", "TemplateCaseConference_Id", "dbo.TemplateCaseConferences");
            DropForeignKey("dbo.UserSettingsPatients", "UserSettings_Id", "dbo.UserSettings");
            DropForeignKey("dbo.UserSettingsPatients", "Patient_Id", "dbo.Patients");
            DropTable("dbo.CaseConferenceMultichoiceTextQuestionAnswers");
            DropTable("dbo.CaseConferenceMultichoiceTextQuestions");
            DropTable("dbo.CaseConferenceMultichoiceQuestionAnswers");
            DropTable("dbo.CaseConferenceMultichoiceQuestions");
            DropTable("dbo.CaseConferenceDateQuestions");
            DropTable("dbo.CaseConferenceTextQuestions");
            DropTable("dbo.CaseConferenceQuestions");
            DropTable("dbo.CaseConferences");
            DropTable("dbo.TemplateCaseConferenceMultichoiceTextQuestionAnswers");
            DropTable("dbo.TemplateCaseConferenceMultichoiceTextQuestions");
            DropTable("dbo.TemplateCaseConferenceMultichoiceQuestionAnswers");
            DropTable("dbo.TemplateCaseConferenceMultichoiceQuestions");
            DropTable("dbo.TemplateCaseConferenceDateQuestions");
            DropTable("dbo.TemplateCaseConferenceTextQuestions");
            DropTable("dbo.TemplateCaseConferenceQuestions");
            DropTable("dbo.TemplateCaseConferences");
            DropTable("dbo.Patients");
            DropTable("dbo.UserSettingsPatients");
        }
    }
}
