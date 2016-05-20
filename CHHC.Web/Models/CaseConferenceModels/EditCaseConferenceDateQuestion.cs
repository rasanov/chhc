using System;
namespace CHHC.Web.Models
{
    public class EditCaseConferenceDateQuestion
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime? Answer { get; set; }

        public bool Validate()
        {
            if (Answer == default(DateTime?))
            {
                return false;
            }
            return true;
        }
    }
}
