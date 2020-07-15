using Egoal.Dependency;
using Egoal.Events.Bus;
using Egoal.WeChat.OAuth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Egoal.WeChat
{
    public static class WeChatModule
    {
        public static void AddWeChatModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            services.AddHostedService<RefreshAccessTokenWorker>();

            services.Configure<WeChatOptions>(configuration);
        }

        public static void Start(IServiceProvider serviceProvider)
        {
            RegisterEventHandler(serviceProvider);
        }

        private static void RegisterEventHandler(IServiceProvider serviceProvider)
        {
            var eventBus = serviceProvider.GetRequiredService<IEventBus>();

            eventBus.RegisterEventHandler(Assembly.GetExecutingAssembly(), serviceProvider);
        }
    }
}
