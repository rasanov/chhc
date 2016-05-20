using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CHHC.DomainModel
{
    public class Patient
    {
        public int Id { get; set; }
        public int InnerChhcId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UserSettingsPatient> UserSettingsPatients { get; private set; }

        public Patient()
        {
            UserSettingsPatients = new Collection<UserSettingsPatient>();
        }
    }
}