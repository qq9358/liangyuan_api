using Egoal.Dependency;
using Egoal.Domain.Entities;
using Egoal.Domain.Repositories;
using Egoal.Reflection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Egoal.Repositories
{
    public class EfGenericRepositoryRegistrar
    {
        private readonly IDbContextEntityFinder _dbContextEntityFinder;

        public EfGenericRepositoryRegistrar()
        {
            _dbContextEntityFinder = new EfCoreDbContextEntityFinder();
        }

        public void RegisterForDbContext(
            Type dbContextType,
            IServiceCollection services,
            AutoRepositoryTypesAttribute defaultAutoRepositoryTypesAttribute)
        {
            var autoRepositoryAttr = dbContextType.GetTypeInfo().GetSingleAttributeOrNull<AutoRepositoryTypesAttribute>() ?? defaultAutoRepositoryTypesAttribute;

            RegisterForDbContext(
                dbContextType,
                services,
                autoRepositoryAttr.RepositoryInterface,
                autoRepositoryAttr.RepositoryInterfaceWithPrimaryKey,
                autoRepositoryAttr.RepositoryImplementation,
                autoRepositoryAttr.RepositoryImplementationWithPrimaryKey
            );

            if (autoRepositoryAttr.WithDefaultRepositoryInterfaces)
            {
                RegisterForDbContext(
                    dbContextType,
                    services,
                    defaultAutoRepositoryTypesAttribute.RepositoryInterface,
                    defaultAutoRepositoryTypesAttribute.RepositoryInterfaceWithPrimaryKey,
                    autoRepositoryAttr.RepositoryImplementation,
                    autoRepositoryAttr.RepositoryImplementationWithPrimaryKey
                );
            }
        }

        private void RegisterForDbContext(
            Type dbContextType,
            IServiceCollection services,
            Type repositoryInterface,
            Type repositoryInterfaceWithPrimaryKey,
            Type repositoryImplementation,
            Type repositoryImplementationWithPrimaryKey)
        {
            foreach (var entityTypeInfo in _dbContextEntityFinder.GetEntityTypeInfos(dbContextType))
            {
                var primaryKeyType = EntityHelper.GetPrimaryKeyType(entityTypeInfo.EntityType);
                if (primaryKeyType == typeof(int))
                {
                    var genericRepositoryType = repositoryInterface.MakeGenericType(entityTypeInfo.EntityType);
                    if (!services.IsRegistered(genericRepositoryType))
                    {
                        var implType = repositoryImplementation.GetGenericArguments().Length == 1
                            ? repositoryImplementation.MakeGenericType(entityTypeInfo.EntityType)
                            : repositoryImplementation.MakeGenericType(entityTypeInfo.DeclaringType,
                                entityTypeInfo.EntityType);

                        services.AddScoped(genericRepositoryType, implType);
                    }
                }
                else
                {
                    var genericRepositoryTypeWithPrimaryKey = repositoryInterfaceWithPrimaryKey.MakeGenericType(entityTypeInfo.EntityType, primaryKeyType);
                    if (!services.IsRegistered(genericRepositoryTypeWithPrimaryKey))
                    {
                        var implType = repositoryImplementationWithPrimaryKey.GetGenericArguments().Length == 2
                            ? repositoryImplementationWithPrimaryKey.MakeGenericType(entityTypeInfo.EntityType, primaryKeyType)
                            : repositoryImplementationWithPrimaryKey.MakeGenericType(entityTypeInfo.DeclaringType, entityTypeInfo.EntityType, primaryKeyType);

                        services.AddScoped(genericRepositoryTypeWithPrimaryKey, implType);
                    }
                }
            }
        }
    }
}
