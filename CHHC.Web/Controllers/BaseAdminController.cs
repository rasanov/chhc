using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using CHHC.Web.Filters;
using CHHC.Web.Models;
using WebMatrix.WebData;

namespace CHHC.Web.Controllers
{
    public abstract class BaseAdminController : Controller
    {
        [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.ControlPanelAdmin)]
        public virtual ActionResult Users(string message)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Users;
            ViewBag.StatusMessage = message;
            using (var context = new UsersContext())
            {
                var model = GetUserProfiles(context);
                return View(model);
            }
        }

        [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.ControlPanelAdmin)]
        public ActionResult CreateUser()
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Users;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.ControlPanelAdmin)]
        public virtual ActionResult CreateUser(RegisterModel model)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Users;
            if (ModelState.IsValid)
            {
                var status = AccountHelper.UserProfileValidate(model.UserName, model.Email);
                if (status == MembershipCreateStatus.DuplicateUserName)
                {
                    ModelState.AddModelError(string.Empty, "This user name is used by another user.");
                    return View(model);
                }
                if (status == MembershipCreateStatus.DuplicateEmail)
                {
                    ModelState.AddModelError(string.Empty, "This email is used by another user.");
                    return View(model);
                }
                if (status != MembershipCreateStatus.Success)
                {
                    ModelState.AddModelError(string.Empty, "Validation error.");
                    return View(model);
                }

                try
                {
                    WebSecurity.CreateUserAndAccount(
                        model.UserName,
                        model.Password,
                        new
                        {
                            FullName = model.FullName,
                            Title = model.Title,
                            Email = model.Email,
                            Phone = model.Phone,
                            Notes = model.Notes,
                            InnerChhcId = model.InnerChhcId
                        },
                        false);

                    return RedirectToAction("Users", new { Message = "User '" + model.UserName + "' have been created." });
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError(string.Empty, AccountHelper.ErrorCodeToString(e.StatusCode));
                }
            }

            return View(model);
        }

        [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.ControlPanelAdmin)]
        public virtual ActionResult EditUser(string userName, string message)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Users;
            ViewBag.StatusMessage = message;
            using (var usersContext = new UsersContext())
            {
                var userProfile = usersContext.UserProfiles.First(x => !x.IsDeleted && x.UserName == userName);
                return View(userProfile);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.ControlPanelAdmin)]
        public virtual ActionResult EditUser(UserProfile model)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Users;
            if (ModelState.IsValid)
            {
                if (!AccountHelper.UserProfileEmailValidate(model))
                {
                    ModelState.AddModelError(string.Empty, "This email is used by another user.");
                    return View(model);
                }

                try
                {
                    using (var usersContext = new UsersContext())
                    {
                        var userProfile = usersContext.UserProfiles.Single(u => u.UserName == model.UserName);
                        userProfile.FullName = model.FullName;
                        userProfile.Title = model.Title;
                        userProfile.Phone = model.Phone;
                        userProfile.Email = model.Email;
                        userProfile.Notes = model.Notes;
                        userProfile.InnerChhcId = model.InnerChhcId;
                        usersContext.SaveChanges();
                    }

                    return RedirectToAction("EditUser", new { UserName = model.UserName, Message = "User data have been changed." });
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError(string.Empty, AccountHelper.ErrorCodeToString(e.StatusCode));
                }
            }

            return View(model);
        }
        
        [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.ControlPanelAdmin)]
        public virtual ActionResult ResetPassword(string userName, string message)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Users;
            ViewBag.StatusMessage = message;
            var model = new ResetPasswordModel
            {
                UserName = userName
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.ControlPanelAdmin)]
        public virtual ActionResult ResetPassword(ResetPasswordModel model)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Users;
            if (ModelState.IsValid)
            {
                bool resetPasswordSucceeded;
                try
                {
                    var token = WebSecurity.GeneratePasswordResetToken(model.UserName);
                    resetPasswordSucceeded = WebSecurity.ResetPassword(token, model.NewPassword);
                }
                catch (Exception)
                {
                    resetPasswordSucceeded = false;
                }

                if (resetPasswordSucceeded)
                {
                    return RedirectToAction("ResetPassword", new { UserName = model.UserName, Message = "Password has been reset." });
                }
                ModelState.AddModelError(string.Empty, "The new password is invalid.");
            }

            return View(model);
        }


        [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.ControlPanelAdmin)]
        public virtual ActionResult UserRoles(string userName, string message, string warning)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Users;
            ViewBag.StatusMessage = message;
            ViewBag.Warning = warning;
            var model = new UserRoles
            {
                UserName = userName,
                Roles = Roles.GetRolesForUser(userName),
                AllRoles = GeAllRoles()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.ControlPanelAdmin)]
        public virtual ActionResult UserRoles(string userName, string[] roles)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Users;
            try
            {
                var currentRoles = Roles.GetRolesForUser(userName);
                if (currentRoles.Length > 0)
                {
                    Roles.RemoveUserFromRoles(userName, currentRoles);
                }
                if (roles != null && roles.Length > 0)
                {
                    Roles.AddUserToRoles(userName, roles);
                }
                return RedirectToAction("UserRoles", new { UserName = userName, Message = "Roles has been saved." });
            }
            catch (Exception exception)
            {
                return RedirectToAction("UserRoles", new { UserName = userName, Warning = "Roles can not be saved. " + exception.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.ControlPanelAdmin)]
        public virtual ActionResult DeleteUser(string userName)
        {
            try
            {
                using (var usersContext = new UsersContext())
                {
                    var userProfile = usersContext.UserProfiles.Single(x => x.UserName == userName);
                    userProfile.IsDeleted = true;
                    usersContext.SaveChanges();
                }
                var currentRoles = Roles.GetRolesForUser(userName);
                if (currentRoles.Length > 0)
                {
                    Roles.RemoveUserFromRoles(userName, currentRoles);
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Users", new { Warning = "User can not be deleted. " + e.Message });
            }

            return RedirectToAction("Users", new { Message = "User '" + userName + "' has been deleted." });
        }



        [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.ControlPanelAdmin)]
        public virtual ActionResult ViewRoles()
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Roles;
            var model = GeAllRoles();
            return View(model);
        }

        [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.ControlPanelAdmin)]
        public virtual ActionResult RoleUsers(string role, string message, string warning)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Roles;
            ViewBag.StatusMessage = message;
            ViewBag.Warning = warning;
            var model = new RoleUsers
            {
                Role = role,
                UserNames = Roles.GetUsersInRole(role)
            };
            using (var context = new UsersContext())
            {
                model.AllUsers = GetUserProfiles(context);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.ControlPanelAdmin)]
        public virtual ActionResult RoleUsers(string role, string[] userNames)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Roles;
            try
            {
                var currentUsers = Roles.GetUsersInRole(role);
                if (currentUsers.Length > 0)
                {
                    Roles.RemoveUsersFromRole(currentUsers, role);
                }
                if (userNames != null && userNames.Length > 0)
                {
                    Roles.AddUsersToRole(userNames, role);
                }
                return RedirectToAction("RoleUsers", new { Role = role, Message = "Users has been saved." });
            }
            catch (Exception exception)
            {
                return RedirectToAction("RoleUsers", new { Role = role, Warning = "Users can not be saved. " + exception.Message });
            }
        }


        protected List<UserProfile> GetUserProfiles(UsersContext context)
        {
            var query = context.UserProfiles.Where(x => !x.IsDeleted);
            return ApplyFilter(query).ToList();
        }

        protected abstract IEnumerable<UserProfile> ApplyFilter(IEnumerable<UserProfile> users);


        protected IEnumerable<string> GeAllRoles()
        {
            var roles = Roles.GetAllRoles();
            return ApplyFilter(roles);
        }

        protected virtual IEnumerable<string> ApplyFilter(string[] roles)
        {
            return roles;
        }
    }
}
