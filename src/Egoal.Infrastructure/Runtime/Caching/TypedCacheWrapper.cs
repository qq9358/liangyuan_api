using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Egoal.Runtime.Caching
{
    public class TypedCacheWrapper<TKey, TValue> : ITypedCache<TKey, TValue>
    {
        public string Name
        {
            get { return InternalCache.Name; }
        }

        public TimeSpan DefaultSlidingExpireTime
        {
            get { return InternalCache.DefaultSlidingExpireTime; }
            set { InternalCache.DefaultSlidingExpireTime = value; }
        }

        public TimeSpan? DefaultAbsoluteExpireTime
        {
            get { return InternalCache.DefaultAbsoluteExpireTime; }
            set { InternalCache.DefaultAbsoluteExpireTime = value; }
        }

        public ICache InternalCache { get; private set; }

        public TypedCacheWrapper(ICache internalCache)
        {
            InternalCache = internalCache;
        }

        public TValue GetOrCreate(TKey key, Func<CacheEntryOptions, TValue> factory)
        {
            return (TValue)InternalCache.GetOrCreate(key.ToString(), entry => factory(entry));
        }

        public async Task<TValue> GetOrCreateAsync(TKey key, Func<CacheEntryOptions, Task<TValue>> factory)
        {
            return (TValue)(await InternalCache.GetOrCreateAsync(key.ToString(), async entry => await factory(entry)));
        }

        public void Set(TKey key, TValue value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            InternalCache.Set(key.ToString(), value, slidingExpireTime, absoluteExpireTime);
        }

        public void Set(KeyValuePair<TKey, TValue>[] pairs, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            var stringPairs = pairs.Select(p => new KeyValuePair<string, object>(p.Key.ToString(), p.Value));
            InternalCache.Set(stringPairs.ToArray(), slidingExpireTime, absoluteExpireTime);
        }

        public Task SetAsync(TKey key, TValue value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            return InternalCache.SetAsync(key.ToString(), value, slidingExpireTime, absoluteExpireTime);
        }

        public Task SetAsync(KeyValuePair<TKey, TValue>[] pairs, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            var stringPairs = pairs.Select(p => new KeyValuePair<string, object>(p.Key.ToString(), p.Value));
            return InternalCache.SetAsync(stringPairs.ToArray(), slidingExpireTime, absoluteExpireTime);
        }

        public void Remove(TKey key)
        {
            InternalCache.Remove(key.ToString());
        }

        public Task RemoveAsync(TKey key)
        {
            return InternalCache.RemoveAsync(key.ToString());
        }

        public void Clear()
        {
            InternalCache.Clear();
        }

        public Task ClearAsync()
        {
            return InternalCache.ClearAsync();
        }

        public void Dispose()
        {
            InternalCache.Dispose();
        }
    }
}
