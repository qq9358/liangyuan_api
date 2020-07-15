using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Egoal.Net.Http
{
    public static class HttpHelper
    {
        public const int Timeout = 10;

        private static HttpClient httpClient;

        static HttpHelper()
        {
            ServicePointManager.Expect100Continue = false;

            httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(Timeout);
        }

        public static async Task<byte[]> GetAsync(string url)
        {
            return await httpClient.GetByteArrayAsync(url);
        }

        public static async Task<string> GetStringAsync(string url)
        {
            return await GetStringAsync(url, Encoding.UTF8);
        }

        public static async Task<string> GetStringAsync(string url, Encoding encoding)
        {
            var result = await httpClient.GetByteArrayAsync(url.Trim());

            return encoding.GetString(result);
        }

        public static async Task<string> PostJsonAsync(string url, string json)
        {
            return await PostJsonAsync(url, json, Encoding.UTF8);
        }

        public static async Task<string> PostJsonAsync(string url, string json, Encoding encoding)
        {
            return await PostAsync(url, json, encoding, "application/json");
        }

        public static async Task<string> PostXmlAsync(string url, string xml)
        {
            return await PostXmlAsync(url, xml, Encoding.UTF8);
        }

        public static async Task<string> PostXmlAsync(string url, string xml, Encoding encoding)
        {
            return await PostAsync(url, xml, encoding, "text/xml");
        }

        public static async Task<string> PostFormDataAsync(string url, IDictionary<string, string> data)
        {
            return await PostFormDataAsync(url, data, Encoding.UTF8);
        }

        public static async Task<string> PostFormDataAsync(string url, IDictionary<string, string> data, Encoding encoding)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(data);

            return await PostAsync(url, content, encoding);
        }

        public static async Task<string> PostFormDataAsync(string url, string data)
        {
            return await PostFormDataAsync(url, data, Encoding.UTF8);
        }

        public static async Task<string> PostFormDataAsync(string url, string data, Encoding encoding)
        {
            return await SendAsync(url, data, encoding, "POST", $"application/x-www-form-urlencoded;charset={encoding.WebName}");
        }

        public static async Task<string> PostAsync(string url, string data, Encoding encoding, string contentType)
        {
            StringContent content = new StringContent(data, encoding, contentType);

            return await PostAsync(url, content, encoding);
        }

        public static async Task<string> PostAsync(string url, HttpContent content, Encoding encoding)
        {
            var response = await httpClient.PostAsync(url.Trim(), content);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsByteArrayAsync();

            return encoding.GetString(result);
        }

        public static async Task<string> PostXmlAsync(string url, string xml, X509Certificate2 certificate2)
        {
            return await PostXmlAsync(url, xml, Encoding.UTF8, certificate2);
        }

        public static async Task<string> PostXmlAsync(string url, string xml, Encoding encoding, X509Certificate2 certificate2)
        {
            return await SendAsync(url, xml, encoding, "POST", "text/xml", certificate2);
        }

        public static async Task<string> SendAsync(string url, string data, Encoding encoding, string method, string contentType, X509Certificate2 certificate2 = null)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;

            try
            {
                var uri = new Uri(url.Trim());

                request = (HttpWebRequest)WebRequest.Create(uri);
                request.ServicePoint.Expect100Continue = false;
                request.ServicePoint.ConnectionLimit = 200;
                request.Method = method;
                request.KeepAlive = true;
                request.UserAgent = ".net core httphelper";
                request.Timeout = Timeout * 1000;
                request.Proxy = null;
                request.ContentType = contentType;

                if (uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
                {
                    request.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => { return true; };
                }

                if (certificate2 != null)
                {
                    request.ClientCertificates.Add(certificate2);
                }

                var sendBytes = encoding.GetBytes(data);
                if (sendBytes != null && sendBytes.Length > 0)
                {
                    request.ContentLength = sendBytes.Length;
                    using (var reqStream = await request.GetRequestStreamAsync())
                    {
                        await reqStream.WriteAsync(sendBytes, 0, sendBytes.Length);
                    }
                }

                response = (HttpWebResponse)await request.GetResponseAsync();

                using (var resStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(resStream, encoding))
                    {
                        return await reader.ReadToEndAsync();
                    }
                }
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
        }
    }
}
