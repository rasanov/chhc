using System.Collections.Generic;
using System.Linq;

namespace CHHC.Web.Models
{
    public class ViewTemplateCaseConferenceMultichoiceQuestion
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public List<ViewTemplateCaseConferenceMultichoiceQuestionAnswer> Answers { get; private set; }

        public ViewTemplateCaseConferenceMultichoiceQuestion()
        {
            Answers = new List<ViewTemplateCaseConferenceMultichoiceQuestionAnswer>();
        }

        public bool Validate()
        {
            if (Answers.Count == 0)
            {
                return false;
            }
            if (Answers.All(x => string.IsNullOrWhiteSpace(x.Text)))
            {
                return false;
            }
            return true;
        }
    }
}