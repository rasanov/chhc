using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CHHC.Web.Models.JobModels
{
    public class ViewJobApplicationWork
    {
        public int Id { get; set; }

        [DisplayName("Job Title")]
        public string JobTitle { get; set; }

        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string Responsibilities { get; set; }

        [DisplayName("Employer Name")]
        public string EmployerName { get; set; }

        public string Address { get; set; }
        public string Phone { get; set; }
        public string Salary { get; set; }

        [DisplayName("Supervisor Name")]
        public string SupervisorName { get; set; }

        [DisplayName("Reason For Leaving")]
        public string ReasonForLeaving { get; set; }
    }
}