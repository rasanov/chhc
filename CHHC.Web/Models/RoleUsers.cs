using System.Collections.Generic;

namespace CHHC.Web.Models
{
    public class RoleUsers
    {
        public string Role { get; set; }
        public string[] UserNames { get; set; }
        public List<UserProfile> AllUsers { get; set; }
    }
}