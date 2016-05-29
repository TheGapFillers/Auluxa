using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Auluxa.WebApp.Auth
{
    public class AuthEmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            // build message
            var mailMessage = new MailMessage(
                "no-reply1@8securities.com",
                message.Destination,
                message.Subject,
                message.Body)
            {
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };

            // build smtp client and send message
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.Credentials = new NetworkCredential("info@auluxa.com", "auluxa55");
                client.EnableSsl = true;

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
