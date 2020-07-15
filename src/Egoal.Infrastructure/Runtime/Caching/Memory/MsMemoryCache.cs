using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Egoal.Runtime.Caching.Memory
{
    public class MsMemoryCache : CacheBase
    {
        private IMemoryCache _memoryCache;
        private readonly IServiceProvider _serviceProvider;

        public MsMemoryCache(
            ILogger<MsMemoryCache> logger,
            IMemoryCache memoryCache,
            IServiceProvider serviceProvider)
            : base(logger)
        {
            _memoryCache = memoryCache;
            _serviceProvider = serviceProvider;
        }

        protected override object GetOrDefault(string key)
        {
            return _memoryCache.Get(GetFullKey(key));
        }

        public override void Set(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            if (value == null)
            {
                throw new TmsException("Can not insert null values to the cache!");
            }

            var fullKey = GetFullKey(key);

            if (absoluteExpireTime != null)
            {
                _memoryCache.Set(fullKey, value, DateTimeOffset.Now.Add(absoluteExpireTime.Value));
            }
            else if (slidingExpireTime != null)
            {
                _memoryCache.Set(fullKey, value, slidingExpireTime.Value);
            }
            else if (DefaultAbsoluteExpireTime != null)
            {
                _memoryCache.Set(fullKey, value, DateTimeOffset.Now.Add(DefaultAbsoluteExpireTime.Value));
            }
            else
            {
                _memoryCache.Set(fullKey, value, DefaultSlidingExpireTime);
            }
        }

        public override void Remove(string key)
        {
            _memoryCache.Remove(GetFullKey(key));
        }

        public override void Clear()
        {
            _memoryCache.Dispose();
            _memoryCache = _serviceProvider.GetRequiredService<IMemoryCache>();
        }

        public override void Dispose()
        {
            _memoryCache.Dispose();
            base.Dispose();
        }
    }
}
