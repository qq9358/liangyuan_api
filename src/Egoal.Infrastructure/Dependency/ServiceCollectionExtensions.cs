using Egoal.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Egoal.Dependency
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterAssemblyByConvention(this IServiceCollection services, Assembly assembly)
        {
            var types = assembly.GetTypes();

            RegisterServicesByConvention<ITransientDependency>(services, types);

            RegisterServicesByConvention<IScopedDependency>(services, types);

            RegisterServicesByConvention<ISingletonDependency>(services, types);
        }

        private static void RegisterServicesByConvention<TLifestyle>(IServiceCollection services, Type[] types)
        {
            var lifetimeType = typeof(TLifestyle);

            var implementationTypes = types.Where(t => t.IsClass && lifetimeType.IsAssignableFrom(t)).ToList();
            foreach (var implType in implementationTypes)
            {
                var interfaces = implType.GetInterfaces().Where(i => i.Name == $"I{implType.Name}").ToList();
                if (interfaces.IsNullOrEmpty())
                {
                    services.AddServiceIfNotContains<TLifestyle>(implType);
                }
                else
                {
                    foreach (var @interface in interfaces)
                    {
                        services.AddServiceIfNotContains<TLifestyle>(@interface, implType);
                    }
                }
            }
        }

        public static void RegisterBasedOn<TBase, TLifestyle>(this IServiceCollection services, Assembly assembly)
        {
            var baseType = typeof(TBase);

            var types = assembly.GetTypes().Where(t => baseType.IsAssignableFrom(t));
            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces().Where(i => baseType.IsAssignableFrom(i));
                if (interfaces.IsNullOrEmpty())
                {
                    services.AddServiceIfNotContains<TLifestyle>(type);
                }
                else
                {
                    foreach (var @interface in interfaces)
                    {
                        if (@interface == baseType) continue;

                        services.AddServiceIfNotContains<TLifestyle>(@interface, type);
                    }
                }
            }
        }

        public static void AddServiceIfNotContains<TLifestyle>(this IServiceCollection services, Type serviceType)
        {
            if (services.IsRegistered(serviceType))
            {
                return;
            }

            var lifetimeType = typeof(TLifestyle);
            if (lifetimeType == typeof(ITransientDependency))
            {
                services.AddTransient(serviceType);
            }
            else if (lifetimeType == typeof(IScopedDependency))
            {
                services.AddScoped(serviceType);
            }
            else if (lifetimeType == typeof(ISingletonDependency))
            {
                services.AddSingleton(serviceType);
            }
        }

        public static void AddServiceIfNotContains<TLifestyle>(this IServiceCollection services, Type serviceType, Type implementationType)
        {
            if (services.IsRegistered(serviceType))
            {
                return;
            }

            var lifetimeType = typeof(TLifestyle);
            if (lifetimeType == typeof(ITransientDependency))
            {
                services.AddTransient(serviceType, implementationType);
            }
            else if (lifetimeType == typeof(IScopedDependency))
            {
                services.AddScoped(serviceType, implementationType);
            }
            else if (lifetimeType == typeof(ISingletonDependency))
            {
                services.AddSingleton(serviceType, implementationType);
            }
        }

        public static bool IsRegistered(this IServiceCollection services, Type type)
        {
            return services.Any(s => s.ServiceType == type);
        }
    }
}
