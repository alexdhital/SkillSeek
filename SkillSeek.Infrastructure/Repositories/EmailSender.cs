using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ServiceAppointmentSystem.Repositories
{
    public class EmailSender : IEmailSender
    {
        public EmailSender()
        {

        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            string fromMail = "alexdhital120@gmail.com";

            string fromPassword = "awcqnyeyhmuehyen";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = subject;
            message.To.Add(new MailAddress(email));
            message.Body = "<html><body> " + htmlMessage + " </body></html>";
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };

            smtpClient.Send(message);
		}
    }

    // public interface IEmailSender
    // {
    //     Task SendEmailAsync(string email, string subject, string htmlMessage);
    // }
}
