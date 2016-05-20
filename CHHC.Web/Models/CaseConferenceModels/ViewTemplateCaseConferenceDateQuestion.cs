namespace CHHC.Web.Models
{
    public class ViewTemplateCaseConferenceDateQuestion
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                return false;
            }
            return true;
        }
    }
}
