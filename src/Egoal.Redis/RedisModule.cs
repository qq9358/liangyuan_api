using Egoal.Redis.Lock;
using Egoal.Threading.Lock;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Egoal.Redis
{
    public static class RedisModule
    {
        public static void AddRedisLock(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<RedisManager>();
            services.AddSingleton<IDistributedLockFactory, RedisDistributedLockFactory>();

            services.Configure<RedisOptions>(configuration.GetSection("Redis"));

            services.AddHostedService<RewriteAofWorker>();
        }

        public static void Start(IServiceProvider serviceProvider)
        {
            var redisManager = serviceProvider.GetRequiredService<RedisManager>();
            redisManager.InitConfigAsync().Wait();
        }

        public static void Stop(IServiceProvider serviceProvider)
        {
            var lockFactory = serviceProvider.GetRequiredService<IDistributedLockFactory>();
            lockFactory.Dispose();

            var redisManager = serviceProvider.GetRequiredService<RedisManager>();
            redisManager.Dispose();
        }
    }
}
