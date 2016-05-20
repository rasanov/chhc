using System.Collections.Generic;

namespace CHHC.Web.Models
{
    public class UserRoles
    {
        public string UserName { get; set; }
        public string[] Roles { get; set; }
        public IEnumerable<string> AllRoles { get; set; }
    }
}