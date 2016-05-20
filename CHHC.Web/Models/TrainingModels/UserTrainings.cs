using System.Collections.Generic;

namespace CHHC.Web.Models
{
    public class UserTrainings
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<int> TrainingIds { get; set; }
        public IEnumerable<UserTraining> AllTrainings { get; set; }
        public string ReturnControllerName { get; set; }
    }
}