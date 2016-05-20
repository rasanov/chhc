using System.Collections.Generic;

namespace CHHC.Web.Models
{
    public class TrainingUsers
    {
        public int TrainingId { get; set; }
        public string TrainingTitle { get; set; }
        public List<int> UserIds { get; set; }
        public IEnumerable<TrainingUser> AllUsers { get; set; }
    }
}