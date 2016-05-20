using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CHHC.DomainModel
{
    public class CaseConference
    {
        public int Id { get; set; }
        public virtual UserSettings UserSettings { get; set; }
        public DateTime Date { get; set; }
        public DateTime CertificationDate { get; set; }
        public virtual Patient Patient { get; set; }
        public CaseConferenceStatus Status { get; set; }
        public virtual ICollection<CaseConferenceQuestion> Questions { get; private set; }

        public CaseConference()
        {
            Questions = new Collection<CaseConferenceQuestion>();
        }
    }
}