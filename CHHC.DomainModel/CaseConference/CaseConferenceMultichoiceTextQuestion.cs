using System.Collections.Generic;

namespace CHHC.DomainModel
{
    public class CaseConferenceMultichoiceTextQuestion
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string TextAnswer { get; set; }
        public virtual List<CaseConferenceMultichoiceTextQuestionAnswer> ChoiceAnswers { get; private set; }

        public CaseConferenceMultichoiceTextQuestion()
        {
            ChoiceAnswers = new List<CaseConferenceMultichoiceTextQuestionAnswer>();
        }
    }
}
