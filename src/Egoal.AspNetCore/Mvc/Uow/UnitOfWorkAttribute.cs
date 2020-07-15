using Egoal.Domain.Uow;
using System;
using System.Transactions;

namespace Egoal.Mvc.Uow
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
    public class UnitOfWorkAttribute : Attribute
    {
        public TransactionScopeOption? Scope { get; set; }
        public bool? IsTransactional { get; set; }
        public TimeSpan? Timeout { get; set; }
        public System.Data.IsolationLevel? IsolationLevel { get; set; }
        public bool IsDisabled { get; set; }

        public UnitOfWorkAttribute()
        {

        }

        public UnitOfWorkAttribute(bool isTransactional)
        {
            IsTransactional = isTransactional;
        }

        public UnitOfWorkAttribute(int timeout)
        {
            Timeout = TimeSpan.FromMilliseconds(timeout);
        }

        public UnitOfWorkAttribute(bool isTransactional, int timeout)
        {
            IsTransactional = isTransactional;
            Timeout = TimeSpan.FromMilliseconds(timeout);
        }

        public UnitOfWorkAttribute(System.Data.IsolationLevel isolationLevel)
        {
            IsTransactional = true;
            IsolationLevel = isolationLevel;
        }

        public UnitOfWorkAttribute(System.Data.IsolationLevel isolationLevel, int timeout)
        {
            IsTransactional = true;
            IsolationLevel = isolationLevel;
            Timeout = TimeSpan.FromMilliseconds(timeout);
        }

        public UnitOfWorkAttribute(TransactionScopeOption scope)
        {
            IsTransactional = true;
            Scope = scope;
        }

        public UnitOfWorkAttribute(TransactionScopeOption scope, bool isTransactional)
        {
            Scope = scope;
            IsTransactional = isTransactional;
        }

        public UnitOfWorkAttribute(TransactionScopeOption scope, int timeout)
        {
            IsTransactional = true;
            Scope = scope;
            Timeout = TimeSpan.FromMilliseconds(timeout);
        }

        internal UnitOfWorkOptions CreateOptions()
        {
            return new UnitOfWorkOptions
            {
                IsTransactional = IsTransactional,
                IsolationLevel = IsolationLevel,
                Timeout = Timeout,
                Scope = Scope
            };
        }
    }
}
