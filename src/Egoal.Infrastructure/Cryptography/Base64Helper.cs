using Egoal.Extensions;
using System;
using System.Text;

namespace Egoal.Cryptography
{
    public static class Base64Helper
    {
        public static string Encode(string str)
        {
            return Encode(str, Encoding.UTF8);
        }

        public static string Encode(string str, Encoding encoding)
        {
            if (str.IsNullOrEmpty())
            {
                return str;
            }

            return Convert.ToBase64String(encoding.GetBytes(str));
        }

        public static string Decode(string str)
        {
            return Decode(str, Encoding.UTF8);
        }

        public static string Decode(string str, Encoding encoding)
        {
            if (str.IsNullOrEmpty())
            {
                return str;
            }

            return encoding.GetString(Convert.FromBase64String(str));
        }
    }
}
