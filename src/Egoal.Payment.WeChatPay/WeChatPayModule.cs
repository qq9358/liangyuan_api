using Microsoft.Extensions.DependencyInjection;

namespace Egoal.Payment.WeChatPay
{
    public static class WeChatPayModule
    {
        public static void AddWeChatPayModule(this IServiceCollection services)
        {
            services.AddScoped<WeChatPayApi>();
            services.AddScoped<PayService>();
        }
    }
}
