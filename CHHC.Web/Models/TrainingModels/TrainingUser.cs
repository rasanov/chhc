using System;
using System.Collections.Generic;

namespace CHHC.Web.Models
{
    public class TrainingUser
    {
        public UserProfile UserProfile { get; set; }
        public bool HasCompleted { get; set; }
        public DateTime? CompletedDate { get; set; }
    }
}