using Egoal.Extensions;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Egoal.Net.Mail
{
    public abstract class EmailSenderBase : IEmailSender
    {
        protected readonly EmailOptions _emailOptions;

        public EmailSenderBase(IOptions<EmailOptions> options)
        {
            _emailOptions = options.Value;
        }

        public virtual async Task SendAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendAsync(new MailMessage
            {
                To = { to },
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml
            });
        }

        public virtual void Send(string to, string subject, string body, bool isBodyHtml = true)
        {
            Send(new MailMessage
            {
                To = { to },
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml
            });
        }

        public virtual async Task SendAsync(string from, string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendAsync(new MailMessage(from, to, subject, body) { IsBodyHtml = isBodyHtml });
        }

        public virtual void Send(string from, string to, string subject, string body, bool isBodyHtml = true)
        {
            Send(new MailMessage(from, to, subject, body) { IsBodyHtml = isBodyHtml });
        }

        public virtual async Task SendAsync(MailMessage mail, bool normalize = true)
        {
            if (normalize)
            {
                NormalizeMail(mail);
            }

            await SendEmailAsync(mail);
        }

        public virtual void Send(MailMessage mail, bool normalize = true)
        {
            if (normalize)
            {
                NormalizeMail(mail);
            }

            SendEmail(mail);
        }

        protected abstract Task SendEmailAsync(MailMessage mail);

        protected abstract void SendEmail(MailMessage mail);

        protected virtual void NormalizeMail(MailMessage mail)
        {
            if (mail.From == null || mail.From.Address.IsNullOrEmpty())
            {
                mail.From = new MailAddress(
                    _emailOptions.EmailDefaultFromAddress,
                    _emailOptions.EmailDefaultFromDisplayName,
                    Encoding.UTF8
                    );
            }

            if (mail.HeadersEncoding == null)
            {
                mail.HeadersEncoding = Encoding.UTF8;
            }

            if (mail.SubjectEncoding == null)
            {
                mail.SubjectEncoding = Encoding.UTF8;
            }

            if (mail.BodyEncoding == null)
            {
                mail.BodyEncoding = Encoding.UTF8;
            }
        }
    }
}
