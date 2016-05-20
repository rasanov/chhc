using CHHC.DomainModel;

namespace CHHC.Web.Models
{
    public class EditCaseConferenceQuestion
    {
        public int Id { get; set; }
        public int Ordinal { get; set; }
        public TemplateCaseConferenceQuestionType Type { get; set; }
        public EditCaseConferenceTextQuestion TextQuestion { get; set; }
        public EditCaseConferenceDateQuestion DateQuestion { get; set; }
        public EditCaseConferenceMultichoiceQuestion MultichoiceQuestion { get; set; }
        public EditCaseConferenceMultichoiceTextQuestion MultichoiceTextQuestion { get; set; }

        public bool Validate()
        {
            switch (Type)
            {
                case TemplateCaseConferenceQuestionType.Text:
                    return TextQuestion.Validate();
                case TemplateCaseConferenceQuestionType.Date:
                    return DateQuestion.Validate();
                case TemplateCaseConferenceQuestionType.Multichoice:
                    return MultichoiceQuestion.Validate();
                case TemplateCaseConferenceQuestionType.MultichoiceText:
                    return MultichoiceTextQuestion.Validate();
            }
            return true;
        }
    }
}