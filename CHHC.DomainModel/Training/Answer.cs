namespace CHHC.DomainModel
{
    public class Answer
    {
        public int Id { get; set; }
        public virtual MultichoiceQuestion MultichoiceQuestion { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
