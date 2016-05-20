using System;
using System.Web.Mvc;
using CHHC.Web.Models;
using CHHC.Web.Services;
using WebMatrix.WebData;

namespace CHHC.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult SignIn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
                {
                    string controllerName;
                    string actionName;
                    AccountHelper.GetLandingControllerAction(model.UserName, out controllerName, out actionName);
                    return RedirectToAction(actionName: actionName, controllerName: controllerName);
                }
            }

            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        public ActionResult SignOut()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        public virtual ActionResult ChangePassword(string message)
        {
            ViewBag.StatusMessage = message;
            var model = new LocalPasswordModel
            {
                UserName = User.Identity.Name
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult ChangePassword(LocalPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                bool changePasswordSucceeded;
                try
                {
                    changePasswordSucceeded = WebSecurity.ChangePassword(model.UserName, model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePassword", new { Message = "Password has been changed." });
                }
                ModelState.AddModelError(string.Empty, "The current password is incorrect or the new password is invalid.");
            }

            return View(model);
        }

        [ChildActionOnly]
        public ActionResult HeaderMenu(AccountMenuItems activeMenuItem)
        {
            ViewBag.ActiveManuItem = activeMenuItem;

            var myChhcDashboard = new MyChhcDashboard();
            var myChhcService = new MyChhcService();
            if (User.CanViewNotEditTraining())
            {
                myChhcDashboard.NotCompletedTrainingsCount = myChhcService.GetNotCompletedTrainingsCount();
            }
            if (User.CanViewNotEditCaseConference())
            {
                myChhcDashboard.NotFilledCaseConferencesCount = myChhcService.GetNotFilledCaseConferencesCount();
            }

            return PartialView(myChhcDashboard);
        }
    }
}
