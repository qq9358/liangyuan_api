using Microsoft.Extensions.Caching.Memory;
using System;

namespace Egoal.Threading.RateLimit
{
    public class RateLimiterManager : IRateLimiterManager
    {
        private readonly IMemoryCache _memoryCache;

        public RateLimiterManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public bool TryAcquire(string rule, double permitsPerSecond, int permits = 1, double timeout = 0)
        {
            var rateLimiter = _memoryCache.GetOrCreate(rule, entry =>
            {
                if (timeout > 0)
                {
                    entry.SlidingExpiration = TimeSpan.FromSeconds(timeout);
                }

                return RateLimiter.Create(permitsPerSecond);
            });

            return rateLimiter.TryAcquire(permits);
        }
    }
}
