namespace CHHC.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MediaTypeMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MediaTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        DisplayName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TrainingMedias", "MediaType_Id", c => c.Int());
            AddForeignKey("dbo.TrainingMedias", "MediaType_Id", "dbo.MediaTypes", "Id");
            CreateIndex("dbo.TrainingMedias", "MediaType_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TrainingMedias", new[] { "MediaType_Id" });
            DropForeignKey("dbo.TrainingMedias", "MediaType_Id", "dbo.MediaTypes");
            DropColumn("dbo.TrainingMedias", "MediaType_Id");
            DropTable("dbo.MediaTypes");
        }
    }
}
