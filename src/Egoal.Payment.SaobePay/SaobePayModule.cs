using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Egoal.Payment.SaobePay
{
    public static class SaobePayModule
    {
        public static void AddSaobePayModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<SaobePayApi>();
            services.AddScoped<PayService>();

            services.Configure<SaobePayOptions>(configuration);
        }
    }
}
