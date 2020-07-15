using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Egoal.Runtime.Caching
{
    public abstract class CacheManagerBase : ICacheManager
    {
        protected readonly ConcurrentDictionary<string, ICache> Caches;

        protected CacheManagerBase()
        {
            Caches = new ConcurrentDictionary<string, ICache>();
        }

        public IReadOnlyList<ICache> GetAllCaches()
        {
            return Caches.Values.ToImmutableList();
        }

        public virtual ICache GetCache(string name)
        {
            Check.NotNull(name, nameof(name));

            return Caches.GetOrAdd(name, cacheName =>
            {
                var cache = CreateCacheImplementation(cacheName);

                return cache;
            });
        }

        protected abstract ICache CreateCacheImplementation(string name);

        public virtual void Dispose()
        {
            DisposeCaches();
            Caches.Clear();
        }

        protected virtual void DisposeCaches()
        {
            foreach (var cache in Caches.Values)
            {
                cache.Dispose();
            }
        }
    }
}
