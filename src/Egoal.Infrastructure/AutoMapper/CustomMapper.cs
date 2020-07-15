using Nelibur.ObjectMapper;
using Nelibur.ObjectMapper.Bindings;
using System;
using System.Linq;
using System.Reflection;

namespace Egoal.AutoMapper
{
    public static class CustomMapper
    {
        public static void Bind(Type sourceType, Type targetType)
        {
            TinyMapper.Bind(sourceType, targetType);
        }

        public static void Bind<TSource, TTarget>()
        {
            TinyMapper.Bind<TSource, TTarget>();
        }

        public static void Bind<TSource, TTarget>(Action<IBindingConfig<TSource, TTarget>> config)
        {
            TinyMapper.Bind(config);
        }

        public static void AutoMap<TSource, TTarget>()
        {
            TinyMapper.Bind<TSource, TTarget>();
            TinyMapper.Bind<TTarget, TSource>();
        }

        public static void CreateAssemblyMappings(Assembly assembly)
        {
            var types = assembly.GetTypes();

            var attributeTypes = types.Where(t => t.IsDefined(typeof(AutoMapAttribute)) || t.IsDefined(typeof(AutoMapFromAttribute)) || t.IsDefined(typeof(AutoMapToAttribute)));
            foreach (var type in attributeTypes)
            {
                var autoMapAttributes = type.GetCustomAttributes<AutoMapAttributeBase>();
                foreach (var autoMapAttribute in autoMapAttributes)
                {
                    autoMapAttribute.CreateMap(type);
                }
            }

            var interfaceType = typeof(IAutoMap);
            var interfaces = types.Where(t => interfaceType.IsAssignableFrom(t));
            foreach (var @interface in interfaces)
            {
                var mapper = Activator.CreateInstance(@interface) as IAutoMap;
                mapper.CreateMappings();
            }
        }
    }
}
