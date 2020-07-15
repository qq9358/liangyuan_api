using Egoal.AutoMapper;
using Egoal.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text;

namespace Egoal.Invoice.GuangDongBaiWangJiuBin
{
    public static class GuangDongBaiWangJiuBinModule
    {
        public static void AddGuangDongBaiWangJiuBinInvoice(this IServiceCollection services, IConfiguration configuration)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddTransient<InvoiceService>();

            services.Configure<BaiWangOptions>(configuration);

            if (!configuration["XSF_NSRSBH"].IsNullOrEmpty() && !configuration["XSF_NSRMY"].IsNullOrEmpty())
            {
                services.AddHostedService<RefreshAccessTokenWorker>();
            }

            CustomMapper.CreateAssemblyMappings(Assembly.GetExecutingAssembly());
        }
    }
}
