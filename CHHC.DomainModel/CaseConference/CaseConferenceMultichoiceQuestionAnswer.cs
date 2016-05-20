namespace CHHC.DomainModel
{
    public class CaseConferenceMultichoiceQuestionAnswer
    {
        public int Id { get; set; }
        public virtual CaseConferenceMultichoiceQuestion CaseConferenceMultichoiceQuestion { get; set; }
        public string Text { get; set; }
        public bool IsChosen { get; set; }
    }
}
