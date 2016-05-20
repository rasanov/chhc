using System.IO;
using System.Text.RegularExpressions;
using CHHC.DAL.Infrastructure;
using CHHC.Web.Filters;
using CHHC.Web.Helpers;
using CHHC.Web.Models;
using System.Web.Mvc;
using System.Linq;
using AutoMapper;
using System.Collections.Generic;
using Novacode;
using WebMatrix.WebData;
using System;
using CHHC.DomainModel;
using System.Data.Entity;
using CHHC.Web.Services;

namespace CHHC.Web.Controllers
{
    [AuthorizeAnyRole(RoleNames.Training_View, RoleNames.CaseConference_View, RoleNames.Document_View)]
    public class MyChhcController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Home;

            var myChhcDashboard = new MyChhcDashboard();
            var myChhcService = new MyChhcService();
            myChhcDashboard.NotCompletedTrainingsCount = myChhcService.GetNotCompletedTrainingsCount();
            myChhcDashboard.NotFilledCaseConferencesCount = myChhcService.GetNotFilledCaseConferencesCount();
            myChhcDashboard.DocumentsCount = myChhcService.GetDocumentsCount(User.IsInRole(RoleNames.Document_View_All));

            return View(myChhcDashboard);
        }

        public ActionResult Trainings()
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Trainings;

            List<ViewUserTraining> viewUserTrainings;
            using (var context = new DomainContext())
            {
                var userSettingsTraining = context.UserSettingsTraining
                    .Where(x => x.UserSettings.MembershipUserId == WebSecurity.CurrentUserId)
                    .ToList();
                viewUserTrainings = Mapper.Map<List<ViewUserTraining>>(userSettingsTraining);
            }
            return View(viewUserTrainings);
        }

        public ActionResult ViewTraining(int id)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Trainings;

            PublicTraining publicTraining;
            using (var context = new DomainContext())
            {
                var training = context.Trainings.First(x => x.Id == id);
                publicTraining = Mapper.Map<PublicTraining>(training);

                int userId = WebSecurity.GetUserId(User.Identity.Name);
                var userSettingsTraining = training.UserSettingsTrainings
                    .Single(x =>
                        x.Training.Id == id
                        && x.UserSettings.MembershipUserId == userId);
                publicTraining.IsCompleted = userSettingsTraining.IsCompleted;
                publicTraining.CompletedDate = userSettingsTraining.CompletedDate;
            }
            return View(publicTraining);
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

            var questions = Mapper.Map<List<PublicMultichoiceQuestion>>(multichoiceQuestions);
            var quiz = new PublicQuiz
            {
                TrainingId = id,
                TrainingTitle = trainingTitle,
                Questions = questions
            };
            return View(quiz);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TrainingQuiz(PublicQuiz quiz)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Trainings;

            List<MultichoiceQuestion> multichoiceQuestions;
            using (var context = new DomainContext())
            {
                multichoiceQuestions = context.MultichoiceQuestions
                    .Where(x => x.Training.Id == quiz.TrainingId)
                    .Include(x => x.Answers)
                    .ToList();
            }
            var questions = Mapper.Map<List<PublicMultichoiceQuestion>>(multichoiceQuestions);
            quiz.Questions = questions;

            CollectTrainingQuizData(quiz);
            CheckAnswers(quiz);

            if (quiz.IsPassed())
            {
                using (var context = new DomainContext())
                {
                    var userSettingsTraining = context.UserSettingsTraining.Single(x =>
                        x.UserSettings.MembershipUserId == WebSecurity.CurrentUserId
                        && x.Training.Id == quiz.TrainingId);

                    userSettingsTraining.IsCompleted = true;
                    userSettingsTraining.CompletedDate = DateTime.Today;
                    context.SaveChanges();
                }

                ViewBag.StatusMessage = "Training quiz has been passed with score " + quiz.GetAnsweredCorrectlyCount() + " out of " + quiz.Questions.Count + ".";
                return View(quiz);
            }

            ViewBag.Warning = "Training quiz has been failed with score " + quiz.GetAnsweredCorrectlyCount() + " out of " + quiz.Questions.Count + ".";
            return View(quiz);
        }

        private void CollectTrainingQuizData(PublicQuiz quiz)
        {
            var allKeys = Request.Form.AllKeys;
            var regex = new Regex(@"Question(?<questionId>\d*)Answer(?<answerId>\d*)IsCorrect");
            foreach (var key in allKeys)
            {
                Match match = regex.Match(key);
                if (!match.Success)
                {
                    continue;
                }

                int questionId;
                if (!int.TryParse(match.Groups["questionId"].Value, out questionId))
                {
                    continue;
                }
                var question = quiz.Questions.SingleOrDefault(x => x.Id == questionId);
                if (question == default(PublicMultichoiceQuestion))
                {
                    continue;
                }

                int answerId;
                if (!int.TryParse(match.Groups["answerId"].Value, out answerId))
                {
                    continue;
                }
                var answer = question.Answers.SingleOrDefault(x => x.Id == answerId);
                if (answer == default(PublicAnswer))
                {
                    continue;
                }

                answer.IsChosenAsCorrect = true;
            }
        }

        private void CheckAnswers(PublicQuiz quiz)
        {
            foreach (var question in quiz.Questions)
            {
                if (question.Answers.Any(IsAnsweredWrong))
                {
                    question.AnsweredCorrectly = false;
                }
                else
                {
                    question.AnsweredCorrectly = true;
                }
            }
        }

        private bool IsAnsweredWrong(PublicAnswer answer)
        {
            if (answer.IsCorrect && !answer.IsChosenAsCorrect)
            {
                return true;
            }
            if (!answer.IsCorrect && answer.IsChosenAsCorrect)
            {
                return true;
            }
            return false;
        }
        
        public ActionResult OpenCompletionForm(int trainingId)
        {
            int userId = WebSecurity.CurrentUserId;

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
        
        public ActionResult Help()
        {
            return View();
        }
    }
}