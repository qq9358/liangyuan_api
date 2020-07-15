using Egoal.Extensions;
using Egoal.Threading.BackgroundWorkers;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Egoal.Redis
{
    public class RewriteAofWorker : PeriodicBackgroundWorkerBase
    {
        private readonly RedisManager _redisManager;

        public RewriteAofWorker(
            ILogger<RewriteAofWorker> logger,
            RedisManager redisManager)
            : base(logger)
        {
            _redisManager = redisManager;

            Period = TimeSpan.FromMinutes(10);
        }

        protected override async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            var now = DateTime.Now;
            if (now.Hour != 3) return;

            var today = now.ToString(DateTimeExtensions.DateFormat);

            var database = _redisManager.GetDatabase();
            var key = "LastRewriteDate";
            var lastRewriteDate = await database.StringGetAsync(key);
            if (lastRewriteDate == today) return;

            var connection = _redisManager.GetConnection();
            var endPoints = connection.GetEndPoints();
            foreach (var endPoint in endPoints)
            {
                var server = connection.GetServer(endPoint);

                await server.ExecuteAsync("bgrewriteaof");
            }

            await database.StringSetAsync(key, today);
        }
    }
}
