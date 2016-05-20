using System.Collections.Generic;
using System.Web.Mvc;
using CHHC.Web.Models;
using CHHC.Web.Services;

namespace CHHC.Web.Controllers
{
    [Authorize(Roles = RoleNames.ControlPanelAdmin)]
    public class ControlPanelController : BaseAdminController
    {
        protected override IEnumerable<UserProfile> ApplyFilter(IEnumerable<UserProfile> users)
        {
            var userService = new UserService();
            users = userService.ApplyControlPannelFilter(users);
            return users;
        }
    }
}
