using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;
using System.Text;

namespace Egoal.Extensions
{
    public static class JsonExtensions
    {
        public static string ToJson(this object obj, bool nullToEmpty = true)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            if (nullToEmpty)
            {
                StringBuilder sb = new StringBuilder();
                var jsonWriter = new NullJsonWriter(new StringWriter(sb));
                var jsonSerializer = new JsonSerializer();
                jsonSerializer.Serialize(jsonWriter, obj);

                return sb.ToString();
            }

            return JsonConvert.SerializeObject(obj);
        }

        public static T JsonToObject<T>(this string s)
        {
            return JsonConvert.DeserializeObject<T>(s);
        }

        public static bool IsJson(this string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                s = s.Trim().Trim('\r', '\n');
                if (s.Length > 1)
                {
                    return (s.StartsWith("{") && s.EndsWith("}")) || (s.StartsWith("[") && s.EndsWith("]"));
                }
            }

            return false;
        }

        private class NullJsonWriter : JsonTextWriter
        {
            public NullJsonWriter(TextWriter textWriter)
                : base(textWriter)
            {
            }

            public override void WriteNull()
            {
                WriteValue(string.Empty);
            }
        }
    }

    public class DateConverter : IsoDateTimeConverter
    {
        public DateConverter()
        {
            DateTimeFormat = DateTimeExtensions.DateFormat;
        }
    }

    public class DateTimeConverter : IsoDateTimeConverter
    {
        public DateTimeConverter()
        {
            DateTimeFormat = DateTimeExtensions.DateTimeFormat;
        }
    }
}
