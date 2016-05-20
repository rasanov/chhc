using System;
using CHHC.Web.Models;

namespace CHHC.Web
{
    public static class ViewHelper
    {
        public static string GetRoleFriendlyName(string roleName)
        {
            switch (roleName)
            {
                case RoleNames.ControlPanelAdmin:
                    return "Control Panel Admin";
                case RoleNames.CHHCAdmin:
                    return "Administrator";
                case RoleNames.Training_Edit:
                    return "Can manage tranings";
                case RoleNames.Training_View:
                    return "Takes trainings";
                case RoleNames.Patient_Edit:
                    return "Can manage patients";
                case RoleNames.CaseConference_Edit:
                    return "Can manage case conferences";
                case RoleNames.CaseConference_View:
                    return "Fills case conferences";
                case RoleNames.Document_View_All:
                    return "Can see all untargetted documents";
                case RoleNames.Document_View:
                    return "Can see targetted (assigned) documents";
            }
            throw new Exception("Unknown role.");
        }

        public static string PrepareTitle(string title)
        {
            return title.Replace(" > ", "&nbsp;<span class='glyphicon glyphicon-chevron-right'></span>&nbsp;");
        }
    }
}