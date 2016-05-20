namespace CHHC.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CaseConferenceCascadeDelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CaseConferenceQuestions", "CaseConference_Id", "dbo.CaseConferences");
            DropForeignKey("dbo.CaseConferenceMultichoiceQuestionAnswers", "CaseConferenceMultichoiceQuestion_Id", "dbo.CaseConferenceMultichoiceQuestions");
            DropForeignKey("dbo.CaseConferenceMultichoiceTextQuestionAnswers", "CaseConferenceMultichoiceQuestion_Id", "dbo.CaseConferenceMultichoiceTextQuestions");
            DropIndex("dbo.CaseConferenceQuestions", new[] { "CaseConference_Id" });
            DropIndex("dbo.CaseConferenceMultichoiceQuestionAnswers", new[] { "CaseConferenceMultichoiceQuestion_Id" });
            DropIndex("dbo.CaseConferenceMultichoiceTextQuestionAnswers", new[] { "CaseConferenceMultichoiceQuestion_Id" });
            AlterColumn("dbo.CaseConferenceQuestions", "CaseConference_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.CaseConferenceMultichoiceQuestionAnswers", "CaseConferenceMultichoiceQuestion_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.CaseConferenceMultichoiceTextQuestionAnswers", "CaseConferenceMultichoiceQuestion_Id", c => c.Int(nullable: false));
            AddForeignKey("dbo.CaseConferenceQuestions", "CaseConference_Id", "dbo.CaseConferences", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CaseConferenceMultichoiceQuestionAnswers", "CaseConferenceMultichoiceQuestion_Id", "dbo.CaseConferenceMultichoiceQuestions", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CaseConferenceMultichoiceTextQuestionAnswers", "CaseConferenceMultichoiceQuestion_Id", "dbo.CaseConferenceMultichoiceTextQuestions", "Id", cascadeDelete: true);
            CreateIndex("dbo.CaseConferenceQuestions", "CaseConference_Id");
            CreateIndex("dbo.CaseConferenceMultichoiceQuestionAnswers", "CaseConferenceMultichoiceQuestion_Id");
            CreateIndex("dbo.CaseConferenceMultichoiceTextQuestionAnswers", "CaseConferenceMultichoiceQuestion_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CaseConferenceMultichoiceTextQuestionAnswers", new[] { "CaseConferenceMultichoiceQuestion_Id" });
            DropIndex("dbo.CaseConferenceMultichoiceQuestionAnswers", new[] { "CaseConferenceMultichoiceQuestion_Id" });
            DropIndex("dbo.CaseConferenceQuestions", new[] { "CaseConference_Id" });
            DropForeignKey("dbo.CaseConferenceMultichoiceTextQuestionAnswers", "CaseConferenceMultichoiceQuestion_Id", "dbo.CaseConferenceMultichoiceTextQuestions");
            DropForeignKey("dbo.CaseConferenceMultichoiceQuestionAnswers", "CaseConferenceMultichoiceQuestion_Id", "dbo.CaseConferenceMultichoiceQuestions");
            DropForeignKey("dbo.CaseConferenceQuestions", "CaseConference_Id", "dbo.CaseConferences");
            AlterColumn("dbo.CaseConferenceMultichoiceTextQuestionAnswers", "CaseConferenceMultichoiceQuestion_Id", c => c.Int());
            AlterColumn("dbo.CaseConferenceMultichoiceQuestionAnswers", "CaseConferenceMultichoiceQuestion_Id", c => c.Int());
            AlterColumn("dbo.CaseConferenceQuestions", "CaseConference_Id", c => c.Int());
            CreateIndex("dbo.CaseConferenceMultichoiceTextQuestionAnswers", "CaseConferenceMultichoiceQuestion_Id");
            CreateIndex("dbo.CaseConferenceMultichoiceQuestionAnswers", "CaseConferenceMultichoiceQuestion_Id");
            CreateIndex("dbo.CaseConferenceQuestions", "CaseConference_Id");
            AddForeignKey("dbo.CaseConferenceMultichoiceTextQuestionAnswers", "CaseConferenceMultichoiceQuestion_Id", "dbo.CaseConferenceMultichoiceTextQuestions", "Id");
            AddForeignKey("dbo.CaseConferenceMultichoiceQuestionAnswers", "CaseConferenceMultichoiceQuestion_Id", "dbo.CaseConferenceMultichoiceQuestions", "Id");
            AddForeignKey("dbo.CaseConferenceQuestions", "CaseConference_Id", "dbo.CaseConferences", "Id");
        }
    }
}
