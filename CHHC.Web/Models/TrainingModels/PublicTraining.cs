using System;
using System.Collections.Generic;

namespace CHHC.Web.Models
{
    public class PublicTraining
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int DurationMinutes { get; set; }
        public string InstructorName { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedDate { get; set; }

        public List<string> YoutubeMediaTitles { get; set; }
        public List<string> YoutubeMediaUrls { get; set; }

        public List<string> AudioMediaTitles { get; set; }
        public List<string> AudioMediaUrls { get; set; }

        public List<string> GoogleDriveMediaTitles { get; set; }
        public List<string> GoogleDriveMediaUrls { get; set; }

        public PublicTraining()
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