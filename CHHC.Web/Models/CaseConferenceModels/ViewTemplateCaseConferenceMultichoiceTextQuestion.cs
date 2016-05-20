using System.Collections.Generic;
using System.Linq;

namespace CHHC.Web.Models
{
    public class ViewTemplateCaseConferenceMultichoiceTextQuestion
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public List<ViewTemplateCaseConferenceMultichoiceTextQuestionAnswer> Answers { get; private set; }

        public ViewTemplateCaseConferenceMultichoiceTextQuestion()
        {
            Answers = new List<ViewTemplateCaseConferenceMultichoiceTextQuestionAnswer>();
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