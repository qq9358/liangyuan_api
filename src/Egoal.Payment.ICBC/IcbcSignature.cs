using Egoal.Cryptography;
using Egoal.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Egoal.Payment.IcbcPay
{
    /// <summary>
    /// 签名和验签
    /// </summary>
    public class IcbcSignature
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public IcbcSignature(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// RSA签名
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="privateKey"></param>
        /// <param name="charset"></param>
        /// <param name="signType"></param>
        /// <returns></returns>
        public string Sign(IDictionary<string, string> parameters, string privateKey, string charset, string signType, string apiUrl, string password = "")
        {
            string signContent = BuildSignContent(apiUrl, parameters);

            if (signType.EqualsIgnoreCase("CA"))
            {
                return CASign(signContent, privateKey, password);
            }

            return RSASign(signContent, privateKey, charset, signType);
        }

        private string BuildSignContent(string path, IDictionary<string, string> parameters)
        {
            var sortedParams = new SortedDictionary<string, string>(parameters);

            StringBuilder query = new StringBuilder(path);
            if (!string.IsNullOrEmpty(path))
            {
                query.Append("?");
            }

            foreach (var param in sortedParams)
            {
                string name = param.Key;
                string value = param.Value;
                if (value == null || name == null || value.Equals(""))
                {
                    continue;
                }

                query.Append(name).Append("=").Append(value).Append("&");
            }

            return query.ToString().TrimEnd('&');
        }

        private string CASign(string data, string privateKey, string password)
        {
            var path = _hostingEnvironment.ContentRootPath;
            if (_hostingEnvironment.IsDevelopment())
            {
                path = Path.Combine(path, "bin", "Debug", "netcoreapp2.1");
            }

            Process process = new Process();
            process.StartInfo.FileName = Path.Combine(path, "Egoal.Payment.IcbcPay.CA.exe");
            process.StartInfo.Arguments = $"{Base64Helper.Encode(data)} {privateKey} {password}";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;

            try
            {
                process.Start();
                process.WaitForExit();

                var sign = process.StandardOutput.ReadToEnd();

                return sign;
            }
            finally
            {
                process.Close();
            }
        }

        private string RSASign(string data, string privateKey, string charset, string signType)
        {
            RSACryptoServiceProvider rsaCsp = null;

            try
            {
                rsaCsp = LoadCertificateString(privateKey, signType);
                if (rsaCsp == null)
                {
                    throw new TmsException($"您使用的私钥格式错误，请检查RSA私钥配置，charset = {charset}");
                }

                byte[] dataBytes = null;
                if (string.IsNullOrEmpty(charset))
                {
                    dataBytes = Encoding.UTF8.GetBytes(data);
                }
                else
                {
                    dataBytes = Encoding.GetEncoding(charset).GetBytes(data);
                }

                byte[] signatureBytes = null;
                if ("RSA2".EqualsIgnoreCase(signType))
                {
                    signatureBytes = rsaCsp.SignData(dataBytes, "SHA256");
                }
                else
                {
                    signatureBytes = rsaCsp.SignData(dataBytes, "SHA1");
                }

                return Convert.ToBase64String(signatureBytes);
            }
            finally
            {
                rsaCsp?.Dispose();
            }
        }

        private RSACryptoServiceProvider LoadCertificateString(string strKey, string signType)
        {
            var kay = Convert.FromBase64String(strKey);

            RSACryptoServiceProvider rsa = DecodePrivateKeyInfo(kay);

            return rsa;
        }

        private RSACryptoServiceProvider DecodePrivateKeyInfo(byte[] pkcs8)
        {
            using (MemoryStream stream = new MemoryStream(pkcs8))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    try
                    {
                        ushort twobytes = 0;
                        twobytes = reader.ReadUInt16();
                        if (twobytes == 0x8130)
                        {
                            reader.ReadByte();
                        }
                        else if (twobytes == 0x8230)
                        {
                            reader.ReadInt16();
                        }
                        else
                        {
                            return null;
                        }

                        byte bt = 0;
                        bt = reader.ReadByte();
                        if (bt != 0x02)
                        {
                            return null;
                        }

                        twobytes = reader.ReadUInt16();
                        if (twobytes != 0x0001)
                        {
                            return null;
                        }

                        byte[] seqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
                        byte[] seq = new byte[15];
                        seq = reader.ReadBytes(15);
                        if (!CompareBytearrays(seq, seqOID))
                        {
                            return null;
                        }

                        bt = reader.ReadByte();
                        if (bt != 0x04)
                        {
                            return null;
                        }

                        bt = reader.ReadByte();
                        if (bt == 0x81)
                        {
                            reader.ReadByte();
                        }
                        else if (bt == 0x82)
                        {
                            reader.ReadUInt16();
                        }

                        int lenstream = (int)stream.Length;
                        byte[] rsaprivkey = reader.ReadBytes((int)(lenstream - stream.Position));
                        RSACryptoServiceProvider rsacsp = DecodeRSAPrivateKey(rsaprivkey);

                        return rsacsp;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
        }

        private bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length) return false;

            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i]) return false;

                i++;
            }

            return true;
        }

        private RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey)
        {
            using (MemoryStream stream = new MemoryStream(privkey))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    try
                    {
                        ushort twobytes = 0;
                        twobytes = reader.ReadUInt16();
                        if (twobytes == 0x8130)
                        {
                            reader.ReadByte();
                        }
                        else if (twobytes == 0x8230)
                        {
                            reader.ReadInt16();
                        }
                        else
                        {
                            return null;
                        }

                        twobytes = reader.ReadUInt16();
                        if (twobytes != 0x0102)
                        {
                            return null;
                        }

                        byte bt = 0;
                        bt = reader.ReadByte();
                        if (bt != 0x00)
                        {
                            return null;
                        }

                        RSAParameters RSAparams = new RSAParameters();

                        int elems = 0;
                        elems = GetIntegerSize(reader);
                        RSAparams.Modulus = reader.ReadBytes(elems);

                        elems = GetIntegerSize(reader);
                        RSAparams.Exponent = reader.ReadBytes(elems);

                        elems = GetIntegerSize(reader);
                        RSAparams.D = reader.ReadBytes(elems);

                        elems = GetIntegerSize(reader);
                        RSAparams.P = reader.ReadBytes(elems);

                        elems = GetIntegerSize(reader);
                        RSAparams.Q = reader.ReadBytes(elems);

                        elems = GetIntegerSize(reader);
                        RSAparams.DP = reader.ReadBytes(elems);

                        elems = GetIntegerSize(reader);
                        RSAparams.DQ = reader.ReadBytes(elems);

                        elems = GetIntegerSize(reader);
                        RSAparams.InverseQ = reader.ReadBytes(elems);

                        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                        rsa.ImportParameters(RSAparams);

                        return rsa;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
        }

        private int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)
            {
                return 0;
            }

            int count = 0;

            bt = binr.ReadByte();
            if (bt == 0x81)
            {
                count = binr.ReadByte();
            }
            else if (bt == 0x82)
            {
                byte highbyte = 0x00;
                highbyte = binr.ReadByte();
                byte lowbyte = 0x00;
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;
            }

            while (binr.ReadByte() == 0x00)
            {
                count -= 1;
            }

            binr.BaseStream.Seek(-1, SeekOrigin.Current);

            return count;
        }

        public bool RSACheckContent(string signContent, string sign, string publicKey, string charset, string signType)
        {
            try
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    byte[] buffer = Encoding.GetEncoding(charset).GetBytes(signContent);
                    byte[] signature = Convert.FromBase64String(sign);
                    RSAParameters param = ConvertFromPublicKey(publicKey);
                    rsa.ImportParameters(param);

                    if ("RSA2".EqualsIgnoreCase(signType))
                    {
                        return rsa.VerifyData(buffer, "SHA256", signature);
                    }
                    else
                    {
                        return rsa.VerifyData(buffer, "SHA1", signature);
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private RSAParameters ConvertFromPublicKey(string publicKey)
        {
            byte[] keyData = Convert.FromBase64String(publicKey);
            if (keyData.Length < 162)
            {
                throw new ArgumentException("pem file content is incorrect.");
            }
            byte[] modulus = new byte[128];
            byte[] publicExponent = new byte[3];
            Array.Copy(keyData, 29, modulus, 0, 128);
            Array.Copy(keyData, 159, publicExponent, 0, 3);

            RSAParameters param = new RSAParameters();
            param.Modulus = modulus;
            param.Exponent = publicExponent;

            return param;
        }

        public string EncryptContent(string content, string encryptType, string encryptKey)
        {
            if (encryptType.EqualsIgnoreCase("AES"))
            {
                return AesEncrypt(content, encryptKey);
            }

            throw new TmsException($"当前不支持该算法类型：encrypeType={encryptType}");
        }

        private string AesEncrypt(string toEncrypt, string key)
        {
            byte[] _Key = Convert.FromBase64String(key);
            byte[] _Source = Encoding.UTF8.GetBytes(toEncrypt);

            Aes aes = Aes.Create("AES");
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = _Key;
            aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            ICryptoTransform cTransform = aes.CreateEncryptor();
            byte[] cryptData = cTransform.TransformFinalBlock(_Source, 0, _Source.Length);

            return Convert.ToBase64String(cryptData);
        }
    }
}
