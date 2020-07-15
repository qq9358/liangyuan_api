using Egoal.Extensions;
using Egoal.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Egoal.Payment.SaobePay
{
    public class SaobePayApi
    {
        private readonly SaobePayOptions _options;
        private readonly ILogger _logger;

        public SaobePayApi(
            IOptions<SaobePayOptions> options,
            ILogger<SaobePayApi> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public async Task<JsApiPayResult> JsApiPayAsync(JsApiPayRequest input)
        {
            input.service_id = "012";
            SetCommonValue(input);
            if (input.notify_url.IsNullOrEmpty())
            {
                input.notify_url = _options.WebApiUrl.UrlCombine("/Payment/SaobeNotify");
            }

            input.MakeSign(_options.SaoBeAccessToken);

            var json = input.ToJson();

            string url = _options.SaoBeDomainUrl.UrlCombine("/pay/100/jspay");
            string response = await HttpHelper.PostJsonAsync(url, json);

            var originalData = $"{json}--{response}";

            if (!response.IsJson())
            {
                throw new ApiException("返回数据格式错误", originalData);
            }

            var payResult = response.JsonToObject<JsApiPayResult>();
            if (!payResult.CheckSign())
            {
                throw new ApiException("返回数据签名错误", originalData);
            }

            Log(payResult, originalData);

            return payResult;
        }

        public async Task<MicroPayResult> MicroPayAsync(MicroPayRequest request)
        {
            request.service_id = "010";
            SetCommonValue(request);

            request.MakeSign(_options.SaoBeAccessToken);

            var json = request.ToJson();

            string url = _options.SaoBeDomainUrl.UrlCombine("/pay/100/barcodepay");
            string response = await HttpHelper.PostJsonAsync(url, json);

            var originalData = $"{json}--{response}";

            if (!response.IsJson())
            {
                throw new ApiException("返回数据格式错误", originalData);
            }

            var result = response.JsonToObject<MicroPayResult>();
            if (!result.CheckSign())
            {
                throw new ApiException("返回数据签名错误", originalData);
            }

            Log(result, originalData);

            return result;
        }

        public async Task<NativePayResult> NativePayAsync(NativePayRequest input)
        {
            input.service_id = "011";
            SetCommonValue(input);
            if (input.notify_url.IsNullOrEmpty())
            {
                input.notify_url = _options.WebApiUrl.UrlCombine("/Payment/SaobeNotify");
            }

            input.MakeSign(_options.SaoBeAccessToken);

            var json = input.ToJson();

            string url = _options.SaoBeDomainUrl.UrlCombine("/pay/100/prepay");
            string response = await HttpHelper.PostJsonAsync(url, json);

            var originalData = $"{json}--{response}";

            if (!response.IsJson())
            {
                throw new ApiException("返回数据格式错误", originalData);
            }

            var payResult = response.JsonToObject<NativePayResult>();
            if (!payResult.CheckSign())
            {
                throw new ApiException("返回数据签名错误", originalData);
            }

            Log(payResult, originalData);

            return payResult;
        }

        public NotifyResult Notify(string data)
        {
            if (!data.IsJson())
            {
                throw new ApiException("通知数据格式错误", data);
            }

            var result = data.JsonToObject<NotifyResult>();
            if (!result.CheckSign(_options.SaoBeAccessToken))
            {
                throw new ApiException("通知数据签名错误", data);
            }

            Log(result, data);

            return result;
        }

        public async Task<QueryOrderResult> QueryPayAsync(QueryOrderRequest request)
        {
            request.service_id = "020";
            SetCommonValue(request);

            request.MakeSign(_options.SaoBeAccessToken);

            var json = request.ToJson();

            string url = _options.SaoBeDomainUrl.UrlCombine("/pay/100/query");
            string response = await HttpHelper.PostJsonAsync(url, json);

            var originalData = $"{json}--{response}";

            if (!response.IsJson())
            {
                throw new ApiException("返回数据格式错误", originalData);
            }

            var result = response.JsonToObject<QueryOrderResult>();
            if (!result.CheckSign())
            {
                throw new ApiException("返回数据签名错误", originalData);
            }

            Log(result, originalData);

            return result;
        }

        public async Task<CloseOrderResult> ClosePayAsync(CloseOrderRequest request)
        {
            request.service_id = "041";
            SetCommonValue(request);

            request.MakeSign(_options.SaoBeAccessToken);

            var json = request.ToJson();

            string url = _options.SaoBeDomainUrl.UrlCombine("/pay/100/close");
            string response = await HttpHelper.PostJsonAsync(url, json);

            var originalData = $"{json}--{response}";

            if (!response.IsJson())
            {
                throw new ApiException("返回数据格式错误", originalData);
            }

            var result = response.JsonToObject<CloseOrderResult>();
            if (!result.CheckSign())
            {
                throw new ApiException("返回数据签名错误", originalData);
            }

            Log(result, originalData);

            return result;
        }

        public async Task<ReverseResult> ReversePayAsync(ReverseRequest request)
        {
            request.service_id = "040";
            SetCommonValue(request);

            request.MakeSign(_options.SaoBeAccessToken);

            var json = request.ToJson();

            string url = _options.SaoBeDomainUrl.UrlCombine("/pay/100/cancel");
            string response = await HttpHelper.PostJsonAsync(url, json);

            var originalData = $"{json}--{response}";

            if (!response.IsJson())
            {
                throw new ApiException("返回数据格式错误", originalData);
            }

            var result = response.JsonToObject<ReverseResult>();
            if (!result.CheckSign())
            {
                throw new ApiException("返回数据签名错误", originalData);
            }

            Log(result, originalData);

            return result;
        }

        public async Task<RefundResult> RefundAsync(RefundRequest request)
        {
            request.service_id = "030";
            SetCommonValue(request);

            request.MakeSign(_options.SaoBeAccessToken);

            var json = request.ToJson();

            string url = _options.SaoBeDomainUrl.UrlCombine("/pay/100/refund");
            string response = await HttpHelper.PostJsonAsync(url, json);

            var originalData = $"{json}--{response}";

            if (!response.IsJson())
            {
                throw new ApiException("返回数据格式错误", originalData);
            }

            var result = response.JsonToObject<RefundResult>();
            if (!result.CheckSign())
            {
                throw new ApiException("返回数据签名错误", originalData);
            }

            Log(result, originalData);

            return result;
        }

        public async Task<QueryRefundResult> QueryRefundAsync(QueryRefundRequest request)
        {
            request.service_id = "031";
            SetCommonValue(request);

            request.MakeSign(_options.SaoBeAccessToken);

            var json = request.ToJson();

            string url = _options.SaoBeDomainUrl.UrlCombine("/pay/100/queryrefund");
            string response = await HttpHelper.PostJsonAsync(url, json);

            var originalData = $"{json}--{response}";

            if (!response.IsJson())
            {
                throw new ApiException("返回数据格式错误", originalData);
            }

            var result = response.JsonToObject<QueryRefundResult>();
            if (!result.CheckSign())
            {
                throw new ApiException("返回数据签名错误", originalData);
            }

            Log(result, originalData);

            return result;
        }

        public async Task<RegisterResult> RegisterAsync(RegisterRequest request)
        {
            request.pay_ver = "100";
            request.service_id = "090";
            request.merchant_no = _options.SaoBeMerchantNo;
            request.terminal_id = _options.SaoBeTerminalId;

            request.MakeSign();

            var json = request.ToJson();

            string url = _options.SaoBeDomainUrl.UrlCombine("/pay/100/sign");
            string response = await HttpHelper.PostJsonAsync(url, json);

            var originalData = $"{json}--{response}";

            if (!response.IsJson())
            {
                throw new ApiException("返回数据格式错误", originalData);
            }

            var result = response.JsonToObject<RegisterResult>();
            if (!result.CheckSign())
            {
                throw new ApiException("返回数据签名错误", originalData);
            }

            var resultBase = new ResultBase
            {
                return_code = result.return_code,
                return_msg = result.return_msg,
                result_code = result.result_code
            };
            Log(resultBase, originalData);

            return result;
        }

        private void SetCommonValue(RequestBase input)
        {
            input.pay_ver = "100";
            input.merchant_no = _options.SaoBeMerchantNo;
            input.terminal_id = _options.SaoBeTerminalId;
        }

        private void Log(ResultBase result, string originalData)
        {
            if (result.return_code != "01")
            {
                _logger.LogError($"{result.return_msg}--{ originalData}");
            }
            else if (result.result_code != "01")
            {
                _logger.LogError($"{result.return_msg}--{ originalData}");
            }
            else
            {
                _logger.LogInformation(originalData);
            }
        }
    }
}
