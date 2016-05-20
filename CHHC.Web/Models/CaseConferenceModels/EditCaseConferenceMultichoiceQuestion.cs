using System.Collections.Generic;
using System.Linq;

namespace CHHC.Web.Models
{
    public class EditCaseConferenceMultichoiceQuestion
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public List<EditCaseConferenceMultichoiceQuestionAnswer> Answers { get; private set; }

        public EditCaseConferenceMultichoiceQuestion()
        {
            Answers = new List<EditCaseConferenceMultichoiceQuestionAnswer>();
        }

        public bool Validate()
        {
            if (Answers.All(x => !x.IsChosen))
            {
                return false;
            }
            return true;
        }
    }
}