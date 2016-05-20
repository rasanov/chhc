using CHHC.DomainModel;

namespace CHHC.Web.Models
{
    public class ViewTemplateCaseConferenceQuestion
    {
        public int Id { get; set; }
        public int Ordinal { get; set; }
        public TemplateCaseConferenceQuestionType Type { get; set; }
        public ViewTemplateCaseConferenceTextQuestion TemplateCaseConferenceTextQuestion { get; set; }
        public ViewTemplateCaseConferenceDateQuestion TemplateCaseConferenceDateQuestion { get; set; }
        public ViewTemplateCaseConferenceMultichoiceQuestion TemplateCaseConferenceMultichoiceQuestion { get; set; }
        public ViewTemplateCaseConferenceMultichoiceTextQuestion TemplateCaseConferenceMultichoiceTextQuestion { get; set; }

        public bool Validate()
        {
            switch (Type)
            {
                case TemplateCaseConferenceQuestionType.Text:
                    return TemplateCaseConferenceTextQuestion.Validate();
                case TemplateCaseConferenceQuestionType.Date:
                    return TemplateCaseConferenceDateQuestion.Validate();
                case TemplateCaseConferenceQuestionType.Multichoice:
                    return TemplateCaseConferenceMultichoiceQuestion.Validate();
                case TemplateCaseConferenceQuestionType.MultichoiceText:
                    return TemplateCaseConferenceMultichoiceTextQuestion.Validate();
            }
            return true;
        }
    }
}