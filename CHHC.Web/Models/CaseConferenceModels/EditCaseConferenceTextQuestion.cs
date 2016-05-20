namespace CHHC.Web.Models
{
    public class EditCaseConferenceTextQuestion
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Answer { get; set; }

        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Answer))
            {
                return false;
            }
            return true;
        }
    }
}