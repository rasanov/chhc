using System.Collections.Generic;

namespace CHHC.Web.Models
{
    public class ViewDocuments
    {
        public List<ViewDocument> DocumentsAssignedToCurrentUser { get; set; }
        public List<ViewDocument> DocumentsUploadedByCurrentUser { get; set; }
    }
}