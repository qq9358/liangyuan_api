using System.Security.Cryptography;
using System.Text;

namespace Egoal.Cryptography
{
    public static class MD5Helper
    {
        public static string Encrypt(string str)
        {
            return Encrypt(str, Encoding.UTF8);
        }

        public static string Encrypt(string str, Encoding e)
        {
            MD5 md5 = MD5.Create();
            byte[] data = md5.ComputeHash(e.GetBytes(str));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
