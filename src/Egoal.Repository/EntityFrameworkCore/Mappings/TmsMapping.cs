using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace Egoal.EntityFrameworkCore.Mappings
{
    public class TmsMapping
    {
        public static void ApplyConfiguration(ModelBuilder modelBuilder)
        {
            var method = modelBuilder.GetType().GetMethods()
                .Where(m =>
                m.Name == "ApplyConfiguration" &&
                m.GetParameters().Any(p => p.ParameterType.IsGenericType && p.ParameterType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
                .First();

            var mapTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsClass && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
                .ToList();

            foreach (var mapType in mapTypes)
            {
                var mapInterface = mapType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));
                var applyConfiguration = method.MakeGenericMethod(mapInterface.GetGenericArguments()[0]);

                applyConfiguration.Invoke(modelBuilder, new object[] { Activator.CreateInstance(mapType) });
            }
        }
    }
}
