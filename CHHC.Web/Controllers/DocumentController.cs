using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CHHC.DAL.Infrastructure;
using CHHC.DomainModel;
using CHHC.Web.Models;
using WebMatrix.WebData;
using CHHC.Web.Services;
using System.Threading;

namespace CHHC.Web.Controllers
{
    public class DocumentController : Controller
    {
        private volatile bool isDownloaded = false;

        [HttpGet]
        public ActionResult All(string message)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Documents;
            ViewBag.StatusMessage = message;

            List<ViewDocument> documentsAssignedToCurrentUser;
            var isUserInDocumentViewAllRole = AccountHelper.CanViewAllDocuments(User);
            using (var context = new DomainContext())
            {
                var documentsAssignedExplicitly = context.UserSettingsDocuments
                    .Where(x =>
                        x.UserSettings.MembershipUserId == WebSecurity.CurrentUserId
                    )
                    .Where(x => !x.IsDownloaded)
                    .Select(x => x.Document);

                var documentsAssignedToAll = context.Documents
                    .Where(x => 
                        x.Status == DocumentStatus.AssignedToAll
                        && isUserInDocumentViewAllRole
                    );

                var documents = documentsAssignedExplicitly.Concat(documentsAssignedToAll).ToList();

                documentsAssignedToCurrentUser = Mapper.Map<List<ViewDocument>>(documents);
            }
            using (var userContext = new UsersContext())
            {
                foreach (var viewDocument in documentsAssignedToCurrentUser)
                {
                    var userName = userContext.UserProfiles.Single(x => x.UserId == viewDocument.UploadedByMembershipUserId).UserName;
                    viewDocument.UploadedByMembershipUserName = userName;
                }
            }

            List<ViewDocument> documentsUploadedByCurrentUser;
            using (var context = new DomainContext())
            {
                var documents = context.Documents
                    .Where(x => x.UploadedBy.MembershipUserId == WebSecurity.CurrentUserId)
                    .ToList();
                documentsUploadedByCurrentUser = Mapper.Map<List<ViewDocument>>(documents);
            }
            using (var userContext = new UsersContext())
            {
                foreach (var viewDocument in documentsUploadedByCurrentUser)
                {
                    var userName = userContext.UserProfiles.Single(x => x.UserId == viewDocument.UploadedByMembershipUserId).UserName;
                    viewDocument.UploadedByMembershipUserName = userName;
                }
            }

            var model = new ViewDocuments
            {
                DocumentsAssignedToCurrentUser = documentsAssignedToCurrentUser,
                DocumentsUploadedByCurrentUser = documentsUploadedByCurrentUser
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult AssignedDocuments()
        {
            int noInfoniteLoop = 0;
            while (!isDownloaded)
            {
                Thread.Sleep(100);
                noInfoniteLoop++;
                if (noInfoniteLoop == 50)
                {
                    break;
                }
            }

            List<ViewDocument> documentsAssignedToCurrentUser;
            var isUserInDocumentViewAllRole = AccountHelper.CanViewAllDocuments(User);
            using (var context = new DomainContext())
            {
                var documentsAssignedExplicitly = context.UserSettingsDocuments
                    .Where(x =>
                        x.UserSettings.MembershipUserId == WebSecurity.CurrentUserId
                    )
                    .Where(x => !x.IsDownloaded)
                    .Select(x => x.Document);

                var documentsAssignedToAll = context.Documents
                    .Where(x =>
                        x.Status == DocumentStatus.AssignedToAll
                        && isUserInDocumentViewAllRole
                    );

                var documents = documentsAssignedExplicitly.Concat(documentsAssignedToAll).ToList();

                documentsAssignedToCurrentUser = Mapper.Map<List<ViewDocument>>(documents);
            }
            using (var userContext = new UsersContext())
            {
                foreach (var viewDocument in documentsAssignedToCurrentUser)
                {
                    var userName = userContext.UserProfiles.Single(x => x.UserId == viewDocument.UploadedByMembershipUserId).UserName;
                    viewDocument.UploadedByMembershipUserName = userName;
                }
            }

            return PartialView(documentsAssignedToCurrentUser);
        }

        [HttpGet]
        public ActionResult New(string documentFile, string message)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Documents;
            ViewBag.StatusMessage = message;

            var uploadDocument = new UploadDocument
            {
                DocumentFile = documentFile
            };

            return View(uploadDocument);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(UploadDocument uploadDocument)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Documents;

            HttpPostedFileBase file = Request.Files["Document"];
            var isFileValid = (file != null && file.ContentLength > 0);
            if (!isFileValid)
            {
                return RedirectToAction("New", new { DocumentFile = uploadDocument.DocumentFile, Message = "Document is not selected or is not valid." });
            }
            if (!file.ContentType.Contains("image")
                && file.ContentType != "application/pdf"
                && file.ContentType != "application/msword")
            {
                return RedirectToAction("New", new { DocumentFile = uploadDocument.DocumentFile, Message = "Document extension is not supported." });
            }

            try
            {
                string timeStamp = DateTime.Now.Ticks.ToString();
                var fileName = Path.GetFileName(file.FileName);
                var uniqueFileName = timeStamp + "_" + fileName;
                var filePath = Path.Combine(Server.MapPath("~/documentExchange/"), uniqueFileName);
                file.SaveAs(filePath);

                DocumentStatus documentStatus;
                if (User.IsInRole(RoleNames.CHHCAdmin))
                {
                    documentStatus = DocumentStatus.Uploaded;
                }
                else
                {
                    documentStatus = DocumentStatus.AssignedToAll;
                }

                using (var context = new DomainContext())
                {
                    var currentUserSettings = context.UserSettings.SingleOrDefault(x => x.MembershipUserId == WebSecurity.CurrentUserId);
                    if (currentUserSettings == default(UserSettings))
                    {
                        currentUserSettings = new UserSettings
                        {
                            MembershipUserId = WebSecurity.CurrentUserId
                        };
                        context.UserSettings.Add(currentUserSettings);
                    }

                    var document = new Document
                    {
                        ContentType = file.ContentType,
                        DateUploaded = DateTime.Now,
                        UploadedBy = currentUserSettings,
                        FileName = fileName,
                        FilePath = filePath,
                        Status = documentStatus
                    };
                    context.Documents.Add(document);
                    context.Entry(document).State = EntityState.Added;
                    context.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                return RedirectToAction("New", new { DocumentFile = uploadDocument.DocumentFile, Message = "Upload failed with exception: " + exception.Message });
            }

            return RedirectToAction("All", new { Message = "Document '" + uploadDocument.DocumentFile + "' was successfully uploaded." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                using (var context = new DomainContext())
                {
                    var document = context.Documents.Single(x => x.Id == id);

                    if (System.IO.File.Exists(document.FilePath))
                    {
                        System.IO.File.Delete(document.FilePath);
                    }

                    context.Documents.Attach(document);
                    context.Documents.Remove(document);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("All", new { Warning = "Document can not be deleted. " + e.Message });
            }

            return RedirectToAction("All", new { Message = "Document has been deleted." });
        }

        [HttpGet]
        public ActionResult Assign(int id, string documentFileName)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Documents;

            List<UserSettingsDocument> userSettingsDocuments;
            List<int> userIds;
            using (var context = new DomainContext())
            {
                userSettingsDocuments = context.UserSettingsDocuments
                    .Where(x => x.Document.Id == id)
                    .Include(x => x.UserSettings)
                    .ToList();

                userIds = userSettingsDocuments.Select(x => x.UserSettings.MembershipUserId).ToList();
            }

            IEnumerable<UserProfile> allUsers;
            using (var context = new UsersContext())
            {
                allUsers = GetUserProfiles(context);
            }

            var allDocumentUsers = new List<DocumentUser>();
            foreach (var user in allUsers)
            {
                bool hasDownloaded;

                var userSettingsDocument = userSettingsDocuments.SingleOrDefault(x => x.UserSettings.MembershipUserId == user.UserId);
                if (userSettingsDocument != default(UserSettingsDocument))
                {
                    hasDownloaded = userSettingsDocument.IsDownloaded;
                }
                else
                {
                    hasDownloaded = false;
                }

                allDocumentUsers.Add(
                    new DocumentUser
                    {
                        UserProfile = user,
                        HasDownloaded = hasDownloaded
                    }
                );
            }

            var model = new DocumentUsers
            {
                DocumentId = id,
                DocumentFileName = documentFileName,
                UserIds = userIds,
                AllUsers = allDocumentUsers
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Assign(int id, string documentFileName, int[] userIds)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Documents;
            try
            {
                SaveDocumentUsers(id, userIds);

                return RedirectToAction("Assign", new { Id = id, DocumentFileName = documentFileName, Message = "Assignment completed successfully." });
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("", "Assignment failed. " + exception.Message);
            }

            List<UserSettingsDocument> userSettingsDocuments;
            using (var context = new DomainContext())
            {
                userSettingsDocuments = context.UserSettingsDocuments
                    .Where(x => x.Document.Id == id)
                    .Include(x => x.UserSettings)
                    .ToList();
            }

            IEnumerable<UserProfile> allUsers;
            using (var context = new UsersContext())
            {
                allUsers = GetUserProfiles(context);
            }

            var allDocumentUsers = new List<DocumentUser>();
            foreach (var user in allUsers)
            {
                bool hasDownloaded;

                var userSettingsDocument = userSettingsDocuments.SingleOrDefault(x => x.UserSettings.MembershipUserId == user.UserId);
                if (userSettingsDocument != default(UserSettingsDocument))
                {
                    hasDownloaded = userSettingsDocument.IsDownloaded;
                }
                else
                {
                    hasDownloaded = false;
                }

                allDocumentUsers.Add(
                    new DocumentUser
                    {
                        UserProfile = user,
                        HasDownloaded = hasDownloaded
                    }
                );
            }

            var model = new DocumentUsers
            {
                DocumentId = id,
                DocumentFileName = documentFileName,
                UserIds = (userIds == null ? new List<int>() : userIds.ToList()),
                AllUsers = allDocumentUsers
            };

            return View(model);
        }

        private List<UserProfile> GetUserProfiles(UsersContext context)
        {
            var query = context.UserProfiles.Where(x => !x.IsDeleted);
            return ApplyFilter(query).ToList();
        }

        private IEnumerable<UserProfile> ApplyFilter(IEnumerable<UserProfile> users)
        {
            var userService = new UserService();
            users = userService.ApplyAdminFilter(users);
            return users;
        }

        private void SaveDocumentUsers(int id, int[] userIds)
        {
            bool usersListNotEmpty = userIds != null && userIds.Length > 0;

            using (var context = new DomainContext())
            {
                var document = context.Documents.Single(x => x.Id == id);

                if (usersListNotEmpty)
                {
                    context.UserSettingsDocuments
                        .Where(x => x.Document.Id == id && !userIds.Contains(x.UserSettings.MembershipUserId))
                        .ToList()
                        .ForEach(x => context.Entry(x).State = EntityState.Deleted);
                    context.SaveChanges();
                }
                else
                {
                    context.UserSettingsDocuments
                        .Where(x => x.Document.Id == id)
                        .ToList()
                        .ForEach(x => context.Entry(x).State = EntityState.Deleted);

                    document.Status = DocumentStatus.Uploaded;

                    context.SaveChanges();
                }

                if (usersListNotEmpty)
                {
                    var existingUserIds = context.UserSettingsDocuments
                        .Where(x => x.Document.Id == id)
                        .Select(u => u.UserSettings.MembershipUserId)
                        .ToList();

                    List<UserProfile> allUsers;
                    using (var userContext = new UsersContext())
                    {
                        allUsers = GetUserProfiles(userContext);
                    }

                    var usersToAdd = allUsers
                        .Where(x => userIds.Contains(x.UserId) && !existingUserIds.Contains(x.UserId))
                        .ToList();

                    var userSettingsList = context.UserSettings.Where(x => userIds.Contains(x.MembershipUserId)).ToList();
                    foreach (var user in usersToAdd)
                    {
                        UserSettings userSettings = userSettingsList.SingleOrDefault(x => x.MembershipUserId == user.UserId);
                        if (userSettings == default(UserSettings))
                        {
                            userSettings = new UserSettings
                            {
                                MembershipUserId = user.UserId
                            };
                            context.UserSettings.Add(userSettings);
                        }

                        context.UserSettingsDocuments.Add(
                            new UserSettingsDocument
                            {
                                UserSettings = userSettings,
                                Document = document,
                                IsDownloaded = false
                            }
                        );
                    }

                    document.Status = DocumentStatus.AssignedToTargets;

                    context.SaveChanges();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult Download(int id)
        {
            string fileName;
            string filePath;
            string contentType;
            bool deletePlease = false;

            try
            {
                using (var context = new DomainContext())
                {
                    var document = context.Documents.Single(x => x.Id == id);

                    filePath = document.FilePath;
                    contentType = document.ContentType;
                    fileName = document.FileName;

                    if (document.Status == DocumentStatus.AssignedToTargets)
                    {
                        var userSettingsDocument = context.UserSettingsDocuments.SingleOrDefault(x =>
                            x.Document.Id == id
                            && x.UserSettings.MembershipUserId == WebSecurity.CurrentUserId);
                        if (userSettingsDocument != default(UserSettingsDocument))
                        {
                            userSettingsDocument.IsDownloaded = true;
                            context.SaveChanges();

                            var dowloadedByEveryone = !context.UserSettingsDocuments.Any(x => x.Document.Id == id && !x.IsDownloaded);
                            if (dowloadedByEveryone)
                            {
                                context.Documents.Remove(document);
                                context.SaveChanges();
                                deletePlease = true;
                            }
                        }
                    }
                }

                isDownloaded = true;

                var fileContent = System.IO.File.ReadAllBytes(filePath);

                if (deletePlease)
                {
                    System.IO.File.Delete(filePath);
                }

                return File(fileContent, contentType, fileName);
            }
            catch
            {
                return null;
            }
        }
    }
}