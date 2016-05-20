using System.Data.Entity;
using CHHC.DomainModel;

namespace CHHC.DAL.Infrastructure
{
    public class DomainContext : DbContext
    {
        public DbSet<UserSettings> UserSettings { get; set; }

        public DbSet<Training> Trainings { get; set; }
        public DbSet<TrainingMedia> TrainingMedia { get; set; }
        public DbSet<MediaType> MediaTypes { get; set; }
        public DbSet<MultichoiceQuestion> MultichoiceQuestions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<UserSettingsTraining> UserSettingsTraining { get; set; }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<UserSettingsPatient> UserSettingsPatient { get; set; }

        public DbSet<TemplateCaseConference> TemplateCaseConferences { get; set; }
        public DbSet<TemplateCaseConferenceQuestion> TemplateCaseConferenceQuestions { get; set; }
        public DbSet<TemplateCaseConferenceTextQuestion> TemplateCaseConferenceTextQuestions { get; set; }
        public DbSet<TemplateCaseConferenceDateQuestion> TemplateCaseConferenceDateQuestions { get; set; }
        public DbSet<TemplateCaseConferenceMultichoiceQuestion> TemplateCaseConferenceMultichoiceQuestions { get; set; }
        public DbSet<TemplateCaseConferenceMultichoiceQuestionAnswer> TemplateCaseConferenceMultichoiceQuestionAnswers { get; set; }
        public DbSet<TemplateCaseConferenceMultichoiceTextQuestion> TemplateCaseConferenceMultichoiceTextQuestions { get; set; }
        public DbSet<TemplateCaseConferenceMultichoiceTextQuestionAnswer> TemplateCaseConferenceMultichoiceTextQuestionAnswers { get; set; }

        public DbSet<CaseConference> CaseConferences { get; set; }
        public DbSet<CaseConferenceQuestion> CaseConferenceQuestions { get; set; }
        public DbSet<CaseConferenceTextQuestion> CaseConferenceTextQuestions { get; set; }
        public DbSet<CaseConferenceDateQuestion> CaseConferenceDateQuestions { get; set; }
        public DbSet<CaseConferenceMultichoiceQuestion> CaseConferenceMultichoiceQuestions { get; set; }
        public DbSet<CaseConferenceMultichoiceQuestionAnswer> CaseConferenceMultichoiceQuestionAnswers { get; set; }
        public DbSet<CaseConferenceMultichoiceTextQuestion> CaseConferenceMultichoiceTextQuestions { get; set; }
        public DbSet<CaseConferenceMultichoiceTextQuestionAnswer> CaseConferenceMultichoiceTextQuestionAnswers { get; set; }

        public DbSet<Document> Documents { get; set; }
        public DbSet<UserSettingsDocument> UserSettingsDocuments { get; set; }

        public DbSet<JobApplication> JobApplication { get; set; }
        public DbSet<JobApplicationEducation> JobApplicationEducation { get; set; }
        public DbSet<JobApplicationWork> JobApplicationWork { get; set; }

        public DbSet<Blog> Blogs { get; set; }

        public DomainContext() : base("DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TrainingMedia>()
                .HasRequired(x => x.Training)
                .WithMany(t => t.Media)
                .WillCascadeOnDelete();

            modelBuilder.Entity<UserSettingsTraining>()
                .HasRequired(x => x.UserSettings)
                .WithMany(t => t.UserSettingsTrainings)
                .WillCascadeOnDelete();

            modelBuilder.Entity<UserSettingsTraining>()
                .HasRequired(x => x.Training)
                .WithMany(t => t.UserSettingsTrainings)
                .WillCascadeOnDelete();



            modelBuilder.Entity<CaseConferenceQuestion>()
                .HasRequired(x => x.CaseConference)
                .WithMany(t => t.Questions)
                .WillCascadeOnDelete();

            modelBuilder.Entity<CaseConferenceMultichoiceQuestionAnswer>()
                .HasRequired(x => x.CaseConferenceMultichoiceQuestion)
                .WithMany(t => t.Answers)
                .WillCascadeOnDelete();

            modelBuilder.Entity<CaseConferenceMultichoiceTextQuestionAnswer>()
                .HasRequired(x => x.CaseConferenceMultichoiceQuestion)
                .WithMany(t => t.ChoiceAnswers)
                .WillCascadeOnDelete();



            modelBuilder.Entity<UserSettingsDocument>()
                .HasRequired(x => x.UserSettings)
                .WithMany(t => t.UserSettingsDocuments)
                .WillCascadeOnDelete();

            modelBuilder.Entity<UserSettingsDocument>()
                .HasRequired(x => x.Document)
                .WithMany(t => t.AssignedTo)
                .WillCascadeOnDelete();
        }
    }
}