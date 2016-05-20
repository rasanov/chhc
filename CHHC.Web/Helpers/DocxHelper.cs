using System.Linq;
using Novacode;
using System.Drawing;
using CHHC.DomainModel;
using CHHC.Web.Models;

namespace CHHC.Web.Helpers
{
    public static class DocxHelper
    {
        public static void ComposeCompletionForm(DocX document, Training training, UserSettingsTraining userSettingsTraining)
        {
            var formatting = new Formatting
            {
                Bold = true,
                Size = 12,
                FontColor = Color.Black,
                FontFamily = new FontFamily("Times New Roman")
            };

            Paragraph paragraph1 = document.InsertParagraph();
            paragraph1.InsertText("Subject:    " + training.Title, false, formatting);
            paragraph1.LineSpacingAfter = 30;
            paragraph1.AppendLine();

            Paragraph paragraph2 = document.InsertParagraph();
            paragraph2.InsertText("Section:\tPERSONEL\t\t\t\tDate: ", false, formatting);
            if (userSettingsTraining.CompletedDate.HasValue)
            {
                paragraph2.InsertText(userSettingsTraining.CompletedDate.Value.ToString("d"), false, formatting);
            }
            paragraph2.InsertText(" \tRev: ", false, formatting);
            paragraph2.LineSpacingAfter = 30;
            paragraph2.AppendLine();

            Paragraph paragraph3 = document.InsertParagraph();
            UserProfile user;
            using (var userContext = new UsersContext())
            {
                user = userContext.UserProfiles.Single(x => x.UserId == userSettingsTraining.UserSettings.MembershipUserId);
            }
            paragraph3.InsertText("EMPLOYEE NAME: " + user.FullName + "\t\tTITLE: " + user.Title, false, formatting);
            paragraph3.AppendLine();

            Table table = document.AddTable(2, 4);
            table.Rows[0].Cells[0].Paragraphs[0].Append("DATE");
            table.Rows[0].Cells[1].Paragraphs[0].Append("TITLE");
            table.Rows[0].Cells[2].Paragraphs[0].Append("# HOURS");
            table.Rows[0].Cells[3].Paragraphs[0].Append("INSTRUCTOR");
            if (userSettingsTraining.CompletedDate.HasValue)
            {
                table.Rows[1].Cells[0].Paragraphs[0].Append(userSettingsTraining.CompletedDate.Value.ToString("d"));
            }
            table.Rows[1].Cells[1].Paragraphs[0].Append(training.Title);
            table.Rows[1].Cells[2].Paragraphs[0].Append(TrainingHelper.MinutesToTime(training.DurationMinutes));
            table.Rows[1].Cells[3].Paragraphs[0].Append(training.InstructorName);
            document.InsertTable(table);

            Paragraph paragraph5 = document.InsertParagraph();
            paragraph5.InsertText("I have received and read information on the above mandatory topics, completed the post test, and had the opportunity to have my questions answered", false, formatting);
            paragraph5.AppendLine();
            paragraph5.AppendLine();

            Paragraph paragraph6 = document.InsertParagraph();
            paragraph6.InsertText("Employee Signature\t\t\t\tTitle    " + user.Title + "\t\t\tDate", false, formatting);
            paragraph6.AppendLine();

            Paragraph paragraph7 = document.InsertParagraph();
            paragraph7.InsertText("Printed Name    " + user.FullName, false, formatting);
            paragraph7.AppendLine();
            paragraph7.AppendLine();

            Paragraph paragraph8 = document.InsertParagraph();
            paragraph8.InsertText("Instructor    " + training.InstructorName + "\t\tTitle    " + training.InstructorTitle, false, formatting);
            paragraph8.AppendLine();
            paragraph8.AppendLine();
        }
    }
}