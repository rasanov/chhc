using CHHC.DomainModel;
using CHHC.Web.Models;
using System;
using System.Collections.Generic;

namespace CHHC.Web.Services
{
    public class CaseConferenceService
    {
        public List<ViewCaseConference> MergeMissed(UserSettingsPatient userSettingsPatient, List<ViewCaseConference> viewCaseConferences, int patientId, int userId)
        {
            if (userSettingsPatient == default(UserSettingsPatient))
            {
                return new List<ViewCaseConference>();
            }

            var mergedViewCaseConferences = new List<ViewCaseConference>();
            int i = 0;
            var dateFrom = GetSuccedingMonday(userSettingsPatient.DateAssigned).Date;
            var dateTo = GetSuccedingMonday(userSettingsPatient.DateUnassigned ?? DateTime.Now).Date;
            while (dateTo >= dateFrom)
            {
                if (viewCaseConferences.Count <= i || dateTo > viewCaseConferences[i].Date)
                {
                    var missedViewCaseConference = new ViewCaseConference
                    {
                        Id = -1,
                        Date = dateTo,
                        Status = ViewCaseConferenceStatus.Missed
                    };
                    mergedViewCaseConferences.Add(missedViewCaseConference);
                }
                else
                {
                    mergedViewCaseConferences.Add(viewCaseConferences[i]);
                    i++;
                }
                dateTo = dateTo.AddDays(-7);
            }

            return mergedViewCaseConferences;
        }
        public int CountMissed(UserSettingsPatient userSettingsPatient, List<ViewCaseConference> viewCaseConferences, int patientId, int userId)
        {
            if (userSettingsPatient == default(UserSettingsPatient))
            {
                return 0;
            }

            int missedCount = 0;
            int i = 0;
            var dateFrom = GetSuccedingMonday(userSettingsPatient.DateAssigned).Date;
            var dateTo = GetSuccedingMonday(userSettingsPatient.DateUnassigned ?? DateTime.Now).Date;
            while (dateTo >= dateFrom)
            {
                if (viewCaseConferences.Count <= i || dateTo > viewCaseConferences[i].Date)
                {
                    missedCount++;
                }
                else
                {
                    i++;
                }
                dateTo = dateTo.AddDays(-7);
            }

            return missedCount;
        }
        public DateTime GetPrecedingMonday(DateTime date)
        {
            while (date.DayOfWeek != DayOfWeek.Monday)
            {
                date = date.AddDays(-1);
            }
            return date;
        }
        public DateTime GetSuccedingMonday(DateTime date)
        {
            while (date.DayOfWeek != DayOfWeek.Monday)
            {
                date = date.AddDays(1);
            }
            return date;
        }
    }
}