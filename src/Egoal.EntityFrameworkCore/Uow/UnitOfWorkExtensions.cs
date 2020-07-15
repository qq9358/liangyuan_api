using Egoal.Domain.Uow;
using Egoal.Extensions;
using Microsoft.EntityFrameworkCore;
using System;

namespace Egoal.Uow
{
    public static class UnitOfWorkExtensions
    {
        public static TDbContext GetDbContext<TDbContext>(this IActiveUnitOfWork unitOfWork)
            where TDbContext : DbContext
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork");
            }

            if (!(unitOfWork is EfCoreUnitOfWork))
            {
                throw new ArgumentException("unitOfWork is not type of " + typeof(EfCoreUnitOfWork).FullName, "unitOfWork");
            }

            return unitOfWork.As<EfCoreUnitOfWork>().GetOrCreateDbContext<TDbContext>();
        }
    }
}
