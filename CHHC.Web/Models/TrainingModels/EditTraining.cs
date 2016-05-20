using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CHHC.Web.Models
{
    public class EditTraining
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Subject")]
        public string Title { get; set; }

        [AllowHtml]
        public string Text { get; set; }

        [Required]
        [Display(Name = "Duration (minutes)")]
        public int DurationMinutes { get; set; }

        [Display(Name = "Instructor Name")]
        public string InstructorName { get; set; }

        [Display(Name = "Instructor Title")]
        public string InstructorTitle { get; set; }

        public List<string> YoutubeMediaTitles { get; set; }
        public List<string> YoutubeMediaUrls { get; set; }

        public List<string> AudioMediaTitles { get; set; }
        public List<string> AudioMediaUrls { get; set; }

        public List<string> GoogleDriveMediaTitles { get; set; }
        public List<string> GoogleDriveMediaUrls { get; set; }

        public EditTraining()
        {
            YoutubeMediaTitles = new List<string>();
            YoutubeMediaUrls = new List<string>();

            AudioMediaTitles = new List<string>();
            AudioMediaUrls = new List<string>();

            GoogleDriveMediaTitles = new List<string>();
            GoogleDriveMediaUrls = new List<string>();
        }
    }
}