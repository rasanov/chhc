namespace CHHC.Web.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Web.Security;
    using Models;
    using WebMatrix.WebData;

    internal sealed class Configuration : DbMigrationsConfiguration<UsersContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(UsersContext context)
        {
            if (!WebSecurity.Initialized)
                WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);

            if (!Roles.RoleExists(RoleNames.ControlPanelAdmin))
            {
                Roles.CreateRole(RoleNames.ControlPanelAdmin);
                if (!WebSecurity.UserExists("administrator"))
                {
                    WebSecurity.CreateUserAndAccount("administrator", "adm1n1strat0r");
                }
                Roles.AddUserToRole("administrator", RoleNames.ControlPanelAdmin);
            }

            if (!Roles.RoleExists(RoleNames.CHHCAdmin))
            {
                Roles.CreateRole(RoleNames.CHHCAdmin);
                if (!WebSecurity.UserExists("ygrupin"))
                {
                    WebSecurity.CreateUserAndAccount("ygrupin", "1234");
                }
                Roles.AddUserToRole("ygrupin", RoleNames.CHHCAdmin);
            }

            if (!Roles.RoleExists(RoleNames.Training_Edit))
            {
                Roles.CreateRole(RoleNames.Training_Edit);
            }

            if (!Roles.RoleExists(RoleNames.Training_View))
            {
                Roles.CreateRole(RoleNames.Training_View);
            }

            if (!Roles.RoleExists(RoleNames.Patient_Edit))
            {
                Roles.CreateRole(RoleNames.Patient_Edit);
            }
        }
    }
}
