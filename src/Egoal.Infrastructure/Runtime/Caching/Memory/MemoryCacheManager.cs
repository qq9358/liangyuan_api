using Microsoft.Extensions.DependencyInjection;
using System;

namespace Egoal.Runtime.Caching.Memory
{
    public class MemoryCacheManager : CacheManagerBase
    {
        private readonly IServiceProvider _serviceProvider;

        public MemoryCacheManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override ICache CreateCacheImplementation(string name)
        {
            var cache = _serviceProvider.GetRequiredService<ICache>();
            cache.Name = name;

            return cache;
        }
    }
}
