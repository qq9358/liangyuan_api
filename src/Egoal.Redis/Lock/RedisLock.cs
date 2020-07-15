using Egoal.Threading.Lock;
using RedLockNet;

namespace Egoal.Redis.Lock
{
    public class RedisLock : ILock
    {
        private readonly IRedLock _redLock;

        public string Resource { get; }
        public string LockId { get; }
        public bool IsAcquired { get; }

        public RedisLock(IRedLock redLock)
        {
            _redLock = redLock;

            Resource = _redLock.Resource;
            LockId = _redLock.LockId;
            IsAcquired = _redLock.IsAcquired;
        }

        public void Dispose()
        {
            _redLock.Dispose();
        }
    }
}
