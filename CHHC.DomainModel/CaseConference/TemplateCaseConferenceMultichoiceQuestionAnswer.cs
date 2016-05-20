namespace CHHC.DomainModel
{
    public class TemplateCaseConferenceMultichoiceQuestionAnswer
    {
        public int Id { get; set; }
        public virtual TemplateCaseConferenceMultichoiceQuestion TemplateCaseConferenceMultichoiceQuestion { get; set; }
        public string Text { get; set; }
    }
}
