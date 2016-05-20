using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CHHC.Web.Models
{
    public class CreateCaseConference
    {
        public int TemplateCaseConferenceId { get; set; }
        public DateTime Date { get; set; }
        public DateTime? CertificationDate { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public int UserId { get; set; }
        public ICollection<EditCaseConferenceQuestion> Questions { get;  set; }

        public CreateCaseConference()
        {
            Questions = new Collection<EditCaseConferenceQuestion>();
        }

        public bool Validate()
        {
            if (!CertificationDate.HasValue)
            {
                return false;
            }
            if (Questions.Any(x => !x.Validate()))
            {
                return false;
            }
            return true;
        }
    }
}