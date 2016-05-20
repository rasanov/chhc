using CHHC.DomainModel;
using System.Data.Entity.Migrations;

namespace CHHC.DAL.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Infrastructure.DomainContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Infrastructure.DomainContext context)
        {
            context.MediaTypes.AddOrUpdate(
                x => x.Key,
                new MediaType { Key = "youtube", DisplayName = "YouTube" },
                new MediaType { Key = "googledrive", DisplayName = "Google Drive" },
                new MediaType { Key = "audio", DisplayName = "Audio" }
            );
        }
    }
}