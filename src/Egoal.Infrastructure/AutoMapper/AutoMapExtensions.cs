using Nelibur.ObjectMapper;
using System.Collections.Generic;

namespace Egoal.AutoMapper
{
    public static class AutoMapExtensions
    {
        public static TDestination MapTo<TDestination>(this object source)
        {
            return TinyMapper.Map<TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return TinyMapper.Map(source, destination);
        }

        public static List<TDestination> MapTo<TDestination>(this IEnumerable<object> source)
        {
            List<TDestination> destinations = new List<TDestination>();

            foreach (var item in source)
            {
                destinations.Add(item.MapTo<TDestination>());
            }

            return destinations;
        }
    }
}
