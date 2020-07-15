using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.Runtime.Caching
{
    public interface ITypedCache<TKey, TValue> : IDisposable
    {
        string Name { get; }
        TimeSpan DefaultSlidingExpireTime { get; set; }
        ICache InternalCache { get; }
        TValue GetOrCreate(TKey key, Func<CacheEntryOptions, TValue> factory);
        Task<TValue> GetOrCreateAsync(TKey key, Func<CacheEntryOptions, Task<TValue>> factory);
        void Set(TKey key, TValue value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);
        void Set(KeyValuePair<TKey, TValue>[] pairs, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);
        Task SetAsync(TKey key, TValue value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);
        Task SetAsync(KeyValuePair<TKey, TValue>[] pairs, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);
        void Remove(TKey key);
        Task RemoveAsync(TKey key);
        void Clear();
        Task ClearAsync();
    }
}
