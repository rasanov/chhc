using System;

namespace CHHC.Web.Models
{
    public class AcceptVerballyCaseConference
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public DateTime Date { get; set; }
        public DateTime CertificationDate { get; set; }
    }
}