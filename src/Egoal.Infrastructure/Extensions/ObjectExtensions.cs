using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Egoal.Extensions
{
    public static class ObjectExtensions
    {
        public static T As<T>(this object obj)
            where T : class
        {
            return obj as T;
        }

        public static T To<T>(this object obj)
            where T : struct
        {
            if (typeof(T) == typeof(Guid))
            {
                return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(obj.ToString());
            }

            return (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);
        }

        public static bool IsIn<T>(this T item, params T[] list)
        {
            return list.Contains(item);
        }

        public static T DeepClone<T>(this T obj)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
        }

        public static bool IsNullOrEmpty(this object obj)
        {
            if (obj == null) return true;

            var props = obj.GetType().GetProperties();
            foreach (var prop in props)
            {
                var value = prop.GetValue(obj);
                if (value != null && value.ToString() != string.Empty)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
