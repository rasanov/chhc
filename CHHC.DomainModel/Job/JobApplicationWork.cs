using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CHHC.DomainModel
{
    public class JobApplicationWork
    {
        public int Id { get; set; }
        public JobApplication JobApplication { get; set; }
        public string JobTitle { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string Responsibilities { get; set; }
        public string EmployerName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Salary { get; set; }
        public string SupervisorName { get; set; }
        public string ReasonForLeaving { get; set; }
    }
}