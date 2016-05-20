using System.Collections.Generic;

namespace CHHC.DomainModel
{
    public class CaseConferenceMultichoiceQuestion
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public virtual List<CaseConferenceMultichoiceQuestionAnswer> Answers { get; private set; }

        public CaseConferenceMultichoiceQuestion()
        {
            Answers = new List<CaseConferenceMultichoiceQuestionAnswer>();
        }
    }
}
