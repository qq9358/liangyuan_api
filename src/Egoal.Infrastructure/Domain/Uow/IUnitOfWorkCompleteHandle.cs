using System;
using System.Threading.Tasks;

namespace Egoal.Domain.Uow
{
    public interface IUnitOfWorkCompleteHandle : IDisposable
    {
        void Complete();
        Task CompleteAsync();
    }
}
