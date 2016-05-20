using System.Collections.Generic;

namespace CHHC.DomainModel
{
    public class UserSettings
    {
        public int Id { get; set; }
        public int MembershipUserId { get; set; }
        public virtual ICollection<UserSettingsTraining> UserSettingsTrainings { get; private set; }
        public virtual ICollection<UserSettingsPatient> UserSettingsPatients { get; private set; }
        public virtual ICollection<UserSettingsDocument> UserSettingsDocuments { get; private set; }

        public UserSettings()
        {
            UserSettingsTrainings = new HashSet<UserSettingsTraining>();
            UserSettingsPatients = new HashSet<UserSettingsPatient>();
            UserSettingsDocuments = new HashSet<UserSettingsDocument>();
        }
    }
}