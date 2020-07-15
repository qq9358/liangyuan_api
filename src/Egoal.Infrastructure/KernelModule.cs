using Egoal.BackgroundJobs;
using Egoal.Domain.Uow;
using Egoal.Events.Bus;
using Egoal.Events.Bus.Entities;
using Egoal.Events.Bus.Handlers;
using Egoal.Events.Bus.Handlers.Internals;
using Egoal.Logging;
using Egoal.Runtime.Caching;
using Egoal.Runtime.Caching.Memory;
using Egoal.Runtime.Session;
using Egoal.Threading.RateLimit;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Egoal
{
    public static class KernelModule
    {
        public static void AddKernelModule(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWorkManager, UnitOfWorkManager>();
            services.AddTransient<ICurrentUnitOfWorkProvider, AsyncLocalCurrentUnitOfWorkProvider>();
            var unitOfWorkDefaultOptions = new UnitOfWorkDefaultOptions();
            unitOfWorkDefaultOptions.RegisterFilter(DataFilters.SoftDelete, true);
            services.AddSingleton(typeof(IUnitOfWorkDefaultOptions), unitOfWorkDefaultOptions);

            services.AddSingleton<ISession, ClaimsSession>();

            services.AddSingleton<LogHelper>();

            services.AddTransient<IEntityChangeEventHelper, EntityChangeEventHelper>();
            services.AddTransient(typeof(IEventHandler<>), typeof(ActionEventHandler<>));
            services.AddTransient(typeof(IAsyncEventHandler<>), typeof(AsyncActionEventHandler<>));
            services.AddSingleton<IEventBus, EventBus>();

            services.AddSingleton<IRateLimiterManager, RateLimiterManager>();
        }

        public static void AddBackgroundJob(this IServiceCollection services)
        {
            services.AddHostedService<BackgroundJobManager>();
        }

        public static IServiceCollection AddMsMemoryCache(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Transient<IMemoryCache, MemoryCache>());

            services.AddSingleton<ICache, MsMemoryCache>();
            services.AddSingleton<ICacheManager, MemoryCacheManager>();

            return services;
        }
    }
}
