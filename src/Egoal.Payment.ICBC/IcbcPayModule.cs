using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Egoal.Payment.IcbcPay
{
    public static class IcbcPayModule
    {
        public static void AddIcbcPayModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<PayService>();
            services.AddScoped<IcbcPayApi>();
            services.AddScoped<IcbcSignature>();

            services.Configure<IcbcPayOptions>(configuration);
        }
    }
}
