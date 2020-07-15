using Egoal.Net.Mail;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Mail;
using System.Threading.Tasks;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Egoal.Email.MailKit
{
    public class MailKitEmailSender : EmailSenderBase
    {
        public MailKitEmailSender(IOptions<EmailOptions> options)
            : base(options)
        {
        }

        public override async Task SendAsync(string from, string to, string subject, string body, bool isBodyHtml = true)
        {
            using (var client = BuildSmtpClient())
            {
                var message = BuildMimeMessage(from, to, subject, body, isBodyHtml);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

        public override void Send(string from, string to, string subject, string body, bool isBodyHtml = true)
        {
            using (var client = BuildSmtpClient())
            {
                var message = BuildMimeMessage(from, to, subject, body, isBodyHtml);
                client.Send(message);
                client.Disconnect(true);
            }
        }

        private static MimeMessage BuildMimeMessage(string from, string to, string subject, string body, bool isBodyHtml = true)
        {
            var bodyType = isBodyHtml ? "html" : "plain";
            var message = new MimeMessage
            {
                Subject = subject,
                Body = new TextPart(bodyType)
                {
                    Text = body
                }
            };

            message.From.Add(new MailboxAddress(from));
            message.To.Add(new MailboxAddress(to));

            return message;
        }

        protected override async Task SendEmailAsync(MailMessage mail)
        {
            using (var client = BuildSmtpClient())
            {
                var message = MimeMessage.CreateFromMailMessage(mail);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

        protected override void SendEmail(MailMessage mail)
        {
            using (var client = BuildSmtpClient())
            {
                var message = MimeMessage.CreateFromMailMessage(mail);
                client.Send(message);
                client.Disconnect(true);
            }
        }

        protected virtual SmtpClient BuildSmtpClient()
        {
            var client = new SmtpClient();

            try
            {
                ConfigureClient(client);
                return client;
            }
            catch
            {
                client.Dispose();
                throw;
            }
        }

        protected virtual void ConfigureClient(SmtpClient client)
        {
            client.Connect(
                _emailOptions.SmtpHost,
                _emailOptions.SmtpPort,
                GetSecureSocketOption()
            );

            if (_emailOptions.SmtpUseDefaultCredentials)
            {
                return;
            }

            client.Authenticate(
                _emailOptions.SmtpUserName,
                _emailOptions.SmtpPassword
            );
        }

        protected virtual SecureSocketOptions GetSecureSocketOption()
        {
            return _emailOptions.SmtpUseSSL
                ? SecureSocketOptions.SslOnConnect
                : SecureSocketOptions.StartTlsWhenAvailable;
        }
    }
}
