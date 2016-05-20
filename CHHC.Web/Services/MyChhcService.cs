using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using CHHC.DAL.Infrastructure;
using CHHC.DomainModel;
using CHHC.Web.Models;
using WebMatrix.WebData;

namespace CHHC.Web.Services
{
    public class MyChhcService
    {
        public int GetNotCompletedTrainingsCount()
        {
            int notCompletedTrainingsCount;
            using (var context = new DomainContext())
            {
                notCompletedTrainingsCount = context.UserSettingsTraining
                    .Count(x =>
                        x.UserSettings.MembershipUserId == WebSecurity.CurrentUserId
                        && !x.IsCompleted);
            }
            return notCompletedTrainingsCount;
        }

        public int GetNotFilledCaseConferencesCount()
        {
            int notFilledCaseConferencesCount = 0;
            using (var context = new DomainContext())
            {
                var userSettingsPatients = context.UserSettingsPatient
                    .Where(x => x.UserSettings.MembershipUserId == WebSecurity.CurrentUserId)
                    .Include(x => x.Patient)
                    .ToList();
                var caseConferenceService = new CaseConferenceService();
                foreach (var userSettingsPatient in userSettingsPatients)
                {
                    var caseConferences = context.CaseConferences
                        .Where(x => x.UserSettings.MembershipUserId == WebSecurity.CurrentUserId)
                        .Where(x => x.Patient.Id == userSettingsPatient.Patient.Id)
                        .Where(x => x.Status != CaseConferenceStatus.NotSubmitted)
                        .OrderByDescending(x => x.Date)
                        .ToList();
                    var viewCaseConferences = Mapper.Map<List<ViewCaseConference>>(caseConferences);

                    notFilledCaseConferencesCount += caseConferenceService.CountMissed(userSettingsPatient, viewCaseConferences, userSettingsPatient.Patient.Id, WebSecurity.CurrentUserId);
                }
            }
            return notFilledCaseConferencesCount;
        }

        public int GetDocumentsCount(bool isUserInRoleDocumentViewAll)
        {
            int documentsCount;
            using (var context = new DomainContext())
            {
                documentsCount = context.UserSettingsDocuments
                    .Where(x =>
                        x.UserSettings.MembershipUserId == WebSecurity.CurrentUserId
                        || (x.Document.Status == DocumentStatus.AssignedToAll && isUserInRoleDocumentViewAll)
                    )
                    .Where(x => !x.IsDownloaded)
                    .Count();
            }
            return documentsCount;
        }
    }
}