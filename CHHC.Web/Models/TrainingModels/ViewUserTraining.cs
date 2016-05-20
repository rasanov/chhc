using System;

namespace CHHC.Web.Models
{
    public class ViewUserTraining
    {
        public int TrainingId { get; set; }
        public string TrainingTitle { get; set; }
        public int TrainingDurationMinutes { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedDate { get; set; }
    }
}