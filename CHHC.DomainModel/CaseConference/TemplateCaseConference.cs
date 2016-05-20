using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CHHC.DomainModel
{
    public class TemplateCaseConference
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual ICollection<TemplateCaseConferenceQuestion> Questions { get; private set; }

        public TemplateCaseConference()
        {
            Questions = new Collection<TemplateCaseConferenceQuestion>();
        }
    }
}