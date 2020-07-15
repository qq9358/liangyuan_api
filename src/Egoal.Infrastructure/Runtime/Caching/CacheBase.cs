using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Egoal.Runtime.Caching
{
    public abstract class CacheBase : ICache
    {
        public string Name { get; set; }
        public TimeSpan DefaultSlidingExpireTime { get; set; } = TimeSpan.FromHours(1);
        public TimeSpan? DefaultAbsoluteExpireTime { get; set; }

        private readonly SemaphoreSlim _locker = new SemaphoreSlim(1);

        private readonly ILogger _logger;

        protected CacheBase(ILogger logger)
        {
            _logger = logger;
        }

        public virtual object GetOrCreate(string key, Func<CacheEntryOptions, object> factory)
        {
            object item = null;

            try
            {
                item = GetOrDefault(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
            }

            if (item == null)
            {
                try
                {
                    _locker.Wait();

                    try
                    {
                        item = GetOrDefault(key);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.ToString(), ex);
                    }

                    if (item == null)
                    {
                        var entry = new CacheEntryOptions();
                        entry.SlidingExpireTime = DefaultSlidingExpireTime;

                        item = factory(entry);

                        if (item == null)
                        {
                            return null;
                        }

                        try
                        {
                            Set(key, item, entry.SlidingExpireTime, entry.AbsoluteExpireTime);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.ToString(), ex);
                        }
                    }
                }
                finally
                {
                    _locker.Release();
                }
            }

            return item;
        }

        public virtual async Task<object> GetOrCreateAsync(string key, Func<CacheEntryOptions, Task<object>> factory)
        {
            object item = null;

            try
            {
                item = await GetOrDefaultAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
            }

            if (item == null)
            {
                try
                {
                    await _locker.WaitAsync();

                    try
                    {
                        item = await GetOrDefaultAsync(key);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.ToString(), ex);
                    }

                    if (item == null)
                    {
                        var entry = new CacheEntryOptions();
                        entry.SlidingExpireTime = DefaultSlidingExpireTime;

                        item = await factory(entry);

                        if (item == null)
                        {
                            return null;
                        }

                        try
                        {
                            await SetAsync(key, item, entry.SlidingExpireTime, entry.AbsoluteExpireTime);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.ToString(), ex);
                        }
                    }
                }
                finally
                {
                    _locker.Release();
                }
            }

            return item;
        }

        protected virtual Task<object> GetOrDefaultAsync(string key)
        {
            return Task.Run(() => GetOrDefault(key));
        }

        protected abstract object GetOrDefault(string key);

        public abstract void Set(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        public virtual void Set(KeyValuePair<string, object>[] pairs, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            foreach (var pair in pairs)
            {
                Set(pair.Key, pair.Value, slidingExpireTime, absoluteExpireTime);
            }
        }

        public virtual Task SetAsync(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            return Task.Run(() => Set(key, value, slidingExpireTime, absoluteExpireTime));
        }

        public virtual Task SetAsync(KeyValuePair<string, object>[] pairs, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            return Task.WhenAll(pairs.Select(p => SetAsync(p.Key, p.Value, slidingExpireTime, absoluteExpireTime)));
        }

        public abstract void Remove(string key);

        public virtual Task RemoveAsync(string key)
        {
            return Task.Run(() => Remove(key));
        }

        public abstract void Clear();

        public virtual Task ClearAsync()
        {
            return Task.Run(() => Clear());
        }

        protected virtual string GetFullKey(string key)
        {
            return $"{Name}:{key}";
        }

        public virtual void Dispose()
        {

        }
    }
}
