using System;

namespace Egoal.Domain.Uow
{
    public class UnitOfWorkFailedEventArgs : EventArgs
    {
        public Exception Exception { get; private set; }
        public UnitOfWorkFailedEventArgs(Exception exception)
        {
            Exception = exception;
        }
    }
}
