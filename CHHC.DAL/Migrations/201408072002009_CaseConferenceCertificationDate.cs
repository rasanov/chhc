namespace CHHC.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CaseConferenceCertificationDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CaseConferences", "CertificationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CaseConferences", "CertificationDate");
        }
    }
}
