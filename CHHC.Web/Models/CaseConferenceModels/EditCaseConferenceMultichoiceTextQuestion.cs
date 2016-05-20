using System.Collections.Generic;
using System.Linq;

namespace CHHC.Web.Models
{
    public class EditCaseConferenceMultichoiceTextQuestion
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string TextAnswer { get; set; }
        public List<EditCaseConferenceMultichoiceTextQuestionAnswer> Answers { get; private set; }

        public EditCaseConferenceMultichoiceTextQuestion()
        {
            Answers = new List<EditCaseConferenceMultichoiceTextQuestionAnswer>();
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