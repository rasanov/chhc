namespace CHHC.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobApplication : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobApplications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicantFirstName = c.String(),
                        ApplicantLastName = c.String(),
                        ApplicationDate = c.String(),
                        HowDidYouHear = c.Int(nullable: false),
                        HowDidYouHearOther = c.String(),
                        Position = c.String(),
                        PositionType = c.Int(nullable: false),
                        StreetAddress = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Zip = c.String(),
                        PhoneHome = c.String(),
                        PhoneCell = c.String(),
                        Email = c.String(),
                        SpecialEducation = c.String(),
                        ClinicalLicenseType = c.String(),
                        ClinicalLicenseNumber = c.String(),
                        ClinicalLicenseState = c.String(),
                        Authorized = c.Boolean(nullable: false),
                        Eligible = c.Boolean(nullable: false),
                        EmployedBefore = c.Boolean(nullable: false),
                        EmployedBeforePosition = c.String(),
                        WhenAvailableToBegin = c.String(),
                        EverDischarged = c.Boolean(nullable: false),
                        EverDischargedExplanation = c.String(),
                        EverHadStipulations = c.Boolean(nullable: false),
                        EverHadStipulationsExplanation = c.String(),
                        EverSanctioned = c.Boolean(nullable: false),
                        RelatedToBoard = c.Boolean(nullable: false),
                        MayWeContactPresentEmployer = c.Boolean(nullable: false),
                        MayWeContactPreviousEmployer = c.Boolean(nullable: false),
                        DateFilledAndAgreed = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.JobApplicationEducations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Name = c.String(),
                        From = c.DateTime(),
                        To = c.DateTime(),
                        MainCourse = c.String(),
                        Degree = c.String(),
                        JobApplication_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JobApplications", t => t.JobApplication_Id)
                .Index(t => t.JobApplication_Id);
            
            CreateTable(
                "dbo.JobApplicationWorks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JobTitle = c.String(),
                        From = c.DateTime(),
                        To = c.DateTime(),
                        Responsibilities = c.String(),
                        EmployerName = c.String(),
                        Address = c.String(),
                        Phone = c.String(),
                        Salary = c.String(),
                        SupervisorName = c.String(),
                        ReasonForLeaving = c.String(),
                        JobApplication_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JobApplications", t => t.JobApplication_Id)
                .Index(t => t.JobApplication_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.JobApplicationWorks", new[] { "JobApplication_Id" });
            DropIndex("dbo.JobApplicationEducations", new[] { "JobApplication_Id" });
            DropForeignKey("dbo.JobApplicationWorks", "JobApplication_Id", "dbo.JobApplications");
            DropForeignKey("dbo.JobApplicationEducations", "JobApplication_Id", "dbo.JobApplications");
            DropTable("dbo.JobApplicationWorks");
            DropTable("dbo.JobApplicationEducations");
            DropTable("dbo.JobApplications");
        }
    }
}
