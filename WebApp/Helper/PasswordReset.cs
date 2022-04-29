using System.Net;
using System.Net.Mail;

namespace WebApp.Helper
{
    public static class PasswordReset
    {
        public static void PasswordResetSendEmail(string link, string email)
        {
            var fromAddress = new MailAddress("casedgts@gmail.com", "WebApp");
            var toAddress = new MailAddress(email);
            const string fromPassword = "DigitusCase";
            const string subject = "www.WebApp.com::Reset Password";
            string body = $"<a href='{link}'>Reset Password Link</a>";

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