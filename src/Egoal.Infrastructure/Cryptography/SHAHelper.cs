using System;
using System.Security.Cryptography;
using System.Text;

namespace Egoal.Cryptography
{
    public static class SHAHelper
    {
        /// <summary>
        /// SHA512加密
        /// </summary>
        /// <param name="s"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string SHA512Encrypt(string s, string salt)
        {
            byte[] buffer1 = Encoding.UTF8.GetBytes(s);
            byte[] buffer2 = Encoding.UTF8.GetBytes(salt);
            SHA512 sha = SHA512.Create();
            /*1.源字符串先加密*/
            byte[] buffer3 = sha.ComputeHash(buffer1);
            /*2.加盐值再加密*/
            byte[] buffer4 = new byte[buffer3.Length + buffer2.Length];
            Array.Copy(buffer3, buffer4, buffer3.Length);
            Array.Copy(buffer2, 0, buffer4, buffer3.Length, buffer2.Length);
            byte[] buffer = sha.ComputeHash(buffer4);
            /*输出字符串*/
            return BitConverter.ToString(buffer).Replace("-", string.Empty);
        }

        public static string SHA1Encrypt(string str)
        {
            var buffer = Encoding.UTF8.GetBytes(str);

            SHA1 sha = SHA1.Create();
            var resultBuffer = sha.ComputeHash(buffer);

            return BitConverter.ToString(resultBuffer).Replace("-", string.Empty);
        }
    }
}
