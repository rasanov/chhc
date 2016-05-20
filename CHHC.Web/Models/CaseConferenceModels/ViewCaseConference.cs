using System;

namespace CHHC.Web.Models
{
    public class ViewCaseConference
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public ViewCaseConferenceStatus Status { get; set; }
    }
}