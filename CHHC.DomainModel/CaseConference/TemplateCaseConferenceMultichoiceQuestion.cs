using System.Collections.Generic;

namespace CHHC.DomainModel
{
    public class TemplateCaseConferenceMultichoiceQuestion
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public virtual List<TemplateCaseConferenceMultichoiceQuestionAnswer> Answers { get; private set; }

        public TemplateCaseConferenceMultichoiceQuestion()
        {
            Answers = new List<TemplateCaseConferenceMultichoiceQuestionAnswer>();
        }
    }
}
