using System.Net;
using System.Net.Mail;

namespace WebApp.Helper
{
    public static class EmailConfirmation
    {
        public static void SendConfirmEmail(string link, string email)
        {
            var fromAddress = new MailAddress("Your Email Adress", "WebApp");
            var toAddress = new MailAddress(email);
            const string fromPassword = "Your Email Password";
            const string subject = "www.WebApp.com::Confirm Email";
            string body = $"<a href='{link}'>Confirm Email Link</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
                   {
                       Subject = subject,
                       Body = body,
                       IsBodyHtml = true
                   })
            {
                smtp.Send(message);
            }
        }
    }
}