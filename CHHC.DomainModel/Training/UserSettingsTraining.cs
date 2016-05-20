using System;

namespace CHHC.DomainModel
{
    public class UserSettingsTraining
    {
        public int Id { get; set; }
        public virtual Training Training { get; set; }
        public virtual UserSettings UserSettings { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedDate { get; set; }
    }
}