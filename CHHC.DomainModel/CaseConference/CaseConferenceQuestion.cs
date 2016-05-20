namespace CHHC.DomainModel
{
    public class CaseConferenceQuestion
    {
        public int Id { get; set; }
        public int Ordinal { get; set; }
        public CaseConference CaseConference { get; set; }
        public TemplateCaseConferenceQuestionType Type { get; set; }
        public virtual CaseConferenceTextQuestion CaseConferenceTextQuestion { get; set; }
        public virtual CaseConferenceDateQuestion CaseConferenceDateQuestion { get; set; }
        public virtual CaseConferenceMultichoiceQuestion CaseConferenceMultichoiceQuestion { get; set; }
        public virtual CaseConferenceMultichoiceTextQuestion CaseConferenceMultichoiceTextQuestion { get; set; }
    }
}