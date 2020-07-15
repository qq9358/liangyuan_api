namespace Egoal.Runtime.Caching
{
    public static class CacheManagerExtensions
    {
        public static ITypedCache<TKey, TValue> GetCache<TKey, TValue>(this ICacheManager cacheManager, string name)
        {
            return new TypedCacheWrapper<TKey, TValue>(cacheManager.GetCache(name));
        }
    }
}
