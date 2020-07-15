using Egoal.Dependency;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Egoal
{
    public static class DomainModule
    {
        public static void AddDomainModule(this IServiceCollection services)
        {
            services.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
