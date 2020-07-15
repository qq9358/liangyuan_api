using Egoal.Domain.Uow;
using Egoal.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Egoal.Uow
{
    public class EfCoreUnitOfWork : UnitOfWorkBase
    {
        private IServiceProvider _serviceProvider;

        protected DbContext ActiveDbContext { get; private set; }

        public EfCoreUnitOfWork(
            IUnitOfWorkDefaultOptions defaultOptions,
            ISession session,
            IServiceProvider serviceProvider)
            : base(defaultOptions, session)
        {
            _serviceProvider = serviceProvider;
        }

        protected override void CompleteUow()
        {
            SaveChanges();
            CommitTransaction();
        }

        public override void SaveChanges()
        {
            ActiveDbContext?.SaveChanges();
        }

        protected override async Task CompleteUowAsync()
        {
            await SaveChangesAsync();
            CommitTransaction();
        }

        public override async Task SaveChangesAsync()
        {
            if (ActiveDbContext == null)
            {
                return;
            }

            await ActiveDbContext.SaveChangesAsync();
        }

        private void CommitTransaction()
        {
            if (Options.IsTransactional == true && ActiveDbContext?.Database?.CurrentTransaction != null)
            {
                ActiveDbContext?.Database?.CommitTransaction();
            }
        }

        public virtual TDbContext GetOrCreateDbContext<TDbContext>()
            where TDbContext : DbContext
        {
            if (ActiveDbContext == null)
            {
                ActiveDbContext = _serviceProvider.GetRequiredService<TDbContext>();

                if (Options.IsTransactional == true)
                {
                    ActiveDbContext.Database.BeginTransaction(Options.IsolationLevel ?? IsolationLevel.ReadCommitted);
                }
            }

            return (TDbContext)ActiveDbContext;
        }

        protected override void DisposeUow()
        {
            if (Options.IsTransactional == true)
            {
                ActiveDbContext?.Database?.CurrentTransaction?.Dispose();
            }

            ActiveDbContext?.Database?.GetDbConnection()?.Close();

            ActiveDbContext?.Dispose();

            ActiveDbContext = null;
        }
    }
}
