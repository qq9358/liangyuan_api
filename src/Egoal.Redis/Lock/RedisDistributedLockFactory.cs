using Egoal.Threading.Lock;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Egoal.Redis.Lock
{
    public class RedisDistributedLockFactory : Threading.Lock.IDistributedLockFactory
    {
        private readonly RedisManager _redisManager;
        private readonly Lazy<RedLockFactory> _redLockFactory;

        public TimeSpan DefaultExpiryTime { get; set; } = TimeSpan.FromMinutes(5);
        public TimeSpan DefaultRetryTime { get; set; } = TimeSpan.FromMilliseconds(500);

        public RedisDistributedLockFactory(RedisManager redisManager)
        {
            _redisManager = redisManager;
            _redLockFactory = new Lazy<RedLockFactory>(CreateRedLockFactory);
        }

        private RedLockFactory CreateRedLockFactory()
        {
            var endPoints = _redisManager.GetConnection().GetEndPoints()
                .Select(e => new RedLockEndPoint { EndPoint = e })
                .ToList();

            return RedLockFactory.Create(endPoints);
        }

        public Task<ILock> LockAsync(string resource, bool block = true, CancellationToken? cancellationToken = null)
        {
            return LockAsync(resource, DefaultExpiryTime, block, cancellationToken);
        }

        public async Task<ILock> LockAsync(string resource, TimeSpan expiryTime, bool block = true, CancellationToken? cancellationToken = null)
        {
            IRedLock redLock = null;
            if (block)
            {
                redLock = await _redLockFactory.Value.CreateLockAsync(resource, expiryTime, expiryTime, DefaultRetryTime, cancellationToken);
            }
            else
            {
                redLock = await _redLockFactory.Value.CreateLockAsync(resource, expiryTime);
            }

            return new RedisLock(redLock);
        }

        public void Dispose()
        {
            _redLockFactory.Value.Dispose();
        }
    }
}
