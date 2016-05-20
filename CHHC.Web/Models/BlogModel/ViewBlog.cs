using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHHC.Web.Models
{
    public class ViewBlog
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public DateTime Date { get; set; }

        [AllowHtml]
        [Required]
        public string Text { get; set; }
    }
}