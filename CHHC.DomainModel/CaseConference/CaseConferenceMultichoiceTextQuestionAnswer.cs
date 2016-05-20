namespace CHHC.DomainModel
{
    public class CaseConferenceMultichoiceTextQuestionAnswer
    {
        public int Id { get; set; }
        public virtual CaseConferenceMultichoiceTextQuestion CaseConferenceMultichoiceQuestion { get; set; }
        public string Text { get; set; }
        public bool IsChosen { get; set; }
    }
}
