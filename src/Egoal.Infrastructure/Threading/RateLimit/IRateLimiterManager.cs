namespace Egoal.Threading.RateLimit
{
    public interface IRateLimiterManager
    {
        bool TryAcquire(string rule, double permitsPerSecond, int permits = 1, double timeout = 0);
    }
}
