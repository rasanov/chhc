using System;

namespace CHHC.DomainModel
{
    public class UserSettingsPatient
    {
        public int Id { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual UserSettings UserSettings { get; set; }
        public DateTime DateAssigned { get; set; }
        public DateTime? DateUnassigned { get; set; }
    }
}