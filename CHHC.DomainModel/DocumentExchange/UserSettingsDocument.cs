namespace CHHC.DomainModel
{
    public class UserSettingsDocument
    {
        public int Id { get; set; }
        public virtual Document Document { get; set; }
        public virtual UserSettings UserSettings { get; set; }
        public bool IsDownloaded { get; set; }
    }
}