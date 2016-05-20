using System.Linq;
using System.Security.Principal;
using System.Web.Security;
using CHHC.Web.Models;

namespace CHHC.Web
{
    public static class AccountHelper
    {
        public static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        public static MembershipCreateStatus UserProfileValidate(string username, string email)
        {
            using (var context = new UsersContext())
            {
                var userProfiles = context.UserProfiles.Where(u => !u.IsDeleted);
                if (userProfiles.Any(u => u.UserName == username))
                {
                    return MembershipCreateStatus.DuplicateUserName;
                }
                if (userProfiles.Any(u => u.Email == email))
                {
                    return MembershipCreateStatus.DuplicateEmail;
                }
                return MembershipCreateStatus.Success;
            }
        }

        public static bool UserProfileEmailValidate(UserProfile userProfile)
        {
            using (var context = new UsersContext())
            {
                return !context.UserProfiles.Where(u => !u.IsDeleted).Any(u => u.Email == userProfile.Email && u.UserName != userProfile.UserName);
            }
        }

        public static bool IsInAnyRole(this IPrincipal user, params string[] roles)
        {
            foreach (var role in roles)
            {
                if (user.IsInRole(role))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CanSeeHomePage(this IPrincipal user)
        {
            if (CanViewNotEditCaseConference(user))
            {
                return true;
            }
            if (CanViewNotEditTraining(user))
            {
                return true;
            }
            if (CanViewTargetedDocuments(user))
            {
                return true;
            }
            return false;
        }

        public static bool CanAccessUsers(this IPrincipal user)
        {
            return user.IsInAnyRole(RoleNames.ControlPanelAdmin, RoleNames.CHHCAdmin);
        }

        public static bool CanAccessRoles(this IPrincipal user)
        {
            return user.IsInAnyRole(RoleNames.ControlPanelAdmin, RoleNames.CHHCAdmin);
        }

        public static bool CanAccessTrainings(this IPrincipal user)
        {
            return user.IsInAnyRole(RoleNames.CHHCAdmin, RoleNames.Training_Edit, RoleNames.Training_View);
        }

        public static bool CanManageTrainings(this IPrincipal user)
        {
            return user.IsInAnyRole(RoleNames.CHHCAdmin, RoleNames.Training_Edit);
        }

        public static bool CanViewNotEditTraining(this IPrincipal user)
        {
            if (user.IsInRole(RoleNames.Training_View) && !(user.IsInRole(RoleNames.Training_Edit) || user.IsInRole(RoleNames.CHHCAdmin)))
            {
                return true;
            }
            return false;
        }

        public static bool CanAccessPatients(this IPrincipal user)
        {
            return user.IsInAnyRole(RoleNames.CHHCAdmin, RoleNames.Patient_Edit);
        }

        public static bool CanManageCaseConferences(this IPrincipal user)
        {
            return user.IsInAnyRole(RoleNames.CHHCAdmin, RoleNames.CaseConference_Edit);
        }

        public static bool CanViewNotEditCaseConference(this IPrincipal user)
        {
            if (user.IsInRole(RoleNames.CaseConference_View) && !(user.IsInRole(RoleNames.CaseConference_Edit) || user.IsInRole(RoleNames.CHHCAdmin)))
            {
                return true;
            }
            return false;
        }

        public static bool CanViewAllDocuments(this IPrincipal user)
        {
            if (user.IsInRole(RoleNames.Document_View_All) || user.IsInRole(RoleNames.CHHCAdmin))
            {
                return true;
            }
            return false;
        }

        public static bool CanViewTargetedDocuments(this IPrincipal user)
        {
            if (user.IsInRole(RoleNames.Document_View) && !(user.IsInRole(RoleNames.Document_View_All) || user.IsInRole(RoleNames.CHHCAdmin)))
            {
                return true;
            }
            return false;
        }

        public static bool CanEditBlogs(this IPrincipal user)
        {
            if (user.IsInRole(RoleNames.CHHCAdmin))
            {
                return true;
            }
            return false;
        }

        public static bool CanViewJobs(this IPrincipal user)
        {
            if (user.IsInRole(RoleNames.CHHCAdmin))
            {
                return true;
            }
            return false;
        }

        public static void GetDefaultController(string userName, out string controllerName)
        {
            var roles = Roles.GetRolesForUser(userName);
            if (roles.Contains(RoleNames.ControlPanelAdmin))
            {
                controllerName = "ControlPanel";
                return;
            }
            if (roles.Contains(RoleNames.CHHCAdmin) || roles.Contains(RoleNames.Training_Edit))
            {
                controllerName = "Admin";
                return;
            }
            controllerName = "MyChhc";
        }

        public static void GetLandingControllerAction(string userName, out string controllerName, out string actionName)
        {
            var roles = Roles.GetRolesForUser(userName);
            if (roles.Contains(RoleNames.ControlPanelAdmin))
            {
                controllerName = "ControlPanel";
                actionName = "Users";
                return;
            }
            if (roles.Contains(RoleNames.CHHCAdmin))
            {
                controllerName = "Admin";
                actionName = "Users";
                return;
            }
            if (roles.Contains(RoleNames.Patient_Edit))
            {
                controllerName = "Patients";
                actionName = "All";
                return;
            }
            if (roles.Contains(RoleNames.CaseConference_Edit))
            {
                controllerName = "CaseConferences";
                actionName = "Compose";
                return;
            }
            if (roles.Contains(RoleNames.Training_Edit))
            {
                controllerName = "Admin";
                actionName = "Trainings";
                return;
            }
            controllerName = "MyChhc";
            actionName = "Index";
        }
    }
}