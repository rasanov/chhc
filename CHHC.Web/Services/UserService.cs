using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using CHHC.Web.Models;

namespace CHHC.Web.Services
{
    public class UserService
    {
        public IEnumerable<UserProfile> ApplyControlPannelFilter(IEnumerable<UserProfile> users)
        {
            return users;
        }

        public IEnumerable<UserProfile> ApplyAdminFilter(IEnumerable<UserProfile> users)
        {
            var controlPanelAdminUsers = Roles.GetUsersInRole(RoleNames.ControlPanelAdmin);
            return users.Where(x => !controlPanelAdminUsers.Contains(x.UserName));
        }
    }
}