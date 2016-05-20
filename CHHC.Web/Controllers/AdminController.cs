using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using CHHC.DAL.Infrastructure;
using CHHC.DomainModel;
using CHHC.Web.Models;
using AutoMapper;
using CHHC.Web.Helpers;
using System;
using System.IO;
using CHHC.Web.Services;
using Microsoft.Ajax.Utilities;
using Novacode;
using CHHC.Web.Filters;
using System.Web;

namespace CHHC.Web.Controllers
{
    [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.Training_Edit)]
    public class AdminController : BaseAdminController
    {
        protected override IEnumerable<UserProfile> ApplyFilter(IEnumerable<UserProfile> users)
        {
            var userService = new UserService();
            users = userService.ApplyAdminFilter(users);
            return users;
        }

        protected override IEnumerable<string> ApplyFilter(string[] roles)
        {
            return roles.Where(x => x != RoleNames.ControlPanelAdmin);
        }


        public ActionResult Trainings(string message, string warning)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Trainings;
            ViewBag.StatusMessage = message;
            ViewBag.Warning = warning;
            List<ViewTraining> viewTrainings;
            using (var context = new DomainContext())
            {
                var trainings = context.Trainings.ToList();
                viewTrainings = Mapper.Map<List<ViewTraining>>(trainings);
            }
            return View(viewTrainings);
        }

        public ActionResult CreateTraining()
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Trainings;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTraining(CreateTraining model)
        {
            var adminControllerHelper = new AdminControllerHelper(Request, Server);

            ViewBag.ActiveManuItem = AccountMenuItems.Trainings;
            if (ModelState.IsValid)
            {
                var audioMediaFiles = new List<HttpPostedFileBase>();
                var errors = new List<string>();
                adminControllerHelper.AddYoutubeMediaToModelFromRequest(model.YoutubeMediaTitles, model.YoutubeMediaUrls, errors);
                adminControllerHelper.AddAudioMediaToModelFromRequest(model.AudioMediaTitles, audioMediaFiles, errors);
                adminControllerHelper.AddGoogleDriveMediaToModelFromRequest(model.GoogleDriveMediaTitles, model.GoogleDriveMediaUrls, errors);

                var isTitleValid = TrainingHelper.NewTrainingValidate(model.Title);
                var hasErrors = errors.Count > 0;
                if (!isTitleValid || hasErrors)
                {
                    if (!isTitleValid)
                    {
                        ModelState.AddModelError("", "There is a training with such a title.");
                    }
                    if (hasErrors)
                    {
                        foreach (var error in errors)
                        {
                            ModelState.AddModelError("", error);
                        }
                    }
                    return View(model);
                }

                try
                {
                    using (var context = new DomainContext())
                    {
                        var training = new Training
                        {
                            Title = model.Title,
                            Text = model.Text,
                            DurationMinutes = model.DurationMinutes,
                            InstructorName = model.InstructorName,
                            InstructorTitle = model.InstructorTitle
                        };

                        adminControllerHelper.AddYoutubeMediaToContextFromModel(model.YoutubeMediaTitles, model.YoutubeMediaUrls, training, context);
                        adminControllerHelper.AddAudioMediaToContextFromModel(model.AudioMediaTitles, audioMediaFiles, training, context);
                        adminControllerHelper.AddGoogleDriveMediaToContextFromModel(model.GoogleDriveMediaTitles, model.GoogleDriveMediaUrls, training, context);

                        context.Trainings.Add(training);
                        context.Entry(training).State = EntityState.Added;
                        context.SaveChanges();
                    }

                    return RedirectToAction("Trainings", new { Message = "Training '" + model.Title + "' has been created." });
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }

            return View(model);
        }

        public ActionResult EditTraining(int id, string message)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Trainings;
            ViewBag.StatusMessage = message;
            using (var context = new DomainContext())
            {
                var training = context.Trainings.First(x => x.Id == id);
                var model = Mapper.Map<EditTraining>(training);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTraining(EditTraining model)
        {
            var adminControllerHelper = new AdminControllerHelper(Request, Server);

            ViewBag.ActiveManuItem = AccountMenuItems.Trainings;
            if (ModelState.IsValid)
            {
                var audioMediaFiles = new List<HttpPostedFileBase>();
                var audioFileChanged = new List<bool>();
                var errors = new List<string>();
                adminControllerHelper.AddYoutubeMediaToModelFromRequest(model.YoutubeMediaTitles, model.YoutubeMediaUrls, errors);
                adminControllerHelper.AddChangedAudioMediaToModelFromRequest(model.AudioMediaTitles, model.AudioMediaUrls, audioMediaFiles, audioFileChanged, errors);
                adminControllerHelper.AddGoogleDriveMediaToModelFromRequest(model.GoogleDriveMediaTitles, model.GoogleDriveMediaUrls, errors);

                var isTitleValid = TrainingHelper.ExistingTrainingValidate(model.Id, model.Title);
                var hasErrors = errors.Count > 0;
                if (!isTitleValid || hasErrors)
                {
                    if (!isTitleValid)
                    {
                        ModelState.AddModelError("", "There is a training with such a title.");
                    }
                    if (hasErrors)
                    {
                        foreach (var error in errors)
                        {
                            ModelState.AddModelError("", error);
                        }
                    }
                    return View(model);
                }

                try
                {
                    using (var context = new DomainContext())
                    {
                        var training = context.Trainings.First(x => x.Id == model.Id);

                        training.Title = model.Title;
                        training.Text = model.Text;
                        training.DurationMinutes = model.DurationMinutes;
                        training.InstructorName = model.InstructorName;
                        training.InstructorTitle = model.InstructorTitle;
                        context.SaveChanges();

                        adminControllerHelper.EditYoutubeMedia(model.YoutubeMediaTitles, model.YoutubeMediaUrls, training, context);
                        adminControllerHelper.EditAudioMedia(model.AudioMediaTitles, audioMediaFiles, audioFileChanged, training, context);
                        adminControllerHelper.EditGoogleDriveMedia(model.GoogleDriveMediaTitles, model.GoogleDriveMediaUrls, training, context);
                    }

                    return RedirectToAction("EditTraining", new { Id = model.Id, Message = "Training has been changed." });
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTraining(int id)
        {
            try
            {
                using (var context = new DomainContext())
                {
                    var training = new Training {Id = id};
                    context.Trainings.Attach(training);
                    context.Trainings.Remove(training);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Trainings", new { Warning = "Training can not be deleted. " + e.Message });
            }

            return RedirectToAction("Trainings", new { Message = "Training has been deleted." });
        }

        public ActionResult TrainingUsers(int id, string trainingTitle, string message)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Trainings;
            ViewBag.StatusMessage = message;
            
            List<UserSettingsTraining> userSettingsTrainings;
            List<int> userIds;
            using (var context = new DomainContext())
            {
                userSettingsTrainings = context.UserSettingsTraining
                    .Where(x => x.Training.Id == id)
                    .Include(x => x.UserSettings)
                    .ToList();

                userIds = userSettingsTrainings.Select(x => x.UserSettings.MembershipUserId).ToList();
            }

            IEnumerable<UserProfile> allUsers;
            using (var context = new UsersContext())
            {
                allUsers = GetUserProfiles(context);
            }

            var allTrainingUser = new List<TrainingUser>();
            foreach (var user in allUsers)
            {
                bool isCompleted;
                DateTime? completedDate;

                var userSettingsTraining = userSettingsTrainings.SingleOrDefault(x => x.UserSettings.MembershipUserId == user.UserId);
                if (userSettingsTraining != default(UserSettingsTraining))
                {
                    isCompleted = userSettingsTraining.IsCompleted;
                    completedDate = userSettingsTraining.CompletedDate;
                }
                else
                {
                    isCompleted = false;
                    completedDate = null;
                }

                allTrainingUser.Add(
                    new TrainingUser
                    {
                        UserProfile = user,
                        HasCompleted = isCompleted,
                        CompletedDate = completedDate
                    }
                );
            }

            var model = new TrainingUsers
            {
                TrainingId = id,
                TrainingTitle = trainingTitle,
                UserIds = userIds,
                AllUsers = allTrainingUser
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TrainingUsers(int id, string trainingTitle, int[] userIds)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Trainings;
            try
            {
                SaveTrainingUsers(id, userIds);

                return RedirectToAction("TrainingUsers", new { Id = id, TrainingTitle = trainingTitle, Message = "Users have been saved." });
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("", "Users can not be saved. " + exception.Message);
            }

            List<UserSettingsTraining> userSettingsTrainings;
            using (var context = new DomainContext())
            {
                userSettingsTrainings = context.UserSettingsTraining
                    .Where(x => x.Training.Id == id)
                    .Include(x => x.UserSettings)
                    .ToList();
            }

            IEnumerable<UserProfile> allUsers;
            using (var context = new UsersContext())
            {
                allUsers = GetUserProfiles(context);
            }

            var allTrainingUser = new List<TrainingUser>();
            foreach (var user in allUsers)
            {
                bool isCompleted;
                DateTime? completedDate;

                var userSettingsTraining = userSettingsTrainings.SingleOrDefault(x => x.UserSettings.MembershipUserId == user.UserId);
                if (userSettingsTraining != default(UserSettingsTraining))
                {
                    isCompleted = userSettingsTraining.IsCompleted;
                    completedDate = userSettingsTraining.CompletedDate;
                }
                else
                {
                    isCompleted = false;
                    completedDate = null;
                }

                allTrainingUser.Add(
                    new TrainingUser
                    {
                        UserProfile = user,
                        HasCompleted = isCompleted,
                        CompletedDate = completedDate
                    }
                );
            }

            var model = new TrainingUsers
            {
                TrainingId = id,
                TrainingTitle = trainingTitle,
                UserIds = (userIds == null ? new List<int>() : userIds.ToList()),
                AllUsers = allTrainingUser
            };

            return View(model);
        }

        private void SaveTrainingUsers(int id, int[] userIds)
        {
            bool usersListNotEmpty = userIds != null && userIds.Length > 0;

            using (var context = new DomainContext())
            {
                if (usersListNotEmpty)
                {
                    context.UserSettingsTraining
                        .Where(x => x.Training.Id == id && !userIds.Contains(x.UserSettings.MembershipUserId))
                        .ToList()
                        .ForEach(x => context.Entry(x).State = EntityState.Deleted);
                    context.SaveChanges();
                }
                else
                {
                    context.UserSettingsTraining
                        .Where(x => x.Training.Id == id)
                        .ToList()
                        .ForEach(x => context.Entry(x).State = EntityState.Deleted);
                    context.SaveChanges();
                }

                if (usersListNotEmpty)
                {
                    var existingUserIds = context.UserSettingsTraining
                        .Where(x => x.Training.Id == id)
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

                    var training = new Training {Id = id};
                    context.Trainings.Attach(training);

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

                        context.UserSettingsTraining.Add(
                            new UserSettingsTraining
                            {
                                UserSettings = userSettings,
                                Training = training,
                                IsCompleted = false
                            }
                        );
                    }
                    context.SaveChanges();
                }
            }
        }

        public ActionResult UserTrainings(int id, string userName, string returnControllerName, string message)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Users;
            ViewBag.StatusMessage = message;

            List<int> trainingIds;
            IEnumerable<Training> allTrainings;
            List<UserSettingsTraining> userSettingsTrainings;
            using (var context = new DomainContext())
            {
                trainingIds = context.UserSettingsTraining
                    .Where(x => x.UserSettings.MembershipUserId == id)
                    .Select(x => x.Training.Id)
                    .ToList();

                allTrainings = context.Trainings.ToList();

                userSettingsTrainings = context.UserSettingsTraining.Where(x => x.UserSettings.MembershipUserId == id).ToList();
            }

            var allViewTrainings = Mapper.Map<List<UserTraining>>(allTrainings);
            foreach (var userSettingsTraining in userSettingsTrainings)
            {
                var viewTraining = allViewTrainings.Single(x => x.Id == userSettingsTraining.Training.Id);
                viewTraining.IsCompleted = userSettingsTraining.IsCompleted;
                viewTraining.CompletedDate = userSettingsTraining.CompletedDate;
            }

            var model = new UserTrainings
            {
                UserId = id,
                UserName = userName,
                TrainingIds = trainingIds,
                AllTrainings = allViewTrainings,
                ReturnControllerName = returnControllerName
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserTrainings(int id, string userName, string returnControllerName, int[] trainingIds)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Users;
            try
            {
                SaveUserTrainings(id, trainingIds);

                return RedirectToAction("UserTrainings", new { userId = id, userName, returnControllerName, Message = "Trainings have been saved." });
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("", "Trainings can not be saved. " + exception.Message);
            }

            IEnumerable<Training> allTrainings;
            List<UserSettingsTraining> userSettingsTrainings;
            using (var context = new DomainContext())
            {
                allTrainings = context.Trainings.ToList();

                userSettingsTrainings = context.UserSettingsTraining.Where(x => x.UserSettings.MembershipUserId == id).ToList();
            }

            var allViewTrainings = Mapper.Map<List<UserTraining>>(allTrainings);
            foreach (var userSettingsTraining in userSettingsTrainings)
            {
                var viewTraining = allViewTrainings.Single(x => x.Id == userSettingsTraining.Training.Id);
                viewTraining.IsCompleted = userSettingsTraining.IsCompleted;
                viewTraining.CompletedDate = userSettingsTraining.CompletedDate;
            }

            var model = new UserTrainings
            {
                UserId = id,
                UserName = userName,
                TrainingIds = trainingIds == null ? new List<int>() : trainingIds.ToList(),
                AllTrainings = allViewTrainings,
                ReturnControllerName = returnControllerName
            };

            return View(model);
        }

        private static void SaveUserTrainings(int userId, int[] trainingIds)
        {
            bool trainingsListNotEmpty = trainingIds != null && trainingIds.Length > 0;

            using (var context = new DomainContext())
            {
                var userSettings = context.UserSettings.FirstOrDefault(x => x.MembershipUserId == userId);
                if (userSettings != default(UserSettings))
                {
                    if (trainingsListNotEmpty)
                    {
                        userSettings.UserSettingsTrainings
                            .Where(x => !trainingIds.Contains(x.Training.Id))
                            .ToList()
                            .ForEach(x => context.Entry(x).State = EntityState.Deleted);
                        context.SaveChanges();
                    }
                    else
                    {
                        userSettings.UserSettingsTrainings.ToList()
                            .ForEach(x => context.Entry(x).State = EntityState.Deleted);
                        context.SaveChanges();
                    }
                }
                else
                {
                    userSettings = new UserSettings
                    {
                        MembershipUserId = userId
                    };
                    context.UserSettings.Add(userSettings);
                }

                if (trainingsListNotEmpty)
                {
                    var existingTrainingIds = userSettings.UserSettingsTrainings.Select(u => u.Training.Id).ToList();
                    var trainingsToAdd = context.Trainings
                        .Where(x => trainingIds.Contains(x.Id) && !existingTrainingIds.Contains(x.Id))
                        .ToList();

                    foreach (var trainingToAdd in trainingsToAdd)
                    {
                        context.UserSettingsTraining.Add(
                            new UserSettingsTraining
                            {
                                UserSettings = userSettings,
                                Training = trainingToAdd,
                                IsCompleted = false
                            }
                            );
                    }
                    context.SaveChanges();
                }
            }
        }

        public ActionResult TrainingQuiz(int id, string message)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Trainings;
            ViewBag.StatusMessage = message;

            List<MultichoiceQuestion> multichoiceQuestions;
            string trainingTitle;
            using (var context = new DomainContext())
            {
                multichoiceQuestions = context.MultichoiceQuestions
                    .Where(x => x.Training.Id == id)
                    .Include(x => x.Answers)
                    .ToList();

                trainingTitle = context.Trainings.Single(x => x.Id == id).Title;
            }

            var questions = Mapper.Map<List<EditMultichoiceQuestion>>(multichoiceQuestions);
            var quiz = new Quiz
            {
                TrainingId = id,
                TrainingTitle = trainingTitle,
                Questions = questions
            };
            return View(quiz);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TrainingQuiz(Quiz quiz, string command)
        {
            try
            {
                CollectTrainingQuizData(quiz);
                if (!quiz.Validate())
                {
                    ViewBag.Warning = "Quiz is not valid. Please check that each question has at least one answer chosen.";
                    return View(quiz);
                }
                SaveTrainingQuiz(quiz);
                return RedirectToAction("TrainingQuiz", new {quiz.TrainingId, Message = "Training quiz has been saved."});
            }
            catch (Exception exception)
            {
                ViewBag.Warning = "Training quiz can not be saved. " + exception.Message;
            }
            return View(quiz);
        }

        private void CollectTrainingQuizData(Quiz quiz)
        {
            string maxQuestionNumberValue = Request.Form["maxQuestionNumber"];
            int maxQuestionNumber = int.Parse(maxQuestionNumberValue);

            for (int questionIterator = 1; questionIterator <= maxQuestionNumber; questionIterator++)
            {
                string questionText = Request.Form["Question" + questionIterator];
                if (string.IsNullOrWhiteSpace(questionText))
                {
                    continue;
                }

                var multichoiceQuestion = new EditMultichoiceQuestion
                {
                    Question = questionText
                };

                for (var answerIterator = 1; answerIterator <= 10; answerIterator++)
                {
                    string answerText = Request.Form["Question" + questionIterator + "Answer" + answerIterator];
                    if (string.IsNullOrWhiteSpace(answerText))
                    {
                        continue;
                    }
                    string answerIsCorrectText = Request.Form["Question" + questionIterator + "Answer" + answerIterator + "IsCorrect"];
                    bool answerIsCorrect = !string.IsNullOrWhiteSpace(answerIsCorrectText);

                    var answer = new EditAnswer
                    {
                        Text = answerText,
                        IsCorrect = answerIsCorrect
                    };
                    multichoiceQuestion.Answers.Add(answer);
                }

                quiz.Questions.Add(multichoiceQuestion);
            }
        }

        private void SaveTrainingQuiz(Quiz quiz)
        {
            using (var context = new DomainContext())
            {
                context.Answers
                    .Where(x => x.MultichoiceQuestion.Training.Id == quiz.TrainingId)
                    .ToList()
                    .ForEach(x => context.Entry(x).State = EntityState.Deleted);
                context.SaveChanges();

                context.MultichoiceQuestions
                    .Where(x => x.Training.Id == quiz.TrainingId)
                    .ToList()
                    .ForEach(x => context.Entry(x).State = EntityState.Deleted);
                context.SaveChanges();

                var training = new Training { Id = quiz.TrainingId };
                context.Trainings.Attach(training);

                foreach (var question in quiz.Questions)
                {
                    var multichoiceQuestion = new MultichoiceQuestion
                    {
                        Training = training,
                        Question = question.Question
                    };

                    foreach (var editAnswer in question.Answers)
                    {
                        var answer = new Answer
                        {
                            MultichoiceQuestion = multichoiceQuestion,
                            Text = editAnswer.Text,
                            IsCorrect = editAnswer.IsCorrect
                        };
                        multichoiceQuestion.Answers.Add(answer);
                    }

                    context.MultichoiceQuestions.Add(multichoiceQuestion);
                }

                context.SaveChanges();
            }
        }

        public ActionResult OpenCompletionForm(int trainingId, int userId)
        {
            Training training;
            UserSettingsTraining userSettingsTraining;
            using (var context = new DomainContext())
            {
                training = context.Trainings.Single(x => x.Id == trainingId);
                userSettingsTraining = training.UserSettingsTrainings.SingleOrDefault(x => x.UserSettings.MembershipUserId == userId);
            }

            if (userSettingsTraining == default(UserSettingsTraining) || !userSettingsTraining.IsCompleted)
            {
                return null;
            }

            string title = training.Title;

            var stream = new MemoryStream();
            using (DocX document = DocX.Create(stream))
            {
                DocxHelper.ComposeCompletionForm(document, training, userSettingsTraining);
                document.Save();
            }
            stream.Position = 0;
            stream.Flush();
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = string.Format("Completion Form {0}.docx", title.Substring(0, title.Length > 20 ? 20 : title.Length)),

                // always prompt the user for downloading, set to true if you want 
                // the browser to try to show the file inline
                Inline = false,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());

            return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        }





        public ActionResult UserPatients(int id, string userName, string returnControllerName, string message)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Users;
            ViewBag.StatusMessage = message;

            List<int> patientIds;
            IEnumerable<ViewPatient> allPatients;
            using (var context = new DomainContext())
            {
                patientIds = context.UserSettingsPatient
                    .Where(x => x.UserSettings.MembershipUserId == id && x.DateUnassigned == null)
                    .Select(x => x.Patient.Id)
                    .ToList();

                var patients = context.Patients.ToList();
                allPatients = Mapper.Map<List<ViewPatient>>(patients);
            }

            var model = new UserPatients
            {
                UserId = id,
                UserName = userName,
                PatientIds = patientIds,
                AllPatients = allPatients,
                ReturnControllerName = returnControllerName
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserPatients(int id, string userName, string returnControllerName, int[] patientIds)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Users;
            try
            {
                SaveUserPatients(id, patientIds);

                return RedirectToAction("UserPatients", new { userId = id, userName, returnControllerName, Message = "Patients have been saved." });
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("", "Patients can not be saved. " + exception.Message);
            }

            IEnumerable<ViewPatient> allPatients;
            using (var context = new DomainContext())
            {
                var patients = context.Patients.ToList();
                allPatients = Mapper.Map<List<ViewPatient>>(patients);
            }

            var model = new UserPatients
            {
                UserId = id,
                UserName = userName,
                PatientIds = patientIds == null ? new List<int>() : patientIds.ToList(),
                AllPatients = allPatients,
                ReturnControllerName = returnControllerName
            };

            return View(model);
        }


        private static void SaveUserPatients(int userId, int[] patientIds)
        {
            using (var context = new DomainContext())
            {
                var userSettings = context.UserSettings.FirstOrDefault(x => x.MembershipUserId == userId);
                if (userSettings != default(UserSettings))
                {
                    UnAssign(context, patientIds, userSettings);
                }
                else
                {
                    userSettings = new UserSettings
                    {
                        MembershipUserId = userId
                    };
                    context.UserSettings.Add(userSettings);
                }

                bool patientsListNotEmpty = patientIds != null && patientIds.Length > 0;
                if (patientsListNotEmpty)
                {
                    AssignAgain(context, patientIds, userSettings);
                    AssignNew(context, patientIds, userSettings);
                }
            }
        }

        private static void UnAssign(DomainContext context, int[] patientIds, UserSettings userSettings)
        {
            bool patientsListNotEmpty = patientIds != null && patientIds.Length > 0;

            List<UserSettingsPatient> userSettingsPatients;
            if (patientsListNotEmpty)
            {
                userSettingsPatients = userSettings.UserSettingsPatients
                    .Where(x => x.DateUnassigned == default(DateTime?))
                    .Where(x => !patientIds.Contains(x.Patient.Id))
                    .ToList();
            }
            else
            {
                userSettingsPatients = userSettings.UserSettingsPatients
                    .Where(x => x.DateUnassigned == default(DateTime?))
                    .ToList();
            }

            userSettingsPatients.ForEach(x => x.DateUnassigned = DateTime.Today);
            context.SaveChanges();
        }

        private static void AssignAgain(DomainContext context, int[] patientIds, UserSettings userSettings)
        {
            userSettings.UserSettingsPatients
                .Where(x => x.DateUnassigned != default(DateTime?))
                .Where(x => patientIds.Contains(x.Patient.Id))
                .ToList()
                .ForEach(x =>
                {
                    x.DateAssigned = DateTime.Today;
                    x.DateUnassigned = null;
                });
            context.SaveChanges();
        }

        private static void AssignNew(DomainContext context, int[] patientIds, UserSettings userSettings)
        {
            var existingPatientIds = userSettings.UserSettingsPatients.Select(u => u.Patient.Id).ToList();
            var patientsToAdd = context.Patients
                .Where(x => patientIds.Contains(x.Id) && !existingPatientIds.Contains(x.Id))
                .ToList();

            foreach (var patientToAdd in patientsToAdd)
            {
                context.UserSettingsPatient.Add(
                    new UserSettingsPatient
                    {
                        UserSettings = userSettings,
                        Patient = patientToAdd,
                        DateAssigned = DateTime.Today,
                        DateUnassigned = null
                    }
                );
            }
            context.SaveChanges();
        }


        public ActionResult Help()
        {
            return View();
        }
    }
}