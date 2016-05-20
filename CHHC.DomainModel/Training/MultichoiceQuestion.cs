using System.Collections.Generic;

namespace CHHC.DomainModel
{
    public class MultichoiceQuestion
    {
        public int Id { get; set; }
        public virtual Training Training { get; set; }
        public string Question { get; set; }
        public virtual ICollection<Answer> Answers { get; private set; }

        public MultichoiceQuestion()
        {
            Answers = new HashSet<Answer>();
        }
    }
}