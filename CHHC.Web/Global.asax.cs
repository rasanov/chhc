using System.Threading;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CHHC.Web.Models;
using CHHC.Web.Mvc;
using WebMatrix.WebData;
using AutoMapper;
using CHHC.DomainModel;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CHHC.Web.Models.JobModels;

namespace CHHC.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            var routes = RouteTable.Routes;
            routes.IgnoreRoute("favicon.ico");
            RouteConfig.RegisterRoutes(routes);
            
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            InitMembership();
            InitViewEngines();
            InitMapper();
        }

        public static void InitMembership()
        {
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        private void InitViewEngines()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new ChhcRazorViewEngine());
        }

        private void InitMapper()
        {
            InitTrainingMapper();
            InitPatientMapper();
            InitCaseConferenceMapper();
            InitDocumentMapper();
            InitBlogMapper();
            InitJobMapper();
        }

        private static void InitTrainingMapper()
        {
            Mapper.CreateMap<Training, ViewTraining>();
            Mapper.CreateMap<Training, UserTraining>();

            Mapper.CreateMap<Training, CreateTraining>()
                .ForMember(
                    destination => destination.YoutubeMediaTitles,
                    memberOption => memberOption.MapFrom(source => source.Media
                        .Where(x => x.MediaType.Key == "youtube")
                        .Select(x => x.Title)
                    )
                )
                .ForMember(
                    destination => destination.YoutubeMediaUrls,
                    memberOption => memberOption.MapFrom(source => source.Media
                        .Where(x => x.MediaType.Key == "youtube")
                        .Select(x => x.Url)
                    )
                )
                .ForMember(
                    destination => destination.AudioMediaTitles,
                    memberOption => memberOption.MapFrom(source => source.Media
                        .Where(x => x.MediaType.Key == "audio")
                        .Select(x => x.Title)
                    )
                )
                .ForMember(
                    destination => destination.AudioMediaUrls,
                    memberOption => memberOption.MapFrom(source => source.Media
                        .Where(x => x.MediaType.Key == "audio")
                        .Select(x => x.Url)
                    )
                )
                .ForMember(
                    destination => destination.GoogleDriveMediaTitles,
                    memberOption => memberOption.MapFrom(source => source.Media
                        .Where(x => x.MediaType.Key == "googledrive")
                        .Select(x => x.Title)
                    )
                )
                .ForMember(
                    destination => destination.GoogleDriveMediaUrls,
                    memberOption => memberOption.MapFrom(source => source.Media
                        .Where(x => x.MediaType.Key == "googledrive")
                        .Select(x => x.Url)
                    )
                );

            Mapper.CreateMap<Training, EditTraining>()
                .ForMember(
                    destination => destination.YoutubeMediaTitles,
                    memberOption => memberOption.MapFrom(source => source.Media
                        .Where(x => x.MediaType.Key == "youtube")
                        .Select(x => x.Title)
                    )
                )
                .ForMember(
                    destination => destination.YoutubeMediaUrls,
                    memberOption => memberOption.MapFrom(source => source.Media
                        .Where(x => x.MediaType.Key == "youtube")
                        .Select(x => x.Url)
                    )
                )
                .ForMember(
                    destination => destination.AudioMediaTitles,
                    memberOption => memberOption.MapFrom(source => source.Media
                        .Where(x => x.MediaType.Key == "audio")
                        .Select(x => x.Title)
                    )
                )
                .ForMember(
                    destination => destination.AudioMediaUrls,
                    memberOption => memberOption.MapFrom(source => source.Media
                        .Where(x => x.MediaType.Key == "audio")
                        .Select(x => x.Url)
                    )
                )
                .ForMember(
                    destination => destination.GoogleDriveMediaTitles,
                    memberOption => memberOption.MapFrom(source => source.Media
                        .Where(x => x.MediaType.Key == "googledrive")
                        .Select(x => x.Title)
                    )
                )
                .ForMember(
                    destination => destination.GoogleDriveMediaUrls,
                    memberOption => memberOption.MapFrom(source => source.Media
                        .Where(x => x.MediaType.Key == "googledrive")
                        .Select(x => x.Url)
                    )
                );

            Mapper.CreateMap<UserSettingsTraining, ViewUserTraining>()
                .ForMember(
                    destination => destination.TrainingId,
                    memberOptions => memberOptions.MapFrom(source => source.Training.Id))
                .ForMember(
                    destination => destination.TrainingTitle,
                    memberOptions => memberOptions.MapFrom(source => source.Training.Title))
                .ForMember(
                    destination => destination.TrainingDurationMinutes,
                    memberOptions => memberOptions.MapFrom(source => source.Training.DurationMinutes));

            Mapper.CreateMap<Training, PublicTraining>()
                .ForMember(
                    destination => destination.YoutubeMediaTitles,
                    memberOption => memberOption.MapFrom(source => source.Media
                        .Where(x => x.MediaType.Key == "youtube")
                        .Select(x => x.Title)
                    )
                )
                .ForMember(
                    destination => destination.YoutubeMediaUrls,
                    memberOption => memberOption.MapFrom(source => source.Media
                        .Where(x => x.MediaType.Key == "youtube")
                        .Select(x => x.Url)
                    )
                )
                .ForMember(
                    destination => destination.AudioMediaTitles,
                    memberOption => memberOption.MapFrom(source => source.Media
                        .Where(x => x.MediaType.Key == "audio")
                        .Select(x => x.Title)
                    )
                )
                .ForMember(
                    destination => destination.AudioMediaUrls,
                    memberOption => memberOption.MapFrom(source => source.Media
                        .Where(x => x.MediaType.Key == "audio")
                        .Select(x => x.Url)
                    )
                )
                .ForMember(
                    destination => destination.GoogleDriveMediaTitles,
                    memberOption => memberOption.MapFrom(source => source.Media
                        .Where(x => x.MediaType.Key == "googledrive")
                        .Select(x => x.Title)
                    )
                )
                .ForMember(
                    destination => destination.GoogleDriveMediaUrls,
                    memberOption => memberOption.MapFrom(source => source.Media
                        .Where(x => x.MediaType.Key == "googledrive")
                        .Select(x => x.Url)
                    )
                );

            Mapper.CreateMap<MultichoiceQuestion, EditMultichoiceQuestion>()
                .ForMember(
                    destination => destination.Answers,
                    memberOption => memberOption.MapFrom(source => source.Answers));

            Mapper.CreateMap<Answer, EditAnswer>();

            Mapper.CreateMap<MultichoiceQuestion, PublicMultichoiceQuestion>()
                .ForMember(
                    destination => destination.Answers,
                    memberOption => memberOption.MapFrom(source => source.Answers));

            Mapper.CreateMap<Answer, PublicAnswer>();
        }

        private static void InitPatientMapper()
        {
            Mapper.CreateMap<Patient, ViewPatient>();
        }

        private static void InitCaseConferenceMapper()
        {
            Mapper.CreateMap<TemplateCaseConference, ViewTemplateCaseConference>();
            Mapper.CreateMap<TemplateCaseConferenceQuestion, ViewTemplateCaseConferenceQuestion>();
            Mapper.CreateMap<TemplateCaseConferenceTextQuestion, ViewTemplateCaseConferenceTextQuestion>();
            Mapper.CreateMap<TemplateCaseConferenceDateQuestion, ViewTemplateCaseConferenceDateQuestion>();
            Mapper.CreateMap<TemplateCaseConferenceMultichoiceQuestion, ViewTemplateCaseConferenceMultichoiceQuestion>();
            Mapper.CreateMap<TemplateCaseConferenceMultichoiceQuestionAnswer, ViewTemplateCaseConferenceMultichoiceQuestionAnswer>();
            Mapper.CreateMap<TemplateCaseConferenceMultichoiceTextQuestion, ViewTemplateCaseConferenceMultichoiceTextQuestion>();
            Mapper.CreateMap<TemplateCaseConferenceMultichoiceTextQuestionAnswer, ViewTemplateCaseConferenceMultichoiceTextQuestionAnswer>();

            Mapper.CreateMap<TemplateCaseConference, CreateCaseConference>();
            Mapper.CreateMap<TemplateCaseConferenceQuestion, EditCaseConferenceQuestion>()
                .ForMember(
                    destination => destination.TextQuestion,
                    memberOptions => memberOptions.MapFrom(source => source.TemplateCaseConferenceTextQuestion)
                )
                .ForMember(
                    destination => destination.DateQuestion,
                    memberOptions => memberOptions.MapFrom(source => source.TemplateCaseConferenceDateQuestion)
                )
                .ForMember(
                    destination => destination.MultichoiceQuestion,
                    memberOptions => memberOptions.MapFrom(source => source.TemplateCaseConferenceMultichoiceQuestion)
                )
                .ForMember(
                    destination => destination.MultichoiceTextQuestion,
                    memberOptions => memberOptions.MapFrom(source => source.TemplateCaseConferenceMultichoiceTextQuestion)
                );
            Mapper.CreateMap<TemplateCaseConferenceTextQuestion, EditCaseConferenceTextQuestion>();
            Mapper.CreateMap<TemplateCaseConferenceDateQuestion, EditCaseConferenceDateQuestion>();
            Mapper.CreateMap<TemplateCaseConferenceMultichoiceQuestion, EditCaseConferenceMultichoiceQuestion>();
            Mapper.CreateMap<TemplateCaseConferenceMultichoiceQuestionAnswer, EditCaseConferenceMultichoiceQuestionAnswer>();
            Mapper.CreateMap<TemplateCaseConferenceMultichoiceTextQuestion, EditCaseConferenceMultichoiceTextQuestion>();
            Mapper.CreateMap<TemplateCaseConferenceMultichoiceTextQuestionAnswer, EditCaseConferenceMultichoiceTextQuestionAnswer>();

            Mapper.CreateMap<CaseConference, EditCaseConference>()
                .ForMember(
                    destination => destination.PatientId,
                    memberOptions => memberOptions.MapFrom(source => source.Patient.Id)
                )
                .ForMember(
                    destination => destination.PatientName,
                    memberOptions => memberOptions.MapFrom(source => source.Patient.Name)
                )
                .ForMember(
                    destination => destination.UserId,
                    memberOptions => memberOptions.MapFrom(source => source.UserSettings.MembershipUserId)
                );
            Mapper.CreateMap<CaseConferenceQuestion, EditCaseConferenceQuestion>()
                .ForMember(
                    destination => destination.TextQuestion,
                    memberOptions => memberOptions.MapFrom(source => source.CaseConferenceTextQuestion)
                )
                .ForMember(
                    destination => destination.DateQuestion,
                    memberOptions => memberOptions.MapFrom(source => source.CaseConferenceDateQuestion)
                )
                .ForMember(
                    destination => destination.MultichoiceQuestion,
                    memberOptions => memberOptions.MapFrom(source => source.CaseConferenceMultichoiceQuestion)
                )
                .ForMember(
                    destination => destination.MultichoiceTextQuestion,
                    memberOptions => memberOptions.MapFrom(source => source.CaseConferenceMultichoiceTextQuestion)
                );
            Mapper.CreateMap<CaseConferenceTextQuestion, EditCaseConferenceTextQuestion>();
            Mapper.CreateMap<CaseConferenceDateQuestion, EditCaseConferenceDateQuestion>();
            Mapper.CreateMap<CaseConferenceMultichoiceQuestion, EditCaseConferenceMultichoiceQuestion>();
            Mapper.CreateMap<CaseConferenceMultichoiceQuestionAnswer, EditCaseConferenceMultichoiceQuestionAnswer>();
            Mapper.CreateMap<CaseConferenceMultichoiceTextQuestion, EditCaseConferenceMultichoiceTextQuestion>()
                .ForMember(
                    destination => destination.Answers,
                    memberOptions => memberOptions.MapFrom(source => source.ChoiceAnswers));
            Mapper.CreateMap<CaseConferenceMultichoiceTextQuestionAnswer, EditCaseConferenceMultichoiceTextQuestionAnswer>();

            Mapper.CreateMap<CaseConference, ViewCaseConference>()
                .ForMember(
                    destination => destination.Status,
                    memberOptions => memberOptions.MapFrom(source => source.Status)
                );
        }

        private static void InitDocumentMapper()
        {
            Mapper.CreateMap<Document, ViewDocument>()
                .ForMember(
                    destination => destination.UploadedByMembershipUserId,
                    memberOptions => memberOptions.MapFrom(source => source.UploadedBy.MembershipUserId)
                );
        }

        private static void InitBlogMapper()
        {
            Mapper.CreateMap<Blog, ViewBlog>();
        }

        private static void InitJobMapper()
        {
            Mapper.CreateMap<JobApplication, ViewJobApplication>();
            Mapper.CreateMap<JobApplicationEducation, ViewJobApplicationEducation>();
            Mapper.CreateMap<JobApplicationWork, ViewJobApplicationWork>();

            Mapper.CreateMap<ViewJobApplication, JobApplication>()
                .ForMember(
                    destination => destination.Educations,
                    memberOptions => memberOptions.MapFrom(source => source.Educations.Where(x => !string.IsNullOrEmpty(x.Name)))
                )
                .ForMember(
                    destination => destination.Works,
                    memberOptions => memberOptions.MapFrom(source => source.Works.Where(x => !string.IsNullOrEmpty(x.JobTitle)))
                );
            Mapper.CreateMap<ViewJobApplicationEducation, JobApplicationEducation>();
            Mapper.CreateMap<ViewJobApplicationWork, JobApplicationWork>();
        }

        public class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                Database.SetInitializer<UsersContext>(null);
                //using (var context = new UsersContext())
                //    context.UserProfiles.Find(1);

                if (!WebSecurity.Initialized)
                    WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            }
        }


        protected void Application_Error()
        {
            var lastError = Server.GetLastError();
            if (lastError != null)
            {
                if (lastError is HttpException && lastError.Message.Contains("exceed"))
                {
                    Response.Clear();
                    Response.Redirect("~/Errors/RequestLengthExceeded");
                    Server.ClearError();
                    Response.End();
                }
            }
        } 
    }
}