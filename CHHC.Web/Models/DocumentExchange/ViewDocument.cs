using System;

namespace CHHC.Web.Models
{
    public class ViewDocument
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public DateTime DateUploaded { get; set; }
        public int UploadedByMembershipUserId { get; set; }
        public string UploadedByMembershipUserName { get; set; }
        public ViewDocumentStatus Status { get; set; }
    }
}