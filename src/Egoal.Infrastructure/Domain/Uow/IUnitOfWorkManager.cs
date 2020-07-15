using System.Transactions;

namespace Egoal.Domain.Uow
{
    public interface IUnitOfWorkManager
    {
        IActiveUnitOfWork Current { get; }
        IUnitOfWorkCompleteHandle Begin();
        IUnitOfWorkCompleteHandle Begin(TransactionScopeOption scope);
        IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options);
    }
}
