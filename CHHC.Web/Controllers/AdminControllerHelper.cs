using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using CHHC.DAL.Infrastructure;
using CHHC.DomainModel;

namespace CHHC.Web.Controllers
{
    public class AdminControllerHelper
    {
        public HttpRequestBase Request { get; private set; }
        public HttpServerUtilityBase Server { get; private set; }

        public AdminControllerHelper(HttpRequestBase request, HttpServerUtilityBase server)
        {
            Request = request;
            Server = server;
        }



        public void AddYoutubeMediaToModelFromRequest(List<string> youtubeMediaTitles, List<string> youtubeMediaUrls, List<string> errors)
        {
            youtubeMediaTitles.Clear();
            youtubeMediaUrls.Clear();
            for (int i = 0; i < 5; i++)
            {
                var title = Request["YoutubeMediaTitles" + i];
                var isEmptyTitle = string.IsNullOrWhiteSpace(title);
                var url = Request["YoutubeMediaUrls" + i];
                var isEmptyUrl = string.IsNullOrWhiteSpace(url);

                if (isEmptyTitle && isEmptyUrl)
                {
                    continue;
                }
                if (!isEmptyTitle && isEmptyUrl)
                {
                    AddIfUnique(errors, "Video title is set but URL is missing.");
                }
                else if (isEmptyTitle/* && !isEmptyUrl*/)
                {
                    AddIfUnique(errors, "Video URL is set but title is missing.");
                }
                youtubeMediaTitles.Add(title);
                youtubeMediaUrls.Add(url);
            }
        }

        public void AddAudioMediaToModelFromRequest(List<string> audioMediaTitles, List<HttpPostedFileBase> audioMediaFiles, List<string> errors)
        {
            audioMediaTitles.Clear();
            for (int i = 0; i < 5; i++)
            {
                var title = Request["AudioMediaTitles" + i];
                var isEmptyTitle = string.IsNullOrWhiteSpace(title);

                HttpPostedFileBase audioMedia = Request.Files["AudioMedia" + i];
                var isFileChosen = (audioMedia != null && audioMedia.ContentLength > 0);

                if (isEmptyTitle && !isFileChosen)
                {
                    return;
                }
                if (!isEmptyTitle && !isFileChosen)
                {
                    errors.Add("Audio title is set but file is not chosen.");
                }
                if (isEmptyTitle /* && isFileChosen*/)
                {
                    errors.Add("Audio file is chosen but title is missing.");
                    continue;
                }

                audioMediaTitles.Add(title);
                audioMediaFiles.Add(audioMedia);
            }
        }

        public void AddChangedAudioMediaToModelFromRequest(List<string> audioMediaTitles, List<string> audioMediaUrls, List<HttpPostedFileBase> audioMediaFiles, List<bool> audioFileChanged, List<string> errors)
        {
            audioMediaTitles.Clear();
            for (int i = 0; i < 5; i++)
            {
                var changed = Request["AudioMediaChanged" + i];
                if (changed == null)
                {
                    break;
                }
                var audioMediaChanged = bool.Parse(changed);

                var isNew = Request["AudioMediaIsNew" + i];
                var audioMediaIsNew = bool.Parse(isNew);

                var title = Request["AudioMediaTitles" + i];
                var isEmptyTitle = string.IsNullOrWhiteSpace(title);

                var url = Request["AudioMediaUrl" + i];

                HttpPostedFileBase audioMedia = Request.Files["AudioMedia" + i];
                var isFileChosen = (audioMedia != null && audioMedia.ContentLength > 0);

                if (isEmptyTitle && !isFileChosen)
                {
                    return;
                }
                if (!isEmptyTitle && !isFileChosen && audioMediaIsNew)
                {
                    errors.Add("Audio title is set but file is not chosen.");
                }
                if (isEmptyTitle && audioMediaIsNew && isFileChosen)
                {
                    errors.Add("Audio file is chosen but title is missing.");
                    continue;
                }
                else if (isEmptyTitle && audioMediaChanged && isFileChosen)
                {
                    errors.Add("Audio file is chosen but title is missing.");
                    continue;
                }

                audioMediaTitles.Add(title);
                if (!audioMediaIsNew && !audioMediaChanged)
                {
                    audioMediaUrls.Add(url);
                }
                audioMediaFiles.Add(audioMedia);
                audioFileChanged.Add(audioMediaChanged);
            }
        }

        public void AddGoogleDriveMediaToModelFromRequest(List<string> googleDriveMediaTitles, List<string> googleDriveMediaUrls, List<string> errors)
        {
            googleDriveMediaTitles.Clear();
            googleDriveMediaUrls.Clear();
            for (int i = 0; i < 5; i++)
            {
                var title = Request["GoogleDriveMediaTitles" + i];
                var isEmptyTitle = string.IsNullOrWhiteSpace(title);
                var url = Request["GoogleDriveMediaUrls" + i];
                var isEmptyUrl = string.IsNullOrWhiteSpace(url);

                if (isEmptyTitle && isEmptyUrl)
                {
                    continue;
                }
                if (!isEmptyTitle && isEmptyUrl)
                {
                    AddIfUnique(errors, "Document title is set but URL is missing.");
                }
                else if (isEmptyTitle/* && !isEmptyUrl*/)
                {
                    AddIfUnique(errors, "Document URL is set but title is missing.");
                }

                googleDriveMediaTitles.Add(title);
                googleDriveMediaUrls.Add(url);
            }
        }



        public void AddYoutubeMediaToContextFromModel(List<string> youtubeMediaTitles, List<string> youtubeMediaUrls, Training training, DomainContext context)
        {
            var mediaType = context.MediaTypes.Single(x => x.Key == "youtube");
            for (int i = 0; i < youtubeMediaTitles.Count; i++)
            {
                training.Media.Add(
                    new TrainingMedia
                    {
                        Training = training,
                        Title = youtubeMediaTitles[i],
                        Url = youtubeMediaUrls[i],
                        MediaType = mediaType
                    }
                );
            }
        }

        public void AddAudioMediaToContextFromModel(List<string> audioMediaTitles, List<HttpPostedFileBase> audioMediaFiles, Training training, DomainContext context)
        {
            var mediaType = context.MediaTypes.Single(x => x.Key == "audio");
            for (int i = 0; i < audioMediaTitles.Count; i++)
            {
                HttpPostedFileBase audioMedia = audioMediaFiles[i];
                var fileName = Path.GetFileName(audioMedia.FileName);
                var path = Path.Combine(Server.MapPath("~/audio/"), fileName);
                audioMedia.SaveAs(path);

                training.Media.Add(
                    new TrainingMedia
                    {
                        Training = training,
                        Title = audioMediaTitles[i],
                        Url = fileName,
                        MediaType = mediaType
                    }
                );
            }
        }

        public void AddGoogleDriveMediaToContextFromModel(List<string> googleDriveMediaTitles, List<string> googleDriveMediaUrls, Training training, DomainContext context)
        {
            var mediaType = context.MediaTypes.Single(x => x.Key == "googledrive");
            for (int i = 0; i < googleDriveMediaTitles.Count; i++)
            {
                training.Media.Add(
                    new TrainingMedia
                    {
                        Training = training,
                        Title = googleDriveMediaTitles[i],
                        Url = googleDriveMediaUrls[i],
                        MediaType = mediaType
                    }
                );
            }
        }



        public void EditYoutubeMedia(List<string> youtubeMediaTitles, List<string> youtubeMediaUrls, Training training, DomainContext context)
        {
            training.Media
                .Where(x => x.MediaType.Key == "youtube")
                .ToList()
                .ForEach(x => context.Entry(x).State = EntityState.Deleted);
            context.SaveChanges();

            AddYoutubeMediaToContextFromModel(youtubeMediaTitles, youtubeMediaUrls, training, context);
            context.SaveChanges();
        }

        public void EditAudioMedia(List<string> audioMediaTitles, List<HttpPostedFileBase> audioMediaFiles, List<bool> audioFileChanged, Training training, DomainContext context)
        {
            var changedAudioMediaTitles = new List<string>();
            var changedAudioFileInputs = new List<HttpPostedFileBase>();
            for (int i = 0; i < audioFileChanged.Count; i++)
            {
                if (audioFileChanged[i])
                {
                    changedAudioMediaTitles.Add(audioMediaTitles[i]);
                    changedAudioFileInputs.Add(audioMediaFiles[i]);
                }
            }

            training.Media
                .Where(x =>
                    x.MediaType.Key == "audio"
                    && changedAudioMediaTitles.Contains(x.Title))
                .ToList()
                .ForEach(x => context.Entry(x).State = EntityState.Deleted);
            context.SaveChanges();

            training.Media
                .Where(x =>
                    x.MediaType.Key == "audio"
                    && !audioMediaTitles.Contains(x.Title))
                .ToList()
                .ForEach(x => context.Entry(x).State = EntityState.Deleted);
            context.SaveChanges();

            AddAudioMediaToContextFromModel(changedAudioMediaTitles, changedAudioFileInputs, training, context);
            context.SaveChanges();
        }

        public void EditGoogleDriveMedia(List<string> googleDriveMediaTitles, List<string> googleDriveMediaUrls, Training training, DomainContext context)
        {
            training.Media
                .Where(x => x.MediaType.Key == "googledrive")
                .ToList()
                .ForEach(x => context.Entry(x).State = EntityState.Deleted);
            context.SaveChanges();

            AddGoogleDriveMediaToContextFromModel(googleDriveMediaTitles, googleDriveMediaUrls, training, context);
            context.SaveChanges();
        }



        public void AddIfUnique(List<string> errors, string error)
        {
            if (!errors.Contains(error))
            {
                errors.Add(error);
            }
        }
    }
}