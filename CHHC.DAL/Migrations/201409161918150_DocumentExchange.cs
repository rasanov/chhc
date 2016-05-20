namespace CHHC.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentExchange : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserSettingsDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDownloaded = c.Boolean(nullable: false),
                        Document_Id = c.Int(nullable: false),
                        UserSettings_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Documents", t => t.Document_Id, cascadeDelete: true)
                .ForeignKey("dbo.UserSettings", t => t.UserSettings_Id, cascadeDelete: true)
                .Index(t => t.Document_Id)
                .Index(t => t.UserSettings_Id);
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        FilePath = c.String(),
                        ContentType = c.String(),
                        DateUploaded = c.DateTime(nullable: false),
                        Status = c.Byte(nullable: false),
                        UploadedBy_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserSettings", t => t.UploadedBy_Id)
                .Index(t => t.UploadedBy_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Documents", new[] { "UploadedBy_Id" });
            DropIndex("dbo.UserSettingsDocuments", new[] { "UserSettings_Id" });
            DropIndex("dbo.UserSettingsDocuments", new[] { "Document_Id" });
            DropForeignKey("dbo.Documents", "UploadedBy_Id", "dbo.UserSettings");
            DropForeignKey("dbo.UserSettingsDocuments", "UserSettings_Id", "dbo.UserSettings");
            DropForeignKey("dbo.UserSettingsDocuments", "Document_Id", "dbo.Documents");
            DropTable("dbo.Documents");
            DropTable("dbo.UserSettingsDocuments");
        }
    }
}
