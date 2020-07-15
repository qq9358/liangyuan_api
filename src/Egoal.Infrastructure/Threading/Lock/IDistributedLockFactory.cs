using System;
using System.Threading;
using System.Threading.Tasks;

namespace Egoal.Threading.Lock
{
    public interface IDistributedLockFactory : IDisposable
    {
        TimeSpan DefaultExpiryTime { get; set; }
        TimeSpan DefaultRetryTime { get; set; }
        Task<ILock> LockAsync(string resource, bool block = true, CancellationToken? cancellationToken = null);
        Task<ILock> LockAsync(string resource, TimeSpan expiryTime, bool block = true, CancellationToken? cancellationToken = null);
    }
}
