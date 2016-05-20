namespace CHHC.Web.Models
{
    public class RoleNames
    {
        public const string ControlPanelAdmin = "ControlPanelAdmin";
        public const string CHHCAdmin = "CHHCAdmin";
        public const string Training_Edit = "Training_Edit";
        public const string Training_View = "Training_View";
        public const string Patient_Edit = "Patient_Edit";
        public const string CaseConference_Edit = "CaseConference_Edit";
        public const string CaseConference_View = "CaseConference_View";
        public const string Document_View_All = "Document_View_All";
        public const string Document_View = "Document_View";
    }

    public enum AccountMenuItems
    {
        None,
        Home,
        Users,
        Roles,
        Trainings,
        Profile,
        Help,
        Patients,
        CaseConferences,
        Documents,
        VisitNotes,
        Blogs,
        Jobs
    }
}