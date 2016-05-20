using System.Collections.Generic;
using System.Linq;

namespace CHHC.Web.Models
{
    public class PublicQuiz
    {
        private const double Threshold = 0.8;
        public int TrainingId { get; set; }
        public string TrainingTitle { get; set; }
        public List<PublicMultichoiceQuestion> Questions { get; set; }

        public int GetAnsweredCorrectlyCount()
        {
            return Questions.Count(x => (x.AnsweredCorrectly.HasValue && x.AnsweredCorrectly.Value));
        }

        public bool IsPassed()
        {
            if (Questions.Count == 0)
            {
                return false;
            }
            if (GetAnsweredCorrectlyCount() / Questions.Count >= Threshold)
            {
                return true;
            }
            return false;
        }

        public PublicQuiz()
        {
            Questions = new List<PublicMultichoiceQuestion>();
        }
    }
}