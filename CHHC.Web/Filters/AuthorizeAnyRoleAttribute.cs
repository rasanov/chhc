using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHHC.Web.Filters
{
    public class AuthorizeAnyRoleAttribute : AuthorizeAttribute
    {
        public AuthorizeAnyRoleAttribute(params string[] roles)
        {
            Roles = String.Join(",", roles);
        }
    }
}