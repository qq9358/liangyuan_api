using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.Runtime.Caching
{
    public interface ICache : IDisposable
    {
        string Name { get; set; }

        TimeSpan DefaultSlidingExpireTime { get; set; }

        TimeSpan? DefaultAbsoluteExpireTime { get; set; }

        object GetOrCreate(string key, Func<CacheEntryOptions, object> factory);

        Task<object> GetOrCreateAsync(string key, Func<CacheEntryOptions, Task<object>> factory);

        void Set(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        void Set(KeyValuePair<string, object>[] pairs, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        Task SetAsync(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        Task SetAsync(KeyValuePair<string, object>[] pairs, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        void Remove(string key);

        Task RemoveAsync(string key);

        void Clear();

        Task ClearAsync();
    }
}
