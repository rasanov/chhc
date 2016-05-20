using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CHHC.DomainModel;

namespace CHHC.Web.Models
{
    public class ViewTemplateCaseConference
    {
        public int Id { get; set; }
        public ICollection<ViewTemplateCaseConferenceQuestion> Questions { get;  set; }

        public ViewTemplateCaseConference()
        {
            Questions = new Collection<ViewTemplateCaseConferenceQuestion>();
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