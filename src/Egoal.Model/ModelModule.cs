using Egoal.AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Egoal
{
    public static class ModelModule
    {
        public static void AddModelModule(this IServiceCollection services)
        {
            CustomMapper.CreateAssemblyMappings(Assembly.GetExecutingAssembly());
        }
    }
}
