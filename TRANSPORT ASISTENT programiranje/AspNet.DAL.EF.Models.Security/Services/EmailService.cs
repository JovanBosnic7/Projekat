using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace AspNet.DAL.EF.Models.Security
{
    public class EmailService : IIdentityMessageService
    {
        //public Task SendAsync(IdentityMessage message)
        //{ return Task.FromResult(0); }

        // Demo template
        public Task SendAsync(IdentityMessage message)
        {
            // Credentials:
            var credentialUserName = "bexbex@bex.rs"; // "yourAccount@outlook.com";
            var sentFrom = "bexbex@bex.rs";//"yourAccount@outlook.com";
            var pwd = "sgdwAU6Z";//"yourPassword";

            // Configure the client:
            var client = new System.Net.Mail.SmtpClient(
                "mail.bex.rs");

            client.Port = 587;
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            // Create the credentials:
            var credentials = new System.Net.NetworkCredential(
                credentialUserName, pwd);

            client.EnableSsl = true;
            client.Credentials = credentials;

            // Create the message:
            var mail = new System.Net.Mail.MailMessage(
                sentFrom, message.Destination);

            mail.Subject = message.Subject;
            mail.Body = message.Body;

            // Send:
            return client.SendMailAsync(mail);
        }
    }
}
