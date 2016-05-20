using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CHHC.Web.Models.JobModels
{
    public class ViewJobApplication
    {
        public int Id { get; set; }

        [DisplayName("First Name")]
        public string ApplicantFirstName { get; set; }

        [DisplayName("Last Name")]
        public string ApplicantLastName { get; set; }

        [DisplayName("Date")]
        public string ApplicationDate { get; set; }

        [DisplayName("How Did You Hear About Us")]
        public int HowDidYouHear { get; set; } // 1 - Newspaper, 2 - Referral, 3 - Walk-in, 4 - Agency, 5 - Friend, 6 - Other

        [DisplayName("Other")]
        public string HowDidYouHearOther { get; set; }

        public string Position { get; set; }

        [DisplayName("Are you seeking")]
        public int PositionType { get; set; } // 1 - Full Time, 2 - Part Time, 3 - Per Diem, 4 - Temp

        [DisplayName("Street")]
        public string StreetAddress { get; set; }

        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        [DisplayName("Home Phone")]
        public string PhoneHome { get; set; }

        [DisplayName("Cell Phone")]
        public string PhoneCell { get; set; }

        public string Email { get; set; }

        public List<ViewJobApplicationEducation> Educations { get; set; }

        [DisplayName("Special Education")]
        public string SpecialEducation { get; set; }

        [DisplayName("Type")]
        public string ClinicalLicenseType { get; set; }

        [DisplayName("Number")]
        public string ClinicalLicenseNumber { get; set; }

        [DisplayName("State")]
        public string ClinicalLicenseState { get; set; }

        public List<ViewJobApplicationWork> Works { get; set; }
        //public List<ViewJobApplicationReference> References { get; set; }

        [DisplayName("Are you legally authorized to work in the United States?")]
        public bool Authorized { get; set; }

        [DisplayName("If under age 18, can you provide required proof of eligibility to work?")]
        public bool Eligible { get; set; }

        [DisplayName("Have you ever been employed here before?")]
        public bool EmployedBefore { get; set; }

        [DisplayName("Position held and dates employed")]
        public string EmployedBeforePosition { get; set; }

        [DisplayName("When are you available to begin work?")]
        public string WhenAvailableToBegin { get; set; }

        [DisplayName("Have you ever been discharged by a former employer or resigned after being told your performance was unsatisfactory?")]
        public bool EverDischarged { get; set; }

        [DisplayName("If yes please explain")]
        public string EverDischargedExplanation { get; set; }

        [DisplayName("If you carry a professional license, have you ever had stipulations imposed on it?")]
        public bool EverHadStipulations { get; set; }

        [DisplayName("If yes please explain")]
        public string EverHadStipulationsExplanation { get; set; }

        [DisplayName("Have you ever been sanctioned by Medicare, Medicaid or Title V?")]
        public bool EverSanctioned { get; set; }

        [DisplayName("Are you related to board or staff members?")]
        public bool RelatedToBoard { get; set; }

        [DisplayName("May we contact your present employer?")]
        public bool MayWeContactPresentEmployer { get; set; }
            
        [DisplayName("May we contact your former employer(s)?")]
        public bool MayWeContactPreviousEmployer { get; set; }

        [DisplayName("Date")]
        public DateTime? DateFilledAndAgreed { get; set; }


        public string ErrorMessage { get; set; }

        public ViewJobApplication()
        {
            Educations = new List<ViewJobApplicationEducation>();
            Works = new List<ViewJobApplicationWork>();
            //References = new List<ViewJobApplicationReference>();
        }
    }
}