using System.Collections.Generic;

namespace CHHC.Web.Models
{
    public class PublicMultichoiceQuestion
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public List<PublicAnswer> Answers { get; set; }
        public bool? AnsweredCorrectly { get; set; }

        public PublicMultichoiceQuestion()
        {
            Answers = new List<PublicAnswer>();
        }
    }
}