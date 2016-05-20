using System.Collections.Generic;

namespace CHHC.Web.Models
{
    public class PatientUsers
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public List<int> UserIds { get; set; }
        public IEnumerable<UserProfile> AllUsers { get; set; }
    }
}