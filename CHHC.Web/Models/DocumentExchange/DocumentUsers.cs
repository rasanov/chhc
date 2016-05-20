using System.Collections.Generic;

namespace CHHC.Web.Models
{
    public class DocumentUsers
    {
        public int DocumentId { get; set; }
        public string DocumentFileName { get; set; }
        public List<int> UserIds { get; set; }
        public IEnumerable<DocumentUser> AllUsers { get; set; }
    }
}