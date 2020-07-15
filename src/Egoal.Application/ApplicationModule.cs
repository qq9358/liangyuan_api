using Egoal.Authorization;
using Egoal.AutoMapper;
using Egoal.BackgroundJobs;
using Egoal.Dependency;
using Egoal.Domain.Uow;
using Egoal.Events.Bus;
using Egoal.Messages;
using Egoal.Orders;
using Egoal.Payment;
using Egoal.Scenics;
using Egoal.Settings;
using Egoal.Stadiums;
using Egoal.Tickets;
using Egoal.TicketTypes;
using Egoal.WeChat.Message;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Egoal
{
    public static class ApplicationModule
    {
        public static void AddApplicationModule(this IServiceCollection services, IConfiguration configuration)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            services.RegisterAssemblyByConvention(assembly);

            services.AddScoped<IBackgroundJobStore, BackgroundJobService>();
            services.AddScoped<ITemplateStore, WeChatMessageTemplateStore>();

            services.Configure<TokenOptions>(configuration);
            services.Configure<ParkOptions>(configuration);
            services.Configure<OrderOptions>(configuration);
            services.Configure<TicketSaleOptions>(configuration);
            services.Configure<ScenicOptions>(configuration);
            services.Configure<PayOptions>(configuration);
            services.Configure<StadiumOptions>(configuration);
            services.Configure<TicketTypeOptions>(configuration);

            CustomMapper.CreateAssemblyMappings(assembly);
        }

        public static void Start(IServiceProvider serviceProvider)
        {
            RegisterEventHandler(serviceProvider);

            using (var scope = serviceProvider.CreateScope())
            {
                var unitOfWorkManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
                using (var uow = unitOfWorkManager.Begin())
                {
                    var payTypeService = scope.ServiceProvider.GetRequiredService<IPayTypeAppService>();
                    payTypeService.EnsureSystemPayTypeAsync().Wait();

                    var settingAppService = scope.ServiceProvider.GetRequiredService<ISettingAppService>();
                    settingAppService.ConfigOptionsAsync().Wait();

                    uow.Complete();
                }
            }
        }

        private static void RegisterEventHandler(IServiceProvider serviceProvider)
        {
            var eventBus = serviceProvider.GetRequiredService<IEventBus>();

            eventBus.RegisterEventHandler(Assembly.GetExecutingAssembly(), serviceProvider);
        }
    }
}
