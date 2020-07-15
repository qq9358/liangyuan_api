using Egoal.Domain.Entities;
using Egoal.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Egoal.EntityFrameworkCore
{
    public class EfCoreRepositoryBase<TEntity, TPrimaryKey> : EfCoreRepositoryBase<TmsDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        public EfCoreRepositoryBase(IServiceProvider serviceProvider)
            : base(serviceProvider.GetRequiredService<IDbContextProvider<TmsDbContext>>())
        {

        }
    }

    public class EfCoreRepositoryBase<TEntity> : EfCoreRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        public EfCoreRepositoryBase(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }
    }
}
