using Egoal.Dependency;
using Egoal.Domain.Repositories;
using Egoal.EntityFrameworkCore;
using Egoal.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Egoal
{
    public static class RepositoryModule
    {
        public static void AddDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<TmsDbContext>(
                builder => builder.UseSqlServer(connectionString, sqlBuilder => OptionsBuilderHelper.BuildSqlServerOptions(connectionString, sqlBuilder)),
                ServiceLifetime.Transient,
                ServiceLifetime.Singleton);
        }

        public static void AddRepositoryModule(this IServiceCollection services)
        {
            new EfGenericRepositoryRegistrar().RegisterForDbContext(typeof(TmsDbContext), services, EfCoreAutoRepositoryTypes.Default);

            var assembly = Assembly.GetExecutingAssembly();

            services.RegisterAssemblyByConvention(assembly);

            services.RegisterBasedOn<IAppRepository, IScopedDependency>(assembly);
        }
    }
}
