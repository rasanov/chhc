using System.Collections.Generic;

namespace CHHC.Web.Models
{
    public class UserPatients
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<int> PatientIds { get; set; }
        public IEnumerable<ViewPatient> AllPatients { get; set; }
        public string ReturnControllerName { get; set; }
    }
}