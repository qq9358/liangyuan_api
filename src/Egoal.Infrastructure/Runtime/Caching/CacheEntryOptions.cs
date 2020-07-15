using System;

namespace Egoal.Runtime.Caching
{
    public class CacheEntryOptions
    {
        public TimeSpan? SlidingExpireTime { get; set; }
        public TimeSpan? AbsoluteExpireTime { get; set; }
    }
}