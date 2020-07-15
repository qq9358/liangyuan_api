using Egoal.Extensions;
using Egoal.Net.Http;
using Egoal.Payment.IcbcPay.Request;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Egoal.Payment.IcbcPay
{
    public class IcbcPayApi
    {
        private readonly IcbcPayOptions _options;
        private readonly ILogger _logger;
        private readonly IcbcSignature _icbcSignature;

        public IcbcPayApi(
            IOptions<IcbcPayOptions> options,
            ILogger<IcbcPayApi> logger,
            IcbcSignature icbcSignature)
        {
            _options = options.Value;
            _logger = logger;
            _icbcSignature = icbcSignature;
        }

        public async Task<string> PageExecuteAsync(IDictionary<string, string> payRequest, string apiUrl)
        {
            payRequest["msg_id"] = GetMsgId();
            payRequest["timestamp"] = GetTimestamp();

            payRequest["sign"] = _icbcSignature.Sign(payRequest, GetPrivateKey(payRequest["sign_type"]), payRequest["charset"], payRequest["sign_type"], apiUrl, _options.IcbcCAPrivateKeyPassword);

            string html = BuildHtmlRequest(payRequest, payRequest["charset"], "POST", "POST", apiUrl);

            _logger.LogInformation(html);

            return await Task.FromResult(html);
        }

        private string BuildHtmlRequest(IDictionary<string, string> iDictionary, string charset, string method, string buttonValue, string apiUrl)
        {
            var queryParams = new[] { "app_id", "sign", "sign_type", "charset", "format", "encrypt_type", "timestamp", "msg_id" };

            StringBuilder queryBuilder = new StringBuilder();
            StringBuilder bodyBuilder = new StringBuilder();
            foreach (KeyValuePair<string, string> keyValuePair in iDictionary)
            {
                if (queryParams.Any(p => p == keyValuePair.Key))
                {
                    queryBuilder.Append(keyValuePair.Key.UrlEncode()).Append("=").Append(keyValuePair.Value.UrlEncode()).Append("&");
                }
                else
                {
                    if (keyValuePair.Value.IsNullOrEmpty()) continue;
                    bodyBuilder.Append($"<input name='{keyValuePair.Key}' value='{keyValuePair.Value}'/>");
                }
            }

            string url = $"{ _options.IcbcPayUrl.UrlCombine(apiUrl)}?{queryBuilder.ToString().TrimEnd('&')}";

            StringBuilder formBuilder = new StringBuilder();
            formBuilder.Append($"<form id='IcbcSubmit' name='IcbcSubmit' action='{url}' method='{method}' style='display:none'>");
            formBuilder.Append(bodyBuilder.ToString());
            formBuilder.Append($"<input type='submit' value='{buttonValue}' /></form>");
            formBuilder.Append("<script>document.forms['IcbcSubmit'].submit();</script>");

            return formBuilder.ToString();
        }

        public async Task<T> ExecuteAsync<T>(IcbcPayRequest icbcPayRequest, string apiUrl) where T : IcbcPayResponse
        {
            if (icbcPayRequest.app_id.IsNullOrEmpty())
            {
                icbcPayRequest.app_id = _options.IcbcAppId;
            }
            icbcPayRequest.msg_id = GetMsgId();
            icbcPayRequest.timestamp = GetTimestamp();

            Dictionary<string, string> parameters = icbcPayRequest.ToJson().JsonToObject<Dictionary<string, string>>();
            parameters["sign"] = icbcPayRequest.sign = _icbcSignature.Sign(parameters, GetPrivateKey(icbcPayRequest.sign_type), icbcPayRequest.charset, icbcPayRequest.sign_type, apiUrl, _options.IcbcCAPrivateKeyPassword);

            string url = _options.IcbcPayUrl.UrlCombine(apiUrl);
            string responseBody = await HttpHelper.PostFormDataAsync(url, parameters, Encoding.GetEncoding(icbcPayRequest.charset));

            string signSourceData = GetSignSourceData(responseBody);
            string sign = GetSign(responseBody);

            CheckResponseSign(signSourceData, sign, _options.IcbcPublicKey, icbcPayRequest.charset, "RSA");

            T response = null;

            try
            {
                response = signSourceData.JsonToObject<T>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            string data = BuildQuery(parameters, icbcPayRequest.charset);
            var originalData = $"{data}--{responseBody}";
            Log(response, originalData);

            return response;
        }

        private string GetSignSourceData(string body)
        {
            try
            {
                string rootNode = "response_biz_content";

                int indexOfRootStart = body.IndexOf(rootNode) + rootNode.Length + 2;
                int indexOfRootEnd = body.LastIndexOf(",");

                return body.Substring(indexOfRootStart, indexOfRootEnd - indexOfRootStart);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}--{body}");

                throw;
            }
        }

        private string GetSign(string body)
        {
            try
            {
                string rootNode = "sign";

                int indexOfSignStart = body.LastIndexOf(rootNode) + rootNode.Length + 3;
                int indexOfSignEnd = body.LastIndexOf("\"");

                return body.Substring(indexOfSignStart, indexOfSignEnd - indexOfSignStart);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}--{body}");

                throw;
            }
        }

        private string BuildQuery(IDictionary<string, string> parameters, string charset)
        {
            StringBuilder data = new StringBuilder();

            foreach (var pair in parameters)
            {
                data.Append(pair.Key).Append("=").Append(pair.Value).Append("&");
            }

            return data.ToString().TrimEnd('&');
        }

        public NotifyRequest Notify(string data, string notifyUrl)
        {
            try
            {
                _logger.LogInformation(data.UrlDecode());

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
                    if (!pair.Key.EqualsIgnoreCase("sign"))
                    {
                        builder.Append(pair.Key).Append("=").Append(pair.Value).Append("&");
                    }
                }

                Uri uri = new Uri(notifyUrl);
                var signSourceData = $"{uri.PathAndQuery}?{builder.ToString().TrimEnd('&')}";

                CheckResponseSign(signSourceData, parameters["sign"], _options.IcbcPublicKey, parameters["charset"], parameters["sign_type"]);

                var request = parameters["biz_content"].JsonToObject<NotifyRequest>();

                return request;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}--{data}");

                throw;
            }
        }

        public string GenerateNotifyResponse(bool success, string message = "", NetPayInput input = null)
        {
            if (success && input != null && input.TradeType == OnlinePayTradeType.MWEB)
            {
                return input.ReturnUrl;
            }

            var content = new
            {
                return_code = success ? 0 : 1,
                return_msg = success ? "success" : message,
                msg_id = GetMsgId()
            };

            string signType = "RSA2";

            var parameters = new Dictionary<string, string>();
            parameters.Add("response_biz_content", content.ToJson());
            parameters.Add("sign_type", signType);

            string sign = _icbcSignature.Sign(parameters, _options.IcbcMerchantPrivateKey, "UTF-8", signType, string.Empty);

            var response = new
            {
                response_biz_content = content,
                sign_type = signType,
                sign
            };

            return response.ToJson();
        }

        private void CheckResponseSign(string signSourceData, string sign, string publicKey, string charset, string signType)
        {
            if (publicKey.IsNullOrEmpty() || charset.IsNullOrEmpty() || sign.IsNullOrEmpty())
            {
                return;
            }

            bool rsaCheckContent = _icbcSignature.RSACheckContent(signSourceData, sign, publicKey, charset, signType);
            if (!rsaCheckContent)
            {
                throw new ApiException("Response签名验证错误", $"{signSourceData}--{sign}");
            }
        }

        private string GetMsgId()
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty);
        }

        private string GetTimestamp()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private string GetPrivateKey(string signType)
        {
            return signType.EqualsIgnoreCase("CA") ? _options.IcbcCAPrivateKeyPath : _options.IcbcMerchantPrivateKey;
        }

        private void Log(IcbcPayResponse response, string originalData)
        {
            if (response == null || response.return_code != 0)
            {
                _logger.LogError($"{response.return_msg}--{originalData}");
            }
            else
            {
                _logger.LogInformation(originalData);
            }
        }
    }
}
