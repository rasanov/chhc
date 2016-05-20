namespace CHHC.Web.Models
{
    public class PublicAnswer
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public bool IsChosenAsCorrect { get; set; }
    }
}