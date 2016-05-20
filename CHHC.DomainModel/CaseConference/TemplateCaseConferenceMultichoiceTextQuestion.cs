using System.Collections.Generic;

namespace CHHC.DomainModel
{
    public class TemplateCaseConferenceMultichoiceTextQuestion
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public virtual List<TemplateCaseConferenceMultichoiceTextQuestionAnswer> Answers { get; private set; }

        public TemplateCaseConferenceMultichoiceTextQuestion()
        {
            Answers = new List<TemplateCaseConferenceMultichoiceTextQuestionAnswer>();
        }
    }
}
