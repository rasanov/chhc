using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CHHC.Web.Models
{
    public class CreateVisitNote
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int PatientId { get; set; }
        public List<ViewPatient> ViewPatients { get; set; }

        [DisplayName("Record Number")]
        public string RecordNumber { get; set; }

        [DisplayName("Reassessment Visit")]
        public bool IsVisitReassessment { get; set; }

        [DisplayName("PRN Visit")]
        public bool IsVisitPRN { get; set; }

        [DisplayName("Non-Billable Visit")]
        public bool IsVisitNonBillable { get; set; }

        [DisplayName("Administrative changes (Address, Phone Number, PC, Insurance ...)")]
        public string AdministrativeChanges { get; set; }

        [DisplayName("D - Direct Skilled Nursing Visit")]
        public bool IsVisitD { get; set; }

        [DisplayName("O/A - Observation and Assessment Visit")]
        public bool IsVisitOA { get; set; }

        [DisplayName("T/E - Teaching and/or Education Visit")]
        public bool IsVisitTE { get; set; }

        [DisplayName("Homebound Taxing Effort")]
        public bool IsHomebound { get; set; }

        public string HomeboundEfforts { get; set; }

        [DisplayName("Assistive Device")]
        public string AssistiveDevice { get; set; }

        [DisplayName("Not homebound, not confined to place of residence, services medically necessary, alternative more costly")]
        public bool IsNotHomebound { get; set; }

        #region VitalSigns

        [DisplayName("Temperature")]
        public string Temperature { get; set; }

        [DisplayName("Weight")]
        public string Weight { get; set; }

        [DisplayName("BP Standing")]
        public string BPStanding { get; set; }

        [DisplayName("BP Sitting")]
        public string BPSitting { get; set; }

        [DisplayName("BP Lying")]
        public string BPLying { get; set; }

        [DisplayName("FSBS Fasting")]
        public string FSBSFasting { get; set; }

        [DisplayName("Non Fasting")]
        public string FSBSNonFasting { get; set; }

        #endregion

        #region Respiratory

        [DisplayName("WNL")]
        public bool RespiratoryWNL { get; set; }

        [DisplayName("Respiratory Rate")]
        public string RespiratoryRate { get; set; }

        [DisplayName("Lung Sound")]
        public string LungSound { get; set; }

        public string SOB { get; set; }

        [DisplayName("O2 Sat")]
        public string O2Sat { get; set; }

        [DisplayName("Cough")]
        public bool CoughExists { get; set; }

        [DisplayName("Dry")]
        public bool CoughDry { get; set; }

        [DisplayName("Productive")]
        public bool CoughProductive { get; set; }

        [DisplayName("Sputum's Color")]
        public string SputumColor { get; set; }

        #endregion

        #region Cardiovascular

        [DisplayName("WNL")]
        public bool CardiovascularWNL { get; set; }

        [DisplayName("HR")]
        public string HR { get; set; }

        [DisplayName("Reg")]
        public bool HRReg { get; set; }

        [DisplayName("Irreg")]
        public bool HRIrreg { get; set; }

        [DisplayName("Chest Pain")]
        public bool ChestPain { get; set; }

        [DisplayName("Dizziness")]
        public bool Dizziness { get; set; }

        [DisplayName("Edema")]
        public bool Edema { get; set; }

        [DisplayName("R Ankle")]
        public string RightAnkle { get; set; }

        [DisplayName("L Ankle")]
        public string LeftAnkle { get; set; }

        [DisplayName("R Calf")]
        public string RightCalf { get; set; }

        [DisplayName("L Calf")]
        public string LeftCalf { get; set; }

        [DisplayName("Periferal Pulses")]
        public string PeriferalPulse { get; set; }

        #endregion

        #region Neurosensory

        [DisplayName("WNL")]
        public bool NeurosensoryWNL { get; set; }

        [DisplayName("Oriented")]
        public bool IsOriented { get; set; }

        [DisplayName("Oriented")]
        public string Oriented { get; set; }

        [DisplayName("Forgetful")]
        public bool Forgetful { get; set; }
        
        [DisplayName("Depressed")]
        public bool Depressed { get; set; }
        
        [DisplayName("Anxious")]
        public bool Anxious { get; set; }
        
        [DisplayName("Hearing Impair.")]
        public bool HearingImpairment { get; set; }
        
        [DisplayName("Visual Impair.")]
        public bool VisualImpairment { get; set; }
        
        [DisplayName("Speech Impair.")]
        public bool SpeechImpairment { get; set; }
        
        [DisplayName("Legally Blind")]
        public bool LegallyBlind { get; set; }
        
        [DisplayName("Dec. Sensitivity")]
        public bool DecreaseSensitivity { get; set; }

        [DisplayName("Seizure")]
        public bool Seizure { get; set; }

        [DisplayName("Other")]
        public string NeurosensoryOther { get; set; }

        #endregion

        #region Gastrointestinal

        [DisplayName("WNL")]
        public bool GastrointestinalWNL { get; set; }

        [DisplayName("Last BM")]
        public string LastBM { get; set; }

        [DisplayName("Bowel Sounds are present in all 4Q")]
        public bool BowelSounds { get; set; }
        
        [DisplayName("Nausea / Vomiting")]
        public bool NauseaVomiting { get; set; }

        [DisplayName("Incontinence")]
        public bool GastrointestinalIncontinence { get; set; }

        [DisplayName("Diarrhea")]
        public bool Diarrhea { get; set; }

        [DisplayName("Constipation")]
        public bool Constipation { get; set; }

        [DisplayName("Tube Feeding")]
        public bool TubeFeeding { get; set; }

        [DisplayName("Colostomy")]
        public bool Colostomy { get; set; }

        [DisplayName("Diet")]
        public string Diet { get; set; }

        #endregion

        #region Genitourinary

        [DisplayName("WNL")]
        public bool GenitourinaryWNL { get; set; }

        [DisplayName("Distention")]
        public bool Distention { get; set; }

        [DisplayName("Retention")]
        public bool Retention { get; set; }

        [DisplayName("Burning")]
        public bool Burning { get; set; }

        [DisplayName("Pain")]
        public bool Pain { get; set; }

        [DisplayName("Incontinence")]
        public bool GenitourinaryIncontinence { get; set; }

        [DisplayName("Frequency")]
        public bool Frequency { get; set; }

        [DisplayName("Urgency")]
        public bool Urgency { get; set; }

        [DisplayName("Nocturia")]
        public bool Nocturia { get; set; }

        [DisplayName("Foley Catheter")]
        public bool FoleyCatheter { get; set; }
        
        #endregion
    }
}