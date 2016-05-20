namespace CHHC.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CaseConferenceStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CaseConferences", "Status", c => c.Int(nullable: false, defaultValue: (int)CHHC.DomainModel.CaseConferenceStatus.NotSubmitted));
            DropColumn("dbo.CaseConferences", "IsSubmitted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CaseConferences", "IsSubmitted", c => c.Boolean(nullable: false));
            DropColumn("dbo.CaseConferences", "Status");
        }
    }
}
