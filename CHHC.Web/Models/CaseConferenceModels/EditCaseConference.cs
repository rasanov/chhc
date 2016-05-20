using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CHHC.Web.Models
{
    public class EditCaseConference
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime CertificationDate { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public int PatientInnerChhcId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public ICollection<EditCaseConferenceQuestion> Questions { get;  set; }

        public EditCaseConference()
        {
            Questions = new Collection<EditCaseConferenceQuestion>();
        }

        public bool Validate()
        {
            if (Questions.Any(x => !x.Validate()))
            {
                return false;
            }
            return true;
        }
    }
}