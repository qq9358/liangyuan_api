using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Egoal.Domain.Uow;
using Egoal.Uow;

namespace Egoal
{
    public static class EntityFrameworkCoreModule
    {
        public static void AddEntityFrameworkCoreModule(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, EfCoreUnitOfWork>();
            services.AddTransient(typeof(IDbContextProvider<>), typeof(UnitOfWorkDbContextProvider<>));
        }
    }
}
