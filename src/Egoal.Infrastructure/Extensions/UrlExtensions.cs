using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web;

namespace Egoal.Extensions
{
    public static class UrlExtensions
    {
        public static string UrlCombine(this string url, params string[] args)
        {
            StringBuilder sbUrl = new StringBuilder(url.TrimEnd('/'));

            foreach (var arg in args)
            {
                sbUrl.Append("/").Append(arg.Trim('/'));
            }

            return sbUrl.ToString();
        }

        public static string ToUrlArgs(this object obj, bool ignoreNull = false)
        {
            StringBuilder args = new StringBuilder();

            PropertyInfo[] propertys = obj.GetType().GetProperties();
            foreach (var property in propertys)
            {
                var value = property.GetValue(obj);

                if (value == null)
                {
                    if (!ignoreNull)
                    {
                        args.Append(property.Name).Append("=").Append("&");
                    }
                }
                else
                {
                    args.Append(property.Name).Append("=").Append(value.ToString()).Append("&");
                }
            }

            return args.ToString().Trim('&');
        }

        public static T UrlArgsToObject<T>(this string s)
        {
            Dictionary<string, string> args = new Dictionary<string, string>();
            var pairs = s.Split('&');
            foreach (var pair in pairs)
            {
                var arg = pair.Split('=');
                if (!arg.IsNullOrEmpty())
                {
                    args.Add(arg[0], arg[1]);
                }
            }

            return args.ToJson().JsonToObject<T>();
        }

        public static string UrlEncode(this string s)
        {
            return HttpUtility.UrlEncode(s, Encoding.UTF8);
        }

        public static string UrlDecode(this string s)
        {
            return HttpUtility.UrlDecode(s);
        }
    }
}
