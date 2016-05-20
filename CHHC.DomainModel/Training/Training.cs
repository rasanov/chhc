using System.Collections.Generic;

namespace CHHC.DomainModel
{
    public class Training
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int DurationMinutes { get; set; }
        public string InstructorName { get; set; }
        public string InstructorTitle { get; set; }
        public virtual ICollection<TrainingMedia> Media { get; private set; }
        public virtual ICollection<MultichoiceQuestion> Quiz { get; private set; }
        public virtual ICollection<UserSettingsTraining> UserSettingsTrainings { get; private set; }

        public Training()
        {
            Media = new HashSet<TrainingMedia>();
            Quiz = new HashSet<MultichoiceQuestion>();
            UserSettingsTrainings = new HashSet<UserSettingsTraining>();
        }
    }
}