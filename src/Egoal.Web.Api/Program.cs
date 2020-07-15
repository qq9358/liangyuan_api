using Egoal.Settings;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NLog.Web;
using System;
using System.IO;

namespace Egoal.Web.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");

                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"程序异常退出{ex.StackTrace}");

                throw;
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostContext, config) =>
            {
                var filePath = Path.Combine(hostContext.HostingEnvironment.ContentRootPath, "appsettings.json");
                config.AddDbConfiguration(filePath);
            })
            .UseStartup<Startup>()
            .UseNLog();
    }
}
