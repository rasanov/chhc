using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CHHC.Web.Models.JobModels
{
    public class ViewJobApplicationEducation
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        [DisplayName("Courses of study")]
        public string MainCourse { get; set; }

        [DisplayName("Degree/Diploma")]
        public string Degree { get; set; }
    }
}