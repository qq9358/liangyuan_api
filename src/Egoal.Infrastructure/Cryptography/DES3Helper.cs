using System;
using System.Security.Cryptography;
using System.Text;

namespace Egoal.Cryptography
{
    public static class DES3Helper
    {
        private static byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        private static string Default3DESKey = "egoal159egoal258egoal357";

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str">明文</param>
        /// <returns>返回密文</returns>
        public static string Encrypt(string str)
        {
            return Encrypt(str, Default3DESKey);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str">明文</param>
        /// <param name="key">加密密匙</param>
        /// <returns>返回密文</returns>
        public static string Encrypt(string str, string key)
        {
            TripleDESCryptoServiceProvider provider1 = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider provider2 = new MD5CryptoServiceProvider();
            provider1.Key = provider2.ComputeHash(Encoding.ASCII.GetBytes(key));
            provider1.IV = IV;
            provider1.Mode = CipherMode.ECB;
            ICryptoTransform ct = provider1.CreateEncryptor();
            byte[] buffer = Encoding.ASCII.GetBytes(str);
            return Convert.ToBase64String(ct.TransformFinalBlock(buffer, 0, buffer.Length));
        }

        /// <summary>
        /// 3DES解密
        /// </summary>
        /// <param name="str">密文</param>
        /// <returns>返回明文</returns>
        public static string Decrypt(string str)
        {
            return Decrypt(str, Default3DESKey);
        }

        /// <summary>
        /// 3DES解密
        /// </summary>
        /// <param name="str">密文</param>
        /// <param name="key">解密密匙</param>
        /// <returns>返回明文</returns>
        public static string Decrypt(string str, string key)
        {
            TripleDESCryptoServiceProvider provider1 = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider provider2 = new MD5CryptoServiceProvider();
            provider1.Key = provider2.ComputeHash(Encoding.ASCII.GetBytes(key));
            provider1.IV = IV;
            provider1.Mode = CipherMode.ECB;
            ICryptoTransform ct = provider1.CreateDecryptor();
            byte[] buffer = Convert.FromBase64String(str);
            return Encoding.ASCII.GetString(ct.TransformFinalBlock(buffer, 0, buffer.Length));
        }
    }
}
