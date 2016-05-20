using System.Collections.Generic;

namespace CHHC.Web.Models
{
    public class CaseConferenceFilter
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
    }
}