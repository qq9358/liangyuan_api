using System;

namespace Egoal.Threading.Lock
{
    public interface ILock : IDisposable
    {
        string Resource { get; }
        string LockId { get; }
        bool IsAcquired { get; }
    }
}
