using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Egoal.Payment.Alipay
{
    public static class AlipayModule
    {
        public static void AddAlipayModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<PayService>();
            services.AddScoped<AlipayApi>();

            services.Configure<AlipayOptions>(configuration);
        }
    }
}
