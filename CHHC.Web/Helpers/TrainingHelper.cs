using CHHC.DAL.Infrastructure;
using System.Linq;

namespace CHHC.Web.Helpers
{
    public static class TrainingHelper
    {
        public static bool NewTrainingValidate(string title)
        {
            using (var context = new DomainContext())
            {
                if (context.Trainings.Any(x => x.Title == title))
                {
                    return false;
                }
                return true;
            }
        }

        public static bool ExistingTrainingValidate(int id, string title)
        {
            using (var context = new DomainContext())
            {
                if (context.Trainings.Any(x => x.Id != id && x.Title == title))
                {
                    return false;
                }
                return true;
            }
        }

        public static string MinutesToTime(int minutes)
        {
            int hours = minutes/60;
            minutes = minutes%60;
            string minutesValue = minutes.ToString();
            if (minutes < 10)
            {
                minutesValue = "0" + minutesValue;
            }
            return string.Format("0{0}:{1}:00", hours, minutesValue);
        }
    }
}