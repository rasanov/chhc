using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using AutoMapper;
using CHHC.DAL.Infrastructure;
using CHHC.DomainModel;
using CHHC.Web.Filters;
using CHHC.Web.Models;
using System.Data.Entity;
using WebMatrix.WebData;
using CHHC.Web.Services;

namespace CHHC.Web.Controllers
{
    [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.CaseConference_Edit, RoleNames.CaseConference_View)]
    public class CaseConferencesController : Controller
    {
        [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.CaseConference_Edit)]
        public ActionResult Compose(string message)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.CaseConferences;
            ViewBag.StatusMessage = message;
            ViewTemplateCaseConference viewTemplateCaseConference;
            List<ViewTemplateCaseConferenceQuestion> viewTemplateCaseConferenceQuestions;
            using (var context = new DomainContext())
            {
                var templateCaseConference = context.TemplateCaseConferences.First();

                var templateCaseConferenceQuestions = context
                    .TemplateCaseConferenceQuestions
                    .Where(x => x.TemplateCaseConference.Id == templateCaseConference.Id)
                    .Include(x => x.TemplateCaseConferenceTextQuestion)
                    .Include(x => x.TemplateCaseConferenceDateQuestion)
                    .Include(x => x.TemplateCaseConferenceMultichoiceQuestion)
                    .Include(x => x.TemplateCaseConferenceMultichoiceQuestion.Answers)
                    .Include(x => x.TemplateCaseConferenceMultichoiceTextQuestion)
                    .Include(x => x.TemplateCaseConferenceMultichoiceTextQuestion.Answers)
                    .OrderBy(x => x.Ordinal);
                viewTemplateCaseConference = Mapper.Map<ViewTemplateCaseConference>(templateCaseConference);
                viewTemplateCaseConferenceQuestions = Mapper.Map<List<ViewTemplateCaseConferenceQuestion>>(templateCaseConferenceQuestions);
                viewTemplateCaseConference.Questions = viewTemplateCaseConferenceQuestions;
            }
            return View(viewTemplateCaseConference);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.CaseConference_Edit)]
        public ActionResult Compose(ViewTemplateCaseConference viewTemplateCaseConference)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.CaseConferences;
            
            try
            {
                CollectTemplateCaseConferenceQuestions(viewTemplateCaseConference);
                if (!viewTemplateCaseConference.Validate())
                {
                    ViewBag.Warning = "Case conference form is not valid. Please check that each 'multi-choice' and 'multi-choice and text' question has at least one answer chosen.";
                    return View(viewTemplateCaseConference);
                }
                SaveTemplateCaseConferenceQuestions(viewTemplateCaseConference);

                return RedirectToAction("Compose", new { Message = "Case Conference form has been changed." });
            }
            catch (Exception exception)
            {
                ViewBag.Warning = "Case conference form can not be saved. " + exception.Message;
            }
            return View(viewTemplateCaseConference);
        }

        private void CollectTemplateCaseConferenceQuestions(ViewTemplateCaseConference viewTemplateCaseConference)
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

                var viewTemplateCaseConferenceQuestion = new ViewTemplateCaseConferenceQuestion();
                viewTemplateCaseConferenceQuestion.Ordinal = questionIterator;

                string questionTypeString = Request.Form["QuestionType" + questionIterator];
                var questionType = (TemplateCaseConferenceQuestionType)Enum.Parse(typeof(TemplateCaseConferenceQuestionType), questionTypeString);
                switch (questionType)
                {
                    case TemplateCaseConferenceQuestionType.Text:
                        viewTemplateCaseConferenceQuestion.Type = TemplateCaseConferenceQuestionType.Text;
                        viewTemplateCaseConferenceQuestion.TemplateCaseConferenceTextQuestion = new ViewTemplateCaseConferenceTextQuestion
                        {
                            Text = questionText
                        };
                        break;
                    case TemplateCaseConferenceQuestionType.Date:
                        viewTemplateCaseConferenceQuestion.Type = TemplateCaseConferenceQuestionType.Date;
                        viewTemplateCaseConferenceQuestion.TemplateCaseConferenceDateQuestion = new ViewTemplateCaseConferenceDateQuestion
                        {
                            Text = questionText
                        };
                        break;
                    case TemplateCaseConferenceQuestionType.Multichoice:
                        viewTemplateCaseConferenceQuestion.Type = TemplateCaseConferenceQuestionType.Multichoice;
                        viewTemplateCaseConferenceQuestion.TemplateCaseConferenceMultichoiceQuestion = new ViewTemplateCaseConferenceMultichoiceQuestion
                        {
                            Text = questionText
                        };
                        for (var answerIterator = 1; answerIterator <= 10; answerIterator++)
                        {
                            string answerText = Request.Form["Question" + questionIterator + "Answer" + answerIterator];
                            if (string.IsNullOrWhiteSpace(answerText))
                            {
                                continue;
                            }

                            var answer = new ViewTemplateCaseConferenceMultichoiceQuestionAnswer
                            {
                                Text = answerText
                            };
                            viewTemplateCaseConferenceQuestion.TemplateCaseConferenceMultichoiceQuestion.Answers.Add(answer);
                        }
                        break;
                    case TemplateCaseConferenceQuestionType.MultichoiceText:
                        viewTemplateCaseConferenceQuestion.Type = TemplateCaseConferenceQuestionType.MultichoiceText;
                        viewTemplateCaseConferenceQuestion.TemplateCaseConferenceMultichoiceTextQuestion = new ViewTemplateCaseConferenceMultichoiceTextQuestion
                        {
                            Text = questionText
                        };
                        for (var answerIterator = 1; answerIterator <= 10; answerIterator++)
                        {
                            string answerText = Request.Form["Question" + questionIterator + "Answer" + answerIterator];
                            if (string.IsNullOrWhiteSpace(answerText))
                            {
                                continue;
                            }

                            var answer = new ViewTemplateCaseConferenceMultichoiceTextQuestionAnswer
                            {
                                Text = answerText
                            };
                            viewTemplateCaseConferenceQuestion.TemplateCaseConferenceMultichoiceTextQuestion.Answers.Add(answer);
                        }
                        break;
                }

                viewTemplateCaseConference.Questions.Add(viewTemplateCaseConferenceQuestion);
            }
        }

        private void SaveTemplateCaseConferenceQuestions(ViewTemplateCaseConference viewTemplateCaseConference)
        {
            using (var context = new DomainContext())
            {
                DeleteCaseConferenceQuestions(context);

                var templateCaseConference = new TemplateCaseConference { Id = viewTemplateCaseConference.Id };
                context.TemplateCaseConferences.Attach(templateCaseConference);

                foreach (var viewTemplateCaseConferenceQuestions in viewTemplateCaseConference.Questions)
                {
                    var templateCaseConferenceQuestion = new TemplateCaseConferenceQuestion();
                    templateCaseConferenceQuestion.Ordinal = viewTemplateCaseConferenceQuestions.Ordinal;
                    templateCaseConferenceQuestion.TemplateCaseConference = templateCaseConference;
                    switch (viewTemplateCaseConferenceQuestions.Type)
                    {
                        case TemplateCaseConferenceQuestionType.Text:
                            var templateCaseConferenceTextQuestion = new TemplateCaseConferenceTextQuestion
                            {
                                Text = viewTemplateCaseConferenceQuestions.TemplateCaseConferenceTextQuestion.Text
                            };
                            templateCaseConferenceQuestion.TemplateCaseConferenceTextQuestion = templateCaseConferenceTextQuestion;
                            templateCaseConferenceQuestion.Type = TemplateCaseConferenceQuestionType.Text;
                            break;
                        case TemplateCaseConferenceQuestionType.Date:
                            var templateCaseConferenceDateQuestion = new TemplateCaseConferenceDateQuestion
                            {
                                Text = viewTemplateCaseConferenceQuestions.TemplateCaseConferenceDateQuestion.Text
                            };
                            templateCaseConferenceQuestion.TemplateCaseConferenceDateQuestion = templateCaseConferenceDateQuestion;
                            templateCaseConferenceQuestion.Type = TemplateCaseConferenceQuestionType.Date;
                            break;
                        case TemplateCaseConferenceQuestionType.Multichoice:
                            var templateCaseConferenceMultichoiceQuestion = new TemplateCaseConferenceMultichoiceQuestion
                            {
                                Text = viewTemplateCaseConferenceQuestions.TemplateCaseConferenceMultichoiceQuestion.Text
                            };

                            foreach (var viewAnswer in viewTemplateCaseConferenceQuestions.TemplateCaseConferenceMultichoiceQuestion.Answers)
                            {
                                var answer = new TemplateCaseConferenceMultichoiceQuestionAnswer
                                {
                                    Text = viewAnswer.Text
                                };
                                templateCaseConferenceMultichoiceQuestion.Answers.Add(answer);
                            }

                            templateCaseConferenceQuestion.TemplateCaseConferenceMultichoiceQuestion = templateCaseConferenceMultichoiceQuestion;
                            templateCaseConferenceQuestion.Type = TemplateCaseConferenceQuestionType.Multichoice;

                            break;
                        case TemplateCaseConferenceQuestionType.MultichoiceText:
                            var templateCaseConferenceMultichoiceTextQuestion = new TemplateCaseConferenceMultichoiceTextQuestion
                            {
                                Text = viewTemplateCaseConferenceQuestions.TemplateCaseConferenceMultichoiceTextQuestion.Text
                            };

                            foreach (var viewAnswer in viewTemplateCaseConferenceQuestions.TemplateCaseConferenceMultichoiceTextQuestion.Answers)
                            {
                                var answer = new TemplateCaseConferenceMultichoiceTextQuestionAnswer
                                {
                                    Text = viewAnswer.Text
                                };
                                templateCaseConferenceMultichoiceTextQuestion.Answers.Add(answer);
                            }

                            templateCaseConferenceQuestion.TemplateCaseConferenceMultichoiceTextQuestion = templateCaseConferenceMultichoiceTextQuestion;
                            templateCaseConferenceQuestion.Type = TemplateCaseConferenceQuestionType.MultichoiceText;

                            break;
                    }

                    context.TemplateCaseConferenceQuestions.Add(templateCaseConferenceQuestion);
                }

                context.SaveChanges();
            }
        }

        private static void DeleteCaseConferenceQuestions(DomainContext context)
        {
            context.TemplateCaseConferenceQuestions
                .ToList()
                .ForEach(x => context.Entry(x).State = EntityState.Deleted);
            context.SaveChanges();

            context.TemplateCaseConferenceTextQuestions
                .ToList()
                .ForEach(x => context.Entry(x).State = EntityState.Deleted);
            context.SaveChanges();

            context.TemplateCaseConferenceDateQuestions
                .ToList()
                .ForEach(x => context.Entry(x).State = EntityState.Deleted);
            context.SaveChanges();

            context.TemplateCaseConferenceMultichoiceQuestionAnswers
                .ToList()
                .ForEach(x => context.Entry(x).State = EntityState.Deleted);
            context.SaveChanges();
            context.TemplateCaseConferenceMultichoiceQuestions
                .ToList()
                .ForEach(x => context.Entry(x).State = EntityState.Deleted);
            context.SaveChanges();

            context.TemplateCaseConferenceMultichoiceTextQuestionAnswers
                .ToList()
                .ForEach(x => context.Entry(x).State = EntityState.Deleted);
            context.SaveChanges();
            context.TemplateCaseConferenceMultichoiceTextQuestions
                .ToList()
                .ForEach(x => context.Entry(x).State = EntityState.Deleted);
            context.SaveChanges();
        }



        [AuthorizeAnyRole(RoleNames.CaseConference_View)]
        [HttpGet]
        public ActionResult All(string message)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.CaseConferences;
            ViewBag.StatusMessage = message;

            var viewPatientCaseConferences = new ViewPatientCaseConferences();
            using (var context = new DomainContext())
            {
                var patients = context.UserSettingsPatient
                    .Where(x => x.UserSettings.MembershipUserId == WebSecurity.CurrentUserId)
                    .OrderByDescending(x => x.DateAssigned)
                    .Select(x => x.Patient)
                    .ToList();
                var viewPatients = Mapper.Map<List<ViewPatient>>(patients);
                viewPatientCaseConferences.ViewPatients = viewPatients;

                if (viewPatients.Count > 0)
                {
                    var firstViewPatient = viewPatients[0];
                    viewPatientCaseConferences.PatientId = firstViewPatient.Id;
                    viewPatientCaseConferences.PatientName = firstViewPatient.Name;

                    var caseConferences = context.CaseConferences
                        .Where(x => x.UserSettings.MembershipUserId == WebSecurity.CurrentUserId)
                        .Where(x => x.Patient.Id == viewPatientCaseConferences.PatientId)
                        .OrderByDescending(x => x.Date)
                        .ToList();
                    var viewCaseConferences = Mapper.Map<List<ViewCaseConference>>(caseConferences);

                    var userSettingsPatient = patients[0]
                        .UserSettingsPatients
                        .SingleOrDefault(x => x.UserSettings.MembershipUserId == WebSecurity.CurrentUserId);
                    if (userSettingsPatient != default(UserSettingsPatient))
                    {
                        var caseConferenceService = new CaseConferenceService();
                        viewCaseConferences = caseConferenceService.MergeMissed(userSettingsPatient, viewCaseConferences, viewPatientCaseConferences.PatientId, WebSecurity.CurrentUserId);
                        
                        if (viewCaseConferences.First().Status != ViewCaseConferenceStatus.Submitted
                            &&
                            !userSettingsPatient.DateUnassigned.HasValue)
                        {
                            viewPatientCaseConferences.IsCurrentFormPending = true;
                        }
                    }

                    viewPatientCaseConferences.ViewCaseConferences = viewCaseConferences;
                }
                else
                {
                    ViewBag.StatusMessage = "There are no patients assigned.";
                }
            }

            var title = "Case Conferences";
            if (!string.IsNullOrEmpty(viewPatientCaseConferences.PatientName))
            {
                title = title + " for patient '" + viewPatientCaseConferences.PatientName + "'";
            }
            ViewBag.Title = title;

            return View(viewPatientCaseConferences);
        }

        [AuthorizeAnyRole(RoleNames.CaseConference_View)]
        public ActionResult AllFilter(ViewPatientCaseConferences viewPatientCaseConferences, string message)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.CaseConferences;
            ViewBag.StatusMessage = message;

            using (var context = new DomainContext())
            {
                var patients = context.UserSettingsPatient
                    .Where(x => x.UserSettings.MembershipUserId == WebSecurity.CurrentUserId)
                    .OrderByDescending(x => x.DateAssigned)
                    .Select(x => x.Patient)
                    .ToList();
                var viewPatients = Mapper.Map<List<ViewPatient>>(patients);
                viewPatientCaseConferences.ViewPatients = viewPatients;

                var caseConferences = context.CaseConferences
                    .Where(x => x.UserSettings.MembershipUserId == WebSecurity.CurrentUserId)
                    .Where(x => x.Patient.Id == viewPatientCaseConferences.PatientId)
                    .OrderByDescending(x => x.Date)
                    .ToList();
                var viewCaseConferences = Mapper.Map<List<ViewCaseConference>>(caseConferences);

                var userSettingsPatient = patients
                    .Single(x => x.Id == viewPatientCaseConferences.PatientId)
                    .UserSettingsPatients
                    .SingleOrDefault(x => x.UserSettings.MembershipUserId == WebSecurity.CurrentUserId);
                if (userSettingsPatient != default(UserSettingsPatient))
                {
                    var caseConferenceService = new CaseConferenceService();
                    viewCaseConferences = caseConferenceService.MergeMissed(userSettingsPatient, viewCaseConferences, viewPatientCaseConferences.PatientId, WebSecurity.CurrentUserId);

                    if (viewCaseConferences.First().Status != ViewCaseConferenceStatus.Submitted
                        &&
                        !userSettingsPatient.DateUnassigned.HasValue)
                    {
                        viewPatientCaseConferences.IsCurrentFormPending = true;
                    }
                }

                viewPatientCaseConferences.ViewCaseConferences = viewCaseConferences;
            }

            var title = "Case Conferences";
            if (!string.IsNullOrEmpty(viewPatientCaseConferences.PatientName))
            {
                title = title + " for patient '" + viewPatientCaseConferences.PatientName + "'";
            }
            ViewBag.Title = title;

            return View("All", viewPatientCaseConferences);
        }

        [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.CaseConference_Edit)]
        [HttpGet]
        public ActionResult AllForPatient(int id, string patientName)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Patients;

            var viewPatientCaseConferences = new ViewPatientCaseConferences();
            viewPatientCaseConferences.PatientId = id;
            viewPatientCaseConferences.PatientName = patientName;
            using (var context = new DomainContext())
            {
                var patients = context.Patients.ToList();
                var viewPatients = Mapper.Map<List<ViewPatient>>(patients);
                viewPatientCaseConferences.ViewPatients = viewPatients;

                var caseConferences = context.CaseConferences
                    .Where(x => x.Patient.Id == id)
                    .Where(x => x.Status != CaseConferenceStatus.NotSubmitted)
                    .OrderBy(x => x.Date)
                    .ToList();
                var viewCaseConferences = Mapper.Map<List<ViewCaseConference>>(caseConferences);
                viewPatientCaseConferences.ViewCaseConferences = viewCaseConferences;
            }
            return View(viewPatientCaseConferences);
        }

        [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.CaseConference_Edit)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AllForPatientFilter(ViewPatientCaseConferences viewPatientCaseConferences)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Patients;

            using (var context = new DomainContext())
            {
                var patients = context.Patients.ToList();
                var viewPatients = Mapper.Map<List<ViewPatient>>(patients);
                viewPatientCaseConferences.ViewPatients = viewPatients;

                var caseConferences = context.CaseConferences
                    .Where(x => x.Patient.Id == viewPatientCaseConferences.PatientId)
                    .Where(x => x.Status != CaseConferenceStatus.NotSubmitted)
                    .OrderBy(x => x.Date)
                    .ToList();
                var viewCaseConferences = Mapper.Map<List<ViewCaseConference>>(caseConferences);
                viewPatientCaseConferences.ViewCaseConferences = viewCaseConferences;
            }
            return View("AllForPatient", viewPatientCaseConferences);
        }

        [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.CaseConference_Edit)]
        [HttpGet]
        public ActionResult AllForEmployee(int userId, string userName)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Users;

            var viewPatientCaseConferences = new ViewPatientCaseConferences();
            viewPatientCaseConferences.UserId = userId;
            viewPatientCaseConferences.UserName = userName;
            using (var context = new DomainContext())
            {
                var patients = context.UserSettingsPatient
                    .Where(x => x.UserSettings.MembershipUserId == userId)
                    .OrderByDescending(x => x.DateAssigned)
                    .Select(x => x.Patient)
                    .ToList();
                var viewPatients = Mapper.Map<List<ViewPatient>>(patients);
                viewPatientCaseConferences.ViewPatients = viewPatients;

                if (viewPatients.Count > 0)
                {
                    var firstViewPatient = viewPatients[0];
                    viewPatientCaseConferences.PatientId = firstViewPatient.Id;
                    viewPatientCaseConferences.PatientName = firstViewPatient.Name;

                    var userSettingsPatient = patients[0]
                        .UserSettingsPatients
                        .SingleOrDefault(x => x.UserSettings.MembershipUserId == viewPatientCaseConferences.UserId);

                    var caseConferences = context.CaseConferences
                        .Where(x => x.UserSettings.MembershipUserId == viewPatientCaseConferences.UserId)
                        .Where(x => x.Patient.Id == viewPatientCaseConferences.PatientId)
                        //.Where(x => x.Status != CaseConferenceStatus.NotSubmitted)
                        .OrderByDescending(x => x.Date)
                        .ToList();
                    var viewCaseConferences = Mapper.Map<List<ViewCaseConference>>(caseConferences);
                    var caseConferenceService = new CaseConferenceService();
                    viewCaseConferences = caseConferenceService.MergeMissed(userSettingsPatient, viewCaseConferences, viewPatientCaseConferences.PatientId, userId);
                    viewPatientCaseConferences.ViewCaseConferences = viewCaseConferences;
                }
                else
                {
                    ViewBag.StatusMessage = "There are no patients assigned.";
                }
            }

            var title = "Employee '" + viewPatientCaseConferences.UserName + "' > Case Conferences";
            if (!string.IsNullOrEmpty(viewPatientCaseConferences.PatientName))
            {
                title = title + " for patient '" + viewPatientCaseConferences.PatientName + "'";
            }
            ViewBag.Title = title;

            return View(viewPatientCaseConferences);
        }

        [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.CaseConference_Edit)]
        public ActionResult AllForEmployeeFilter(ViewPatientCaseConferences viewPatientCaseConferences, string message)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Users;
            ViewBag.StatusMessage = message;

            using (var context = new DomainContext())
            {
                var patients = context.UserSettingsPatient
                    .Where(x => x.UserSettings.MembershipUserId == viewPatientCaseConferences.UserId)
                    .OrderByDescending(x => x.DateAssigned)
                    .Select(x => x.Patient)
                    .ToList();
                var viewPatients = Mapper.Map<List<ViewPatient>>(patients);
                viewPatientCaseConferences.ViewPatients = viewPatients;

                var userSettingsPatient = patients
                    .Single(x => x.Id == viewPatientCaseConferences.PatientId)
                    .UserSettingsPatients
                    .SingleOrDefault(x => x.UserSettings.MembershipUserId == viewPatientCaseConferences.UserId);

                var caseConferences = context
                    .CaseConferences
                    .Where(x => x.UserSettings.MembershipUserId == viewPatientCaseConferences.UserId)
                    .Where(x => x.Patient.Id == viewPatientCaseConferences.PatientId)
                    //.Where(x => x.Status != CaseConferenceStatus.NotSubmitted)
                    .OrderByDescending(x => x.Date)
                    .ToList();
                var viewCaseConferences = Mapper.Map<List<ViewCaseConference>>(caseConferences);
                var caseConferenceService = new CaseConferenceService();
                viewCaseConferences = caseConferenceService.MergeMissed(userSettingsPatient, viewCaseConferences, viewPatientCaseConferences.PatientId, viewPatientCaseConferences.UserId);
                viewPatientCaseConferences.ViewCaseConferences = viewCaseConferences;
            }

            var title = "Employee '" + viewPatientCaseConferences.UserName + "' > Case Conferences";
            if (!string.IsNullOrEmpty(viewPatientCaseConferences.PatientName))
            {
                title = title + " for patient '" + viewPatientCaseConferences.PatientName + "'";
            }
            ViewBag.Title = title;

            return View("AllForEmployee", viewPatientCaseConferences);
        }



        [AuthorizeAnyRole(RoleNames.CaseConference_View)]
        [HttpGet]
        public ActionResult FillCurrent(int id)
        {
            var caseConferenceService = new CaseConferenceService();
            var date = caseConferenceService.GetSuccedingMonday(DateTime.Today);

            using (var context = new DomainContext())
            {
                var caseConference = context
                    .CaseConferences
                    .Where(x => x.Patient.Id == id)
                    .Where(x => x.Date == date)
                    .SingleOrDefault(x => x.UserSettings.MembershipUserId == WebSecurity.CurrentUserId);

                if (caseConference == default(CaseConference))
                {
                    return RedirectToAction("Create", new { Date = date, PatientId = id });
                }
                return RedirectToAction("Edit", new { Id = caseConference.Id });
            }
        }

        [AuthorizeAnyRole(RoleNames.CaseConference_View)]
        [HttpGet]
        public ActionResult Create(DateTime date, int patientId)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.CaseConferences;

            CreateCaseConference createCaseConference;
            using (var context = new DomainContext())
            {
                var templateCaseConference = context.TemplateCaseConferences.First();

                createCaseConference = Mapper.Map<CreateCaseConference>(templateCaseConference);
                createCaseConference.Questions = createCaseConference.Questions.OrderBy(x => x.Ordinal).ToList();
                createCaseConference.TemplateCaseConferenceId = templateCaseConference.Id;
            }

            createCaseConference.Date = date;
            createCaseConference.PatientId = patientId;

            using (var context = new DomainContext())
            {
                createCaseConference.PatientName = context.Patients.Single(x => x.Id == patientId).Name;
            }

            return View(createCaseConference);
        }

        [AuthorizeAnyRole(RoleNames.CaseConference_View)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateCaseConference createCaseConference)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.CaseConferences;

            try
            {
                using (var context = new DomainContext())
                {
                    var templateCaseConferenceQuestions = context
                        .TemplateCaseConferenceQuestions
                        .Where(x => x.TemplateCaseConference.Id == createCaseConference.TemplateCaseConferenceId)
                        .Include(x => x.TemplateCaseConferenceTextQuestion)
                        .Include(x => x.TemplateCaseConferenceDateQuestion)
                        .Include(x => x.TemplateCaseConferenceMultichoiceQuestion)
                        .Include(x => x.TemplateCaseConferenceMultichoiceQuestion.Answers)
                        .Include(x => x.TemplateCaseConferenceMultichoiceTextQuestion)
                        .Include(x => x.TemplateCaseConferenceMultichoiceTextQuestion.Answers)
                        .OrderBy(x => x.Ordinal);
                    var editCaseConferenceQuestions = Mapper.Map<List<EditCaseConferenceQuestion>>(templateCaseConferenceQuestions);
                    createCaseConference.Questions = editCaseConferenceQuestions;
                }

                CollectCaseConferenceQuestions(createCaseConference);
                if (!createCaseConference.Validate())
                {
                    ViewBag.Warning = "Case conference form is not valid. Please, check certification date is set and if all questions are answered.";
                    return View(createCaseConference);
                }
                SaveCaseConferenceQuestions(createCaseConference);

                return RedirectToAction("AllFilter",
                    new {
                        PatientId = createCaseConference.PatientId,
                        PatientName = createCaseConference.PatientName,
                        UserId = createCaseConference.UserId,
                        Message = "Case Conference form has been filled."
                    });
            }
            catch (Exception exception)
            {
                ViewBag.Warning = "Case conference form can not be saved. " + exception.Message;
            }
            return View(createCaseConference);
        }


        [AuthorizeAnyRole(RoleNames.CaseConference_View)]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.CaseConferences;

            EditCaseConference editCaseConference;
            using (var context = new DomainContext())
            {
                var caseConference = context.CaseConferences
                    .Include(x => x.Questions)
                    .Include(x => x.Patient)
                    .Single(x => x.Id == id);

                editCaseConference = Mapper.Map<EditCaseConference>(caseConference);
                editCaseConference.Questions = editCaseConference.Questions.OrderBy(x => x.Ordinal).ToList();
                editCaseConference.PatientId = caseConference.Patient.Id;
                editCaseConference.PatientName = caseConference.Patient.Name;
            }

            return View(editCaseConference);
        }

        [AuthorizeAnyRole(RoleNames.CaseConference_View)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditCaseConference editCaseConference)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.CaseConferences;

            var patientId = editCaseConference.PatientId;
            var patientName = editCaseConference.PatientName;
            var userId = editCaseConference.UserId;

            try
            {
                using (var context = new DomainContext())
                {
                    var caseConference = context.CaseConferences
                        .Include(x => x.Questions)
                        .Single(x => x.Id == editCaseConference.Id);

                    editCaseConference = Mapper.Map<EditCaseConference>(caseConference);
                    editCaseConference.Questions = editCaseConference.Questions.OrderBy(x => x.Ordinal).ToList();

                    CollectCaseConferenceQuestions(editCaseConference);
                    if (!editCaseConference.Validate())
                    {
                        ViewBag.Warning = "Case conference form is not valid. Please, check if all questions are answered.";
                        return View(editCaseConference);
                    }
                    Map(caseConference, editCaseConference);
                    context.SaveChanges();
                }

                return RedirectToAction("AllFilter",
                    new
                    {
                        PatientId = patientId,
                        PatientName = patientName,
                        UserId = userId,
                        Message = "Case Conference form has been saved."
                    });
            }
            catch (Exception exception)
            {
                ViewBag.Warning = "Case conference form can not be saved. " + exception.Message;
            }
            return View(editCaseConference);
        }


        [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.CaseConference_Edit)]
        [HttpGet]
        public ActionResult ViewForEmployee(int id)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Users;
            ViewBag.ReturnAction = "AllForEmployeeFilter";

            return View(id);
        }

        [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.CaseConference_Edit)]
        [HttpGet]
        public ActionResult ViewForPatient(int id)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Patients;
            ViewBag.ReturnAction = "AllForPatientFilter";

            return View(id);
        }

        [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.CaseConference_Edit)]
        [HttpGet]
        public ActionResult View(int id)
        {
            EditCaseConference editCaseConference;
            using (var context = new DomainContext())
            {
                var caseConference = context.CaseConferences
                    .Include(x => x.Questions)
                    .Single(x => x.Id == id);

                editCaseConference = Mapper.Map<EditCaseConference>(caseConference);
                editCaseConference.Questions = editCaseConference.Questions.OrderBy(x => x.Ordinal).ToList();
                editCaseConference.PatientId = caseConference.Patient.Id;
                editCaseConference.PatientName = caseConference.Patient.Name;
                editCaseConference.PatientInnerChhcId = caseConference.Patient.InnerChhcId;
                editCaseConference.UserId = caseConference.UserSettings.MembershipUserId;
            }
            using (var usersContext = new UsersContext())
            {
                editCaseConference.UserName = usersContext.UserProfiles.Single(x => x.UserId == editCaseConference.UserId).UserName;
            }

            return View("View", editCaseConference);
        }


        private void CollectCaseConferenceQuestions(CreateCaseConference createCaseConference)
        {
            CollectCaseConferenceQuestions(createCaseConference.Questions);
        }

        private void CollectCaseConferenceQuestions(EditCaseConference editCaseConference)
        {
            CollectCaseConferenceQuestions(editCaseConference.Questions);
        }

        private void CollectCaseConferenceQuestions(ICollection<EditCaseConferenceQuestion> editCaseConferenceQuestions)
        {
            var allKeys = Request.Form.AllKeys;
            var questionIds = Request.Form["QuestionIds"];
            var questionIdList = questionIds.Split(',');

            foreach (var questionIdString in questionIdList)
            {
                var questionId = int.Parse(questionIdString);

                var question = editCaseConferenceQuestions.SingleOrDefault(x => x.Id == questionId);
                if (question == default(EditCaseConferenceQuestion))
                {
                    continue;//exception
                }

                var questionTypeString = Request.Form["QuestionType" + questionId];
                var questionType = (TemplateCaseConferenceQuestionType)Enum.Parse(typeof(TemplateCaseConferenceQuestionType), questionTypeString);
                if (question.Type != questionType)
                {
                    continue;//exception
                }

                switch (questionType)
                {
                    case TemplateCaseConferenceQuestionType.Text:
                        string textQuestionAnswer = Request.Form["QuestionAnswer" + questionId];
                        question.TextQuestion.Answer = textQuestionAnswer;
                        break;
                    case TemplateCaseConferenceQuestionType.Date:
                        DateTime? dateQuestionAnswer = default(DateTime?);
                        DateTime date;
                        if (DateTime.TryParse(Request.Form["QuestionAnswer" + questionId], out date))
                        {
                            dateQuestionAnswer = date;
                        }
                        question.DateQuestion.Answer = dateQuestionAnswer;
                        break;
                    case TemplateCaseConferenceQuestionType.Multichoice:
                        question.MultichoiceQuestion.Answers.ForEach(x => x.IsChosen = false);

                        var regexMultichoiceQuestionAnswer = new Regex(@"Question" + questionId + @"Answer(?<answerId>\d*)IsCorrect");
                        foreach (var key in allKeys)
                        {
                            Match match = regexMultichoiceQuestionAnswer.Match(key);
                            if (!match.Success)
                            {
                                continue;
                            }

                            int answerId;
                            if (!int.TryParse(match.Groups["answerId"].Value, out answerId))
                            {
                                continue;
                            }
                            var answer = question.MultichoiceQuestion.Answers.SingleOrDefault(x => x.Id == answerId);
                            if (answer == default(EditCaseConferenceMultichoiceQuestionAnswer))
                            {
                                continue;
                            }

                            answer.IsChosen = true;
                        }
                        break;
                    case TemplateCaseConferenceQuestionType.MultichoiceText:
                        question.MultichoiceTextQuestion.Answers.ForEach(x => x.IsChosen = false);

                        string multichoiceTextQuestionAnswer = Request.Form["QuestionAnswer" + questionId];
                        question.MultichoiceTextQuestion.TextAnswer = multichoiceTextQuestionAnswer;

                        var regexMultichoiceTextQuestionAnswer = new Regex(@"Question" + questionId + @"Answer(?<answerId>\d*)IsCorrect");
                        foreach (var key in allKeys)
                        {
                            Match match = regexMultichoiceTextQuestionAnswer.Match(key);
                            if (!match.Success)
                            {
                                continue;
                            }

                            int answerId;
                            if (!int.TryParse(match.Groups["answerId"].Value, out answerId))
                            {
                                continue;
                            }
                            var answer = question.MultichoiceTextQuestion.Answers.SingleOrDefault(x => x.Id == answerId);
                            if (answer == default(EditCaseConferenceMultichoiceTextQuestionAnswer))
                            {
                                continue;
                            }

                            answer.IsChosen = true;
                        }
                        break;
                }
            }
        }

        private void SaveCaseConferenceQuestions(CreateCaseConference createCaseConference)
        {
            using (var context = new DomainContext())
            {
                var patient = new Patient { Id = createCaseConference.PatientId };
                context.Patients.Attach(patient);

                var userSettings = context.UserSettings.SingleOrDefault(x => x.MembershipUserId == WebSecurity.CurrentUserId);
                if (userSettings == default(UserSettings))
                {
                    userSettings = new UserSettings
                    {
                        MembershipUserId = WebSecurity.CurrentUserId
                    };
                    context.UserSettings.Add(userSettings);
                    context.Entry(userSettings).State = EntityState.Added;
                    context.SaveChanges();
                }

                var caseConference = new CaseConference
                {
                    Date = createCaseConference.Date,
                    CertificationDate = createCaseConference.CertificationDate ?? DateTime.Today,
                    Patient = patient,
                    UserSettings = userSettings,
                    Status = CaseConferenceStatus.NotSubmitted
                };

                foreach (var editCaseConferenceQuestion in createCaseConference.Questions)
                {
                    var caseConferenceQuestion = new CaseConferenceQuestion();
                    caseConferenceQuestion.Ordinal = editCaseConferenceQuestion.Ordinal;
                    caseConferenceQuestion.Type = editCaseConferenceQuestion.Type;
                    switch (editCaseConferenceQuestion.Type)
                    {
                        case TemplateCaseConferenceQuestionType.Text:
                            caseConferenceQuestion.CaseConferenceTextQuestion = new CaseConferenceTextQuestion
                            {
                                Text = editCaseConferenceQuestion.TextQuestion.Text,
                                Answer = editCaseConferenceQuestion.TextQuestion.Answer
                            };
                            break;
                        case TemplateCaseConferenceQuestionType.Date:
                            if (editCaseConferenceQuestion.DateQuestion.Answer.HasValue)
                            {
                                caseConferenceQuestion.CaseConferenceDateQuestion = new CaseConferenceDateQuestion
                                {
                                    Text = editCaseConferenceQuestion.DateQuestion.Text,
                                    Answer = editCaseConferenceQuestion.DateQuestion.Answer.Value
                                };
                            }
                            break;
                        case TemplateCaseConferenceQuestionType.Multichoice:
                            caseConferenceQuestion.CaseConferenceMultichoiceQuestion = new CaseConferenceMultichoiceQuestion
                            {
                                Text = editCaseConferenceQuestion.MultichoiceQuestion.Text
                            };
                            foreach (var answer in editCaseConferenceQuestion.MultichoiceQuestion.Answers)
                            {
                                var caseConferenceMultichoiceQuestionAnswer = new CaseConferenceMultichoiceQuestionAnswer
                                {
                                    Text = answer.Text,
                                    IsChosen = answer.IsChosen
                                };
                                caseConferenceQuestion.CaseConferenceMultichoiceQuestion.Answers.Add(caseConferenceMultichoiceQuestionAnswer);
                            }
                            break;
                        case TemplateCaseConferenceQuestionType.MultichoiceText:
                            caseConferenceQuestion.CaseConferenceMultichoiceTextQuestion = new CaseConferenceMultichoiceTextQuestion
                            {
                                Text = editCaseConferenceQuestion.MultichoiceTextQuestion.Text,
                                TextAnswer = editCaseConferenceQuestion.MultichoiceTextQuestion.TextAnswer
                            };
                            foreach (var answer in editCaseConferenceQuestion.MultichoiceTextQuestion.Answers)
                            {
                                var caseConferenceMultichoiceTextQuestionAnswer = new CaseConferenceMultichoiceTextQuestionAnswer
                                {
                                    Text = answer.Text,
                                    IsChosen = answer.IsChosen
                                };
                                caseConferenceQuestion.CaseConferenceMultichoiceTextQuestion.ChoiceAnswers.Add(caseConferenceMultichoiceTextQuestionAnswer);
                            }
                            break;
                    }
                    caseConference.Questions.Add(caseConferenceQuestion);
                }

                context.CaseConferences.Add(caseConference);
                context.Entry(caseConference).State = EntityState.Added;
                context.SaveChanges();
            }
        }

        private void Map(CaseConference caseConference, EditCaseConference editCaseConference)
        {
            caseConference.CertificationDate = editCaseConference.CertificationDate;

            foreach (var editCaseConferenceQuestion in editCaseConference.Questions)
            {
                var caseConferenceQuestion = caseConference.Questions.Single(x => x.Id == editCaseConferenceQuestion.Id);
                switch (editCaseConferenceQuestion.Type)
                {
                    case TemplateCaseConferenceQuestionType.Text:
                        caseConferenceQuestion.CaseConferenceTextQuestion.Answer = editCaseConferenceQuestion.TextQuestion.Answer;
                        break;
                    case TemplateCaseConferenceQuestionType.Date:
                        if (editCaseConferenceQuestion.DateQuestion.Answer.HasValue)
                        {
                            caseConferenceQuestion.CaseConferenceDateQuestion.Answer = editCaseConferenceQuestion.DateQuestion.Answer.Value;
                        }
                        break;
                    case TemplateCaseConferenceQuestionType.Multichoice:
                        foreach (var answer in editCaseConferenceQuestion.MultichoiceQuestion.Answers)
                        {
                            var caseConferenceMultichoiceQuestionAnswer = caseConferenceQuestion.CaseConferenceMultichoiceQuestion.Answers.Single(x => x.Id == answer.Id);
                            caseConferenceMultichoiceQuestionAnswer.IsChosen = answer.IsChosen;
                        }
                        break;
                    case TemplateCaseConferenceQuestionType.MultichoiceText:
                        caseConferenceQuestion.CaseConferenceMultichoiceTextQuestion.TextAnswer = editCaseConferenceQuestion.MultichoiceTextQuestion.TextAnswer;
                        foreach (var answer in editCaseConferenceQuestion.MultichoiceTextQuestion.Answers)
                        {
                            var caseConferenceMultichoiceTextQuestionAnswer = caseConferenceQuestion.CaseConferenceMultichoiceTextQuestion.ChoiceAnswers.Single(x => x.Id == answer.Id);
                            caseConferenceMultichoiceTextQuestionAnswer.IsChosen = answer.IsChosen;
                        }
                        break;
                }
            }
        }



        [AuthorizeAnyRole(RoleNames.CaseConference_View)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Submit(CaseConferenceFilter caseConferenceFilter)
        {
            DateTime date;
            using (var context = new DomainContext())
            {
                var caseConference = context.CaseConferences.Single(x => x.Id == caseConferenceFilter.Id);

                date = caseConference.Date;

                caseConference.Status = CaseConferenceStatus.Submitted;
                context.SaveChanges();
            }

            return RedirectToAction("AllFilter",
                new
                {
                    PatientId = caseConferenceFilter.PatientId,
                    PatientName = caseConferenceFilter.PatientName,
                    Message = "Case Conference for '" + date.ToString("MM/dd/yyyy") + "' has been submitted."
                });
        }

        [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.CaseConference_Edit)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForEmployee(CaseConferenceFilter caseConferenceFilter)
        {
            DateTime date;
            using (var context = new DomainContext())
            {
                var caseConference = context.CaseConferences.Single(x => x.Id == caseConferenceFilter.Id);

                date = caseConference.Date;

                caseConference.Status = CaseConferenceStatus.Submitted;
                context.SaveChanges();
            }

            return RedirectToAction("AllForEmployeeFilter",
                new
                {
                    UserId = caseConferenceFilter.UserId,
                    UserName = caseConferenceFilter.UserName,
                    PatientId = caseConferenceFilter.PatientId,
                    PatientName = caseConferenceFilter.PatientName,
                    Message = "Case Conference for '" + date.ToString("MM/dd/yyyy") + "' has been submitted."
                });
        }

        [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.CaseConference_Edit)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnsubmitForEmployee(CaseConferenceFilter caseConferenceFilter)
        {
            DateTime date;
            using (var context = new DomainContext())
            {
                var caseConference = context.CaseConferences.Single(x => x.Id == caseConferenceFilter.Id);

                date = caseConference.Date;

                caseConference.Status = CaseConferenceStatus.NotSubmitted;
                context.SaveChanges();
            }

            return RedirectToAction("AllForEmployeeFilter",
                new
                {
                    UserId = caseConferenceFilter.UserId,
                    UserName = caseConferenceFilter.UserName,
                    PatientId = caseConferenceFilter.PatientId,
                    PatientName = caseConferenceFilter.PatientName,
                    Message = "Case Conference for '" + date.ToString("MM/dd/yyyy") + "' has been unsubmitted."
                });
        }

        [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.CaseConference_Edit)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnsubmitForPatient(CaseConferenceFilter caseConferenceFilter)
        {
            DateTime date;
            using (var context = new DomainContext())
            {
                var caseConference = context.CaseConferences.Single(x => x.Id == caseConferenceFilter.Id);

                date = caseConference.Date;

                caseConference.Status = CaseConferenceStatus.NotSubmitted;
                context.SaveChanges();
            }

            return RedirectToAction("AllForPatientFilter",
                new
                {
                    PatientId = caseConferenceFilter.PatientId,
                    PatientName = caseConferenceFilter.PatientName,
                    Message = "Case Conference for '" + date.ToString("MM/dd/yyyy") + "' has been unsubmitted."
                });
        }

        [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.CaseConference_Edit)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AcceptForEmployee(AcceptVerballyCaseConference acceptVerballyCaseConference)
        {
            using (var context = new DomainContext())
            {
                if (acceptVerballyCaseConference.Id != 0)
                {
                    Delete(context, acceptVerballyCaseConference.Id);
                }

                var patient = new Patient { Id = acceptVerballyCaseConference.PatientId };
                context.Patients.Attach(patient);

                var userSettings = context.UserSettings.SingleOrDefault(x => x.MembershipUserId == acceptVerballyCaseConference.UserId);

                var caseConference = new CaseConference
                {
                    Date = acceptVerballyCaseConference.Date,
                    CertificationDate = acceptVerballyCaseConference.CertificationDate,
                    Patient = patient,
                    UserSettings = userSettings,
                    Status = CaseConferenceStatus.AcceptedVerbally
                };

                context.CaseConferences.Add(caseConference);
                context.Entry(caseConference).State = EntityState.Added;
                context.SaveChanges();
            }

            return RedirectToAction("AllForEmployeeFilter",
                new
                {
                    UserId = acceptVerballyCaseConference.UserId,
                    UserName = acceptVerballyCaseConference.UserName,
                    PatientId = acceptVerballyCaseConference.PatientId,
                    PatientName = acceptVerballyCaseConference.PatientName,
                    Message = "Case Conference has been accepted verbally."
                });
        }

        [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.CaseConference_Edit)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelAcceptanceForEmployee(CaseConferenceFilter caseConferenceFilter)
        {
            using (var context = new DomainContext())
            {
                var caseConference = new CaseConference { Id = caseConferenceFilter.Id };
                context.CaseConferences.Attach(caseConference);
                context.CaseConferences.Remove(caseConference);
                context.SaveChanges();
            }

            return RedirectToAction("AllForEmployeeFilter",
                new
                {
                    UserId = caseConferenceFilter.UserId,
                    UserName = caseConferenceFilter.UserName,
                    PatientId = caseConferenceFilter.PatientId,
                    PatientName = caseConferenceFilter.PatientName,
                    Message = "Case Conference verbal acceptance has been canceled."
                });
        }

        [AuthorizeAnyRole(RoleNames.CaseConference_View)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(CaseConferenceFilter caseConferenceFilter)
        {
            using (var context = new DomainContext())
            {
                Delete(context, caseConferenceFilter.Id);
            }

            return RedirectToAction("AllFilter",
                new
                {
                    PatientId = caseConferenceFilter.PatientId,
                    PatientName = caseConferenceFilter.PatientName,
                    Message = "Case Conference has been deleted."
                });
        }

        [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.CaseConference_Edit)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForEmployee(CaseConferenceFilter caseConferenceFilter)
        {
            using (var context = new DomainContext())
            {
                Delete(context, caseConferenceFilter.Id);
            }

            return RedirectToAction("AllForEmployeeFilter",
                new
                {
                    UserId = caseConferenceFilter.UserId,
                    UserName = caseConferenceFilter.UserName,
                    PatientId = caseConferenceFilter.PatientId,
                    PatientName = caseConferenceFilter.PatientName,
                    Message = "Case Conference has been deleted."
                });
        }

        private static void Delete(DomainContext context, int caseConferenceId)
        {
            var caseConferenceQuestions = context.CaseConferenceQuestions
                .Where(x => x.CaseConference.Id == caseConferenceId)
                .ToList();

            var caseConferenceTextQuestions = caseConferenceQuestions
                .Where(x => x.CaseConferenceTextQuestion != null)
                .Select(x => x.CaseConferenceTextQuestion)
                .ToList();
            foreach (var caseConferenceTextQuestion in caseConferenceTextQuestions)
            {
                context.CaseConferenceTextQuestions.Attach(caseConferenceTextQuestion);
                context.CaseConferenceTextQuestions.Remove(caseConferenceTextQuestion);
            }

            var caseConferenceDateQuestions = caseConferenceQuestions
                .Where(x => x.CaseConferenceDateQuestion != null)
                .Select(x => x.CaseConferenceDateQuestion)
                .ToList();
            foreach (var caseConferenceDateQuestion in caseConferenceDateQuestions)
            {
                context.CaseConferenceDateQuestions.Attach(caseConferenceDateQuestion);
                context.CaseConferenceDateQuestions.Remove(caseConferenceDateQuestion);
            }

            var caseConferenceMultichoiceQuestions = caseConferenceQuestions
                .Where(x => x.CaseConferenceMultichoiceQuestion != null)
                .Select(x => x.CaseConferenceMultichoiceQuestion)
                .ToList();
            foreach (var caseConferenceMultichoiceQuestion in caseConferenceMultichoiceQuestions)
            {
                context.CaseConferenceMultichoiceQuestions.Attach(caseConferenceMultichoiceQuestion);
                context.CaseConferenceMultichoiceQuestions.Remove(caseConferenceMultichoiceQuestion);
            }

            var caseConferenceMultichoiceTextQuestions = caseConferenceQuestions
                .Where(x => x.CaseConferenceMultichoiceTextQuestion != null)
                .Select(x => x.CaseConferenceMultichoiceTextQuestion)
                .ToList();
            foreach (var caseConferenceMultichoiceTextQuestion in caseConferenceMultichoiceTextQuestions)
            {
                context.CaseConferenceMultichoiceTextQuestions.Attach(caseConferenceMultichoiceTextQuestion);
                context.CaseConferenceMultichoiceTextQuestions.Remove(caseConferenceMultichoiceTextQuestion);
            }

            var caseConference = new CaseConference { Id = caseConferenceId };
            context.CaseConferences.Attach(caseConference);
            context.CaseConferences.Remove(caseConference);

            context.SaveChanges();
        }
    }
}