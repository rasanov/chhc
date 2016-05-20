using System.Linq;
using CHHC.DAL.Infrastructure;

namespace CHHC.Web
{
    public class PatientHelper
    {
        public static bool NewPatientValidate(string name)
        {
            using (var context = new DomainContext())
            {
                if (context.Patients.Any(x => x.Name == name))
                {
                    return false;
                }
                return true;
            }
        }

        public static bool ExistingPatientValidate(int id, string name)
        {
            using (var context = new DomainContext())
            {
                if (context.Patients.Any(x => x.Id != id && x.Name == name))
                {
                    return false;
                }
                return true;
            }
        }
    }
}