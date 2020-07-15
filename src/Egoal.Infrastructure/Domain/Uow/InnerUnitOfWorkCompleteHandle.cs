using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Egoal.Domain.Uow
{
    internal class InnerUnitOfWorkCompleteHandle : IUnitOfWorkCompleteHandle
    {
        public const string DidNotCallCompleteMethodExceptionMessage = "没有调用Complete方法";

        private volatile bool _isCompleteCalled;
        private volatile bool _isDisposed;

        public void Complete()
        {
            _isCompleteCalled = true;
        }

        public Task CompleteAsync()
        {
            _isCompleteCalled = true;
            return Task.FromResult(0);
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;

            if (!_isCompleteCalled)
            {
                if (HasException())
                {
                    return;
                }

                throw new TmsException(DidNotCallCompleteMethodExceptionMessage);
            }
        }

        private static bool HasException()
        {
            try
            {
                return Marshal.GetExceptionCode() != 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
