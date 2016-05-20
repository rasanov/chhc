namespace CHHC.DomainModel
{
    public class TemplateCaseConferenceMultichoiceTextQuestionAnswer
    {
        public int Id { get; set; }
        public virtual TemplateCaseConferenceMultichoiceTextQuestion TemplateCaseConferenceMultichoiceQuestion { get; set; }
        public string Text { get; set; }
    }
}
