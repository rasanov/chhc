using System.Collections.Generic;
using System.Linq;

namespace CHHC.Web.Models
{
    public class Quiz
    {
        public int TrainingId { get; set; }
        public string TrainingTitle { get; set; }
        public List<EditMultichoiceQuestion> Questions { get; set; }

        public Quiz()
        {
            Questions = new List<EditMultichoiceQuestion>();
        }

        public bool Validate()
        {
            if (Questions.Any(x => !x.Validate()))
            {
                return false;
            }
            return true;
        }
    }
}