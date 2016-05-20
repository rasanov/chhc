using System;
using System.Collections.Generic;

namespace CHHC.Web.Models
{
    public class DocumentUser
    {
        public UserProfile UserProfile { get; set; }
        public bool HasDownloaded { get; set; }
    }
}