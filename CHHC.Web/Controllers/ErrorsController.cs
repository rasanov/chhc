using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHHC.Web.Controllers
{
    public class ErrorsController : Controller
    {
        public ActionResult RequestLengthExceeded()
        {
            return View();
        }
    }
}
