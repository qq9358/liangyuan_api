using Egoal.Extensions;
using Egoal.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Egoal.Payment.Alipay
{
    public class AlipayApi
    {
        private readonly AlipayOptions _options;
        private readonly ILogger _logger;

        public AlipayApi(
            IOptions<AlipayOptions> options,
            ILogger<AlipayApi> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public async Task<string> PageExecuteAsync(AlipayRequest alipayRequest)
        {
            alipayRequest.app_id = _options.AliAppID;
            alipayRequest.sign_type = _options.AliPaySignType;
            alipayRequest.timestamp = DateTime.Now.ToString(AlipayOptions.DateTimeFormat);

            var parameters = alipayRequest.ToJson().JsonToObject<Dictionary<string, string>>();
            parameters["sign"] = alipayRequest.sign = AlipaySignature.RSASign(parameters, _options.AliPayMerChantPrivateKeyPath, alipayRequest.charset, _options.AliPaySignType);

            string html = BuildHtmlRequest(parameters, alipayRequest.charset, "POST", "POST");

            return await Task.FromResult(html);
        }

        private string BuildHtmlRequest(IDictionary<string, string> parameters, string charset, string method, string buttonValue)
        {
            StringBuilder sbHtml = new StringBuilder();

            sbHtml.Append($"<form id='alipaysubmit' name='alipaysubmit' action='https://openapi.alipay.com/gateway.do?charset={charset}' method='{method}' style='display:none;'>");
            foreach (KeyValuePair<string, string> temp in parameters)
            {
                sbHtml.Append($"<input  name='{temp.Key}' value='{temp.Value}'/>");
            }
            sbHtml.Append($"<input type='submit' value='{buttonValue}'></form>");
            sbHtml.Append("<script>document.forms['alipaysubmit'].submit();</script>");

            return sbHtml.ToString();
        }

        public async Task<T> ExecuteAsync<T>(AlipayRequest alipayRequest) where T : AlipayResponse
        {
            alipayRequest.app_id = _options.AliAppID;
            alipayRequest.sign_type = _options.AliPaySignType;
            alipayRequest.timestamp = DateTime.Now.ToString(AlipayOptions.DateTimeFormat);

            var parameters = alipayRequest.ToJson().JsonToObject<Dictionary<string, string>>();
            parameters["sign"] = alipayRequest.sign = AlipaySignature.RSASign(parameters, _options.AliPayMerChantPrivateKeyPath, alipayRequest.charset, _options.AliPaySignType);

            string url = $"https://openapi.alipay.com/gateway.do?charset={alipayRequest.charset}";
            string responseBody = await HttpHelper.PostFormDataAsync(url, parameters, Encoding.GetEncoding(alipayRequest.charset));

            string sign = GetSign(responseBody);
            string signSourceData = GetSignSourceData(alipayRequest, responseBody);

            CheckResponseSign(signSourceData, sign, _options.AliPayPublicKeyPath, alipayRequest.charset, _options.AliPaySignType);

            T response = null;

            try
            {
                response = signSourceData.JsonToObject<T>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            string data = BuildQuery(parameters, alipayRequest.charset);
            var originalData = $"{data}--{responseBody}";
            log(response, originalData);

            return response;
        }

        private string GetSign(string body)
        {
            var responseObj = body.JsonToObject<dynamic>();

            return responseObj.sign;
        }

        private string GetSignSourceData(AlipayRequest request, string body)
        {
            try
            {
                string rootNode = $"{request.method.Replace(".", "_")}_response";
                string errorRootNode = "error_response";

                int indexOfRootNode = body.IndexOf(rootNode);
                int indexOfErrorRoot = body.IndexOf(errorRootNode);

                string result = null;
                if (indexOfRootNode > 0)
                {
                    result = ParseSignSourceData(body, rootNode, indexOfRootNode);
                }
                else if (indexOfErrorRoot > 0)
                {
                    result = ParseSignSourceData(body, errorRootNode, indexOfErrorRoot);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}--{body}");

                throw;
            }
        }

        private string ParseSignSourceData(string body, string rootNode, int indexOfRootNode)
        {
            int signDataStartIndex = indexOfRootNode + rootNode.Length + 2;
            int indexOfSign = body.IndexOf("\"sign\"");
            int signDataEndIndex = indexOfSign < 0 ? body.Length - 1 : indexOfSign - 1;
            int length = signDataEndIndex - signDataStartIndex;

            return body.Substring(signDataStartIndex, length);
        }

        private string BuildQuery(IDictionary<string, string> parameters, string charset)
        {
            StringBuilder data = new StringBuilder();

            foreach (var pair in parameters)
            {
                if (!pair.Key.IsNullOrEmpty() && !pair.Value.IsNullOrEmpty())
                {
                    string encodedValue = HttpUtility.UrlEncode(pair.Value, Encoding.GetEncoding(charset));

                    data.Append(pair.Key).Append("=").Append(encodedValue).Append("&");
                }
            }

            return data.ToString().TrimEnd('&');
        }

        public NotifyRequest Notify(string data)
        {
            var parameters = new SortedDictionary<string, string>();

            var pairs = data.Split('&');
            foreach (var pair in pairs)
            {
                var temp = pair.Split('=');
                var key = temp[0];
                var value = temp[1];

                if (!key.IsNullOrEmpty() && !value.IsNullOrEmpty())
                {
                    parameters.Add(key, value.UrlDecode());
                }
            }

            StringBuilder builder = new StringBuilder();
            foreach (var pair in parameters)
            {
                if (!pair.Key.Equals("sign", StringComparison.OrdinalIgnoreCase) && !pair.Key.Equals("sign_type", StringComparison.OrdinalIgnoreCase))
                {
                    builder.Append(pair.Key).Append("=").Append(pair.Value).Append("&");
                }
            }

            var signSourceData = builder.ToString().TrimEnd('&');

            var request = parameters.ToJson().JsonToObject<NotifyRequest>();

            CheckResponseSign(signSourceData, request.sign, _options.AliPayPublicKeyPath, "utf-8", request.sign_type);

            return request;
        }

        private void CheckResponseSign(string signSourceData, string sign, string alipayPublicKey, string charset, string signType)
        {
            if (alipayPublicKey.IsNullOrEmpty() || charset.IsNullOrEmpty() || sign.IsNullOrEmpty())
            {
                return;
            }

            bool rsaCheckContent = AlipaySignature.RSACheckContent(signSourceData, sign, alipayPublicKey, charset, signType);
            if (!rsaCheckContent)
            {
                if (!string.IsNullOrEmpty(signSourceData) && signSourceData.Contains("\\/"))
                {
                    string srouceData = signSourceData.Replace("\\/", "/");
                    bool jsonCheck = AlipaySignature.RSACheckContent(srouceData, sign, alipayPublicKey, charset, signType);
                    if (!jsonCheck)
                    {
                        throw new ApiException("支付宝Response签名验证错误", $"{signSourceData}--{sign}");
                    }
                }
                else
                {
                    throw new ApiException("支付宝Response签名验证错误", $"{signSourceData}--{sign}");
                }
            }
        }

        private void log(AlipayResponse response, string originalData)
        {
            if (response == null || response.code != "10000")
            {
                _logger.LogError($"{response.msg}--{response.sub_msg}--{originalData}");
            }
            else
            {
                _logger.LogInformation(originalData);
            }
        }
    }
}
