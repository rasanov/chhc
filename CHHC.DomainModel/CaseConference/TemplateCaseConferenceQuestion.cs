namespace CHHC.DomainModel
{
    public class TemplateCaseConferenceQuestion
    {
        public int Id { get; set; }
        public int Ordinal { get; set; }
        public TemplateCaseConference TemplateCaseConference { get; set; }
        public TemplateCaseConferenceQuestionType Type { get; set; }
        public virtual TemplateCaseConferenceTextQuestion TemplateCaseConferenceTextQuestion { get; set; }
        public virtual TemplateCaseConferenceDateQuestion TemplateCaseConferenceDateQuestion { get; set; }
        public virtual TemplateCaseConferenceMultichoiceQuestion TemplateCaseConferenceMultichoiceQuestion { get; set; }
        public virtual TemplateCaseConferenceMultichoiceTextQuestion TemplateCaseConferenceMultichoiceTextQuestion { get; set; }
    }
}