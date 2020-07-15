using Egoal.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Egoal.Email.MailKit
{
    public static class MailKitEmailModule
    {
        public static void AddMailKitEmail(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEmailSender, MailKitEmailSender>();

            services.Configure<EmailOptions>(configuration);
        }
    }
}
