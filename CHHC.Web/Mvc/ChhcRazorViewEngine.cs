using System.Web.Mvc;

namespace CHHC.Web.Mvc
{
    public class ChhcRazorViewEngine : RazorViewEngine
    {
        public ChhcRazorViewEngine()
        {
            base.ViewLocationFormats = new[] {
                "~/Views/{1}/{0}.cshtml",
                "~/Views/{1}/{0}.vbhtml",
                "~/Views/Shared/{0}.cshtml",
                "~/Views/Shared/{0}.vbhtml",
                "~/Views/Shared/Admin/{0}.cshtml",
                "~/Views/Shared/Admin/{0}.vbhtml"
            };

            base.MasterLocationFormats = new[] {
                "~/Views/{1}/{0}.cshtml",
                "~/Views/{1}/{0}.vbhtml",
                "~/Views/Shared/{0}.cshtml",
                "~/Views/Shared/{0}.vbhtml",
                "~/Views/Shared/Admin/{0}.cshtml",
                "~/Views/Shared/Admin/{0}.vbhtml"
            };

            base.PartialViewLocationFormats = new[] {
                "~/Views/{1}/{0}.cshtml",
                "~/Views/{1}/{0}.vbhtml",
                "~/Views/Shared/{0}.cshtml",
                "~/Views/Shared/{0}.vbhtml",
                "~/Views/Shared/Admin/{0}.cshtml",
                "~/Views/Shared/Admin/{0}.vbhtml"
            };
        }
    }
}