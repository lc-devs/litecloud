using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using System.Threading.Tasks;
using WashALoadService.Models;
using WashALoadService.Settings;

namespace WashALoadService.Services
{
    public class MailService : IMailService
    {
        private MailSettings _mailSettings;
        public MailService()
        {
            _mailSettings = new MailSettings();

            _mailSettings.DisplayName = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("MailSettings")["DisplayName"];
            _mailSettings.Mail = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("MailSettings")["Mail"];
            _mailSettings.Password = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("MailSettings")["Password"];
            _mailSettings.Host = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("MailSettings")["Host"];
            _mailSettings.Port =  int.Parse( new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("MailSettings")["Port"]);
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            // create email message
            var email = new MimeKit.MimeMessage();
            email.From.Add(new MimeKit.MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
            email.To.Add(new MimeKit.MailboxAddress(mailRequest.ToEmail, mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            email.Body = new MimeKit.TextPart(mailRequest.textFormat) { Text = mailRequest.Body };

            // send email
            using (var client = new SmtpClient())
            {
                client.CheckCertificateRevocation = false;
                client.SslProtocols = System.Security.Authentication.SslProtocols.Ssl3 | System.Security.Authentication.SslProtocols.Tls | System.Security.Authentication.SslProtocols.Tls11 | System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13;

                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.SslOnConnect);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(_mailSettings.Mail, _mailSettings.Password);

                await client.SendAsync(email);
                client.Disconnect(true);
            }

        }
    }
}
