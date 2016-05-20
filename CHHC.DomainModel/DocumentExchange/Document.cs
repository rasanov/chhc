using System;
using System.Collections.Generic;

namespace CHHC.DomainModel
{
    public class Document
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ContentType { get; set; }
        public DateTime DateUploaded { get; set; }
        public DocumentStatus Status { get; set; }
        public virtual UserSettings UploadedBy { get; set; }
        public virtual ICollection<UserSettingsDocument> AssignedTo { get; private set; }

        public Document()
        {
            AssignedTo = new HashSet<UserSettingsDocument>();
        }
    }
}