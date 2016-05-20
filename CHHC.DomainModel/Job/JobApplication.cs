using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CHHC.DomainModel
{
    public class JobApplication
    {
        public int Id { get; set; }

        public string ApplicantFirstName { get; set; }
        public string ApplicantLastName { get; set; }
        public string ApplicationDate { get; set; }
        public int HowDidYouHear { get; set; } // 1 - Newspaper, 2 - Referral, 3 - Walk-in, 4 - Agency, 5 - Friend, 6 - Other
        public string HowDidYouHearOther { get; set; }
        public string Position { get; set; }
        public int PositionType { get; set; } // 1 - Full Time, 2 - Part Time, 3 - Per Diem, 4 - Temp
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string PhoneHome { get; set; }
        public string PhoneCell { get; set; }
        public string Email { get; set; }

        public ICollection<JobApplicationEducation> Educations { get; private set; }
        public string SpecialEducation { get; set; }
        public string ClinicalLicenseType { get; set; }
        public string ClinicalLicenseNumber { get; set; }
        public string ClinicalLicenseState { get; set; }

        public ICollection<JobApplicationWork> Works { get; private set; }

        public bool Authorized { get; set; }
        public bool Eligible { get; set; }
        public bool EmployedBefore { get; set; }
        public string EmployedBeforePosition { get; set; }
        public string WhenAvailableToBegin { get; set; }
        public bool EverDischarged { get; set; }
        public string EverDischargedExplanation { get; set; }
        public bool EverHadStipulations { get; set; }
        public string EverHadStipulationsExplanation { get; set; }
        public bool EverSanctioned { get; set; }
        public bool RelatedToBoard { get; set; }
        public bool MayWeContactPresentEmployer { get; set; }
        public bool MayWeContactPreviousEmployer { get; set; }

        public DateTime? DateFilledAndAgreed { get; set; }

        public JobApplication()
        {
            Educations = new Collection<JobApplicationEducation>();
            Works = new Collection<JobApplicationWork>();
        }
    }
}