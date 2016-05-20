using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CHHC.DomainModel
{
    public class JobApplicationEducation
    {
        public int Id { get; set; }
        public JobApplication JobApplication { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string MainCourse { get; set; }
        public string Degree { get; set; }
    }
}