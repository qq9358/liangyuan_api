using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Egoal.ShortMessage.Huyi
{
    public static class HuyiShortMessageModule
    {
        public static void AddHuyiShortMessage(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IShortMessageService, HuyiShortMessageService>();

            services.Configure<MessageOptions>(configuration);
        }
    }
}
