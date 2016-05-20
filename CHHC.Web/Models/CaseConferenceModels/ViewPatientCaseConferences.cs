using System.Collections.Generic;

namespace CHHC.Web.Models
{
    public class ViewPatientCaseConferences
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public bool IsCurrentFormPending { get; set; }
        public List<ViewCaseConference> ViewCaseConferences { get; set; }
        public List<ViewPatient> ViewPatients { get; set; }
    }
}