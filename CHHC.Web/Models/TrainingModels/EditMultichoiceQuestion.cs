using System.Collections.Generic;
using System.Linq;

namespace CHHC.Web.Models
{
    public class EditMultichoiceQuestion
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public List<EditAnswer> Answers { get; set; }

        public EditMultichoiceQuestion()
        {
            Answers = new List<EditAnswer>();
        }

        public bool Validate()
        {
            if (Answers.Count == 0)
            {
                return false;
            }
            if (!Answers.Any(x => x.IsCorrect))
            {
                return false;
            }
            return true;
        }
    }
}