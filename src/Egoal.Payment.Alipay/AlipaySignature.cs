using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Egoal.Payment.Alipay
{
    public class AlipaySignature
    {
        public static string RSASign(IDictionary<string, string> parameters, string privateKeyPem, string charset, string signType)
        {
            string signContent = BuildSignContent(parameters);

            return RSASignCharSet(signContent, privateKeyPem, charset, IsKeyFromFile(privateKeyPem), signType);
        }

        private static string BuildSignContent(IDictionary<string, string> parameters)
        {
            var sortedParams = new SortedDictionary<string, string>(parameters);
            var dem = sortedParams.GetEnumerator();

            StringBuilder query = new StringBuilder();
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value) && !key.Equals("sign", StringComparison.OrdinalIgnoreCase))
                {
                    query.Append(key).Append("=").Append(value).Append("&");
                }
            }

            string content = query.ToString().TrimEnd('&');

            return content;
        }

        private static string RSASignCharSet(string data, string privateKeyPem, string charset, bool keyFromFile, string signType)
        {
            RSACryptoServiceProvider rsaCsp = null;

            try
            {
                if (keyFromFile)
                {
                    rsaCsp = LoadCertificateFile(privateKeyPem, signType);
                }
                else
                {
                    rsaCsp = LoadCertificateString(privateKeyPem, signType);
                }

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
                if ("RSA2".Equals(signType, StringComparison.OrdinalIgnoreCase))
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

        private static RSACryptoServiceProvider LoadCertificateFile(string filename, string signType)
        {
            using (FileStream fs = File.OpenRead(filename))
            {
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);

                byte[] key = null;
                if (IsPemFile(filename))
                {
                    key = GetPem("RSA PRIVATE KEY", data);
                }
                else
                {
                    key = Convert.FromBase64String(Encoding.UTF8.GetString(data));
                }

                RSACryptoServiceProvider rsa = DecodeRSAPrivateKey(key, signType);

                return rsa;
            }
        }

        private static byte[] GetPem(string type, byte[] data)
        {
            string pem = Encoding.UTF8.GetString(data);

            string header = string.Format("-----BEGIN {0}-----\\n", type);
            string footer = string.Format("-----END {0}-----", type);
            int start = pem.IndexOf(header) + header.Length;
            int end = pem.IndexOf(footer, start);
            string base64 = pem.Substring(start, (end - start));

            return Convert.FromBase64String(base64);
        }

        private static RSACryptoServiceProvider LoadCertificateString(string strKey, string signType)
        {
            var data = Convert.FromBase64String(strKey);
            RSACryptoServiceProvider rsa = DecodeRSAPrivateKey(data, signType);

            return rsa;
        }

        private static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey, string signType)
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

                        int elems = GetIntegerSize(reader);
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

                        int bitLen = "RSA2".Equals(signType, StringComparison.OrdinalIgnoreCase) ? 2048 : 1024;

                        CspParameters CspParameters = new CspParameters();
                        CspParameters.Flags = CspProviderFlags.UseMachineKeyStore;

                        RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(bitLen, CspParameters);
                        RSA.ImportParameters(RSAparams);

                        return RSA;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
        }

        private static int GetIntegerSize(BinaryReader binr)
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

        public static bool RSACheckContent(string signContent, string sign, string publicKeyPem, string charset, string signType)
        {
            try
            {
                if (IsKeyFromFile(publicKeyPem))
                {
                    publicKeyPem = File.ReadAllText(publicKeyPem);
                }

                if ("RSA2".Equals(signType, StringComparison.OrdinalIgnoreCase))
                {
                    using (var rsa = DecodeRSAPublicKey(Convert.FromBase64String(publicKeyPem)))
                    {
                        bool bVerifyResultOriginal = rsa.VerifyData(Encoding.GetEncoding(charset).GetBytes(signContent), "SHA256", Convert.FromBase64String(sign));

                        return bVerifyResultOriginal;
                    }
                }
                else
                {
                    using (var rsa = DecodeRSAPublicKey(Convert.FromBase64String(publicKeyPem)))
                    {
                        SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
                        bool bVerifyResultOriginal = rsa.VerifyData(Encoding.GetEncoding(charset).GetBytes(signContent), sha1, Convert.FromBase64String(sign));

                        return bVerifyResultOriginal;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private static bool IsKeyFromFile(string key)
        {
            Regex regex = new Regex(@"^[a-zA-Z]:\\.+\.(pem|txt)$");

            return regex.IsMatch(key);
        }

        private static bool IsPemFile(string fileName)
        {
            return Path.GetExtension(fileName).Equals(".pem", StringComparison.OrdinalIgnoreCase);
        }

        private static string buildPem(string key)
        {
            StringBuilder pem = new StringBuilder();
            pem.Append("-----BEGIN PUBLIC KEY-----\r\n");
            pem.Append(key);
            pem.Append("-----END PUBLIC KEY-----\r\n\r\n");

            return pem.ToString();
        }

        public static RSACryptoServiceProvider DecodeRSAPublicKey(byte[] publicKey)
        {
            byte[] SeqOID = { 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01 };

            using (MemoryStream ms = new MemoryStream(publicKey))
            {
                using (BinaryReader reader = new BinaryReader(ms))
                {
                    if (reader.ReadByte() == 0x30)
                    {
                        ReadASNLength(reader);
                    }
                    else
                    {
                        return null;
                    }

                    int identifierSize = 0;
                    if (reader.ReadByte() == 0x30)
                    {
                        identifierSize = ReadASNLength(reader);
                    }
                    else
                    {
                        return null;
                    }

                    if (reader.ReadByte() == 0x06)
                    {
                        int oidLength = ReadASNLength(reader);
                        byte[] oidBytes = new byte[oidLength];
                        reader.Read(oidBytes, 0, oidBytes.Length);

                        if (!SequenceEqualByte(oidBytes, SeqOID)) return null;

                        int remainingBytes = identifierSize - 2 - oidBytes.Length;
                        reader.ReadBytes(remainingBytes);
                    }

                    if (reader.ReadByte() == 0x03)
                    {
                        ReadASNLength(reader);
                        reader.ReadByte();
                        if (reader.ReadByte() == 0x30)
                        {
                            ReadASNLength(reader);
                            if (reader.ReadByte() == 0x02)
                            {
                                int modulusSize = ReadASNLength(reader);
                                byte[] modulus = new byte[modulusSize];
                                reader.Read(modulus, 0, modulus.Length);
                                if (modulus[0] == 0x00)
                                {
                                    byte[] tempModulus = new byte[modulus.Length - 1];
                                    Array.Copy(modulus, 1, tempModulus, 0, modulus.Length - 1);
                                    modulus = tempModulus;
                                }

                                if (reader.ReadByte() == 0x02)
                                {
                                    int exponentSize = ReadASNLength(reader);
                                    byte[] exponent = new byte[exponentSize];
                                    reader.Read(exponent, 0, exponent.Length);

                                    RSAParameters parameters = new RSAParameters();
                                    parameters.Modulus = modulus;
                                    parameters.Exponent = exponent;
                                    RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                                    RSA.ImportParameters(parameters);

                                    return RSA;
                                }
                            }
                        }
                    }

                    return null;
                }
            }
        }

        private static int ReadASNLength(BinaryReader reader)
        {
            int length = reader.ReadByte();
            if ((length & 0x00000080) == 0x00000080)
            {
                int count = length & 0x0000000f;
                byte[] lengthBytes = new byte[4];
                reader.Read(lengthBytes, 4 - count, count);
                Array.Reverse(lengthBytes);
                length = BitConverter.ToInt32(lengthBytes, 0);
            }

            return length;
        }

        private static bool SequenceEqualByte(byte[] a, byte[] b)
        {
            var len1 = a.Length;
            var len2 = b.Length;
            if (len1 != len2)
            {
                return false;
            }
            for (var i = 0; i < len1; i++)
            {
                if (a[i] != b[i]) return false;
            }

            return true;
        }
    }
}
