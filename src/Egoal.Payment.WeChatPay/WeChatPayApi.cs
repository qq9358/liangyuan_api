using Egoal.Extensions;
using Egoal.Net.Http;
using Egoal.WeChat;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Egoal.Payment.WeChatPay
{
    public class WeChatPayApi
    {
        private readonly WeChatOptions _options;
        private readonly ILogger _logger;

        public WeChatPayApi(
            IOptions<WeChatOptions> options,
            ILogger<WeChatPayApi> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public async Task<UnifiedOrderResult> UnifiedOrderAsync(PayData inputObj)
        {
            if (!inputObj.IsSet("out_trade_no"))
            {
                throw new ApiException("缺少统一支付接口必填参数out_trade_no");
            }
            else if (!inputObj.IsSet("body"))
            {
                throw new ApiException("缺少统一支付接口必填参数body");
            }
            else if (!inputObj.IsSet("total_fee"))
            {
                throw new ApiException("缺少统一支付接口必填参数total_fee");
            }
            else if (!inputObj.IsSet("trade_type"))
            {
                throw new ApiException("缺少统一支付接口必填参数trade_type");
            }

            if (inputObj.GetValue("trade_type")?.ToUpper() == "JSAPI" && !inputObj.IsSet("openid"))
            {
                throw new ApiException("统一支付接口中，缺少必填参数openid，trade_type为JSAPI时，openid为必填参数");
            }
            if (inputObj.GetValue("trade_type")?.ToUpper() == "NATIVE" && !inputObj.IsSet("product_id"))
            {
                throw new ApiException("统一支付接口中，缺少必填参数product_id，trade_type为JSAPI时，product_id为必填参数");
            }

            if (!inputObj.IsSet("notify_url"))
            {
                inputObj.SetValue("notify_url", _options.NotifyUrl.UrlCombine("WeChatNotify"));
            }
            if (!inputObj.IsSet("spbill_create_ip"))
            {
                inputObj.SetValue("spbill_create_ip", _options.IP);
            }
            SetCommonValue(inputObj);

            inputObj.SetValue("sign", inputObj.MakeSign(_options.WxApiKey));

            string xml = inputObj.ToXml();

            string url = _options.PayUrl.UrlCombine("pay/unifiedorder");
            string response = await HttpHelper.PostXmlAsync(url, xml);

            PayData result = new PayData();
            result.FromXml(response, _options.WxApiKey);

            Log(result, $"{xml}--{response}");

            var unifiedOrderResult = result.ToJson().JsonToObject<UnifiedOrderResult>();
            if (unifiedOrderResult.prepay_id.IsNullOrEmpty())
            {
                _logger.LogError("prepay_id为空", $"{xml}--{response}");
            }

            return unifiedOrderResult;
        }

        public async Task<MicroPayResult> MicroPayAsync(PayData inputObj)
        {
            if (!inputObj.IsSet("body"))
            {
                throw new ApiException("提交被扫支付API接口中，缺少必填参数body！");
            }
            else if (!inputObj.IsSet("out_trade_no"))
            {
                throw new ApiException("提交被扫支付API接口中，缺少必填参数out_trade_no！");
            }
            else if (!inputObj.IsSet("total_fee"))
            {
                throw new ApiException("提交被扫支付API接口中，缺少必填参数total_fee！");
            }
            else if (!inputObj.IsSet("auth_code"))
            {
                throw new ApiException("提交被扫支付API接口中，缺少必填参数auth_code！");
            }

            if (!inputObj.IsSet("spbill_create_ip"))
            {
                inputObj.SetValue("spbill_create_ip", _options.IP);
            }
            SetCommonValue(inputObj);

            inputObj.SetValue("sign", inputObj.MakeSign(_options.WxApiKey));

            string xml = inputObj.ToXml();

            string url = _options.PayUrl.UrlCombine("pay/micropay");
            string response = await HttpHelper.PostXmlAsync(url, xml);

            PayData result = new PayData();
            result.FromXml(response, _options.WxApiKey);

            Log(result, $"{xml}--{response}");

            var payResult = result.ToJson().JsonToObject<MicroPayResult>();
            return payResult;
        }

        public NotifyResult Notify(string xml)
        {
            var notifyData = new PayData();
            notifyData.FromXml(xml, _options.WxApiKey);

            Log(notifyData, xml);

            var result = notifyData.ToJson().JsonToObject<NotifyResult>();

            return result;
        }

        public async Task<QueryPayResult> QueryPayAsync(PayData inputObj)
        {
            if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id"))
            {
                throw new ApiException("订单查询接口中，out_trade_no、transaction_id至少填一个");
            }

            SetCommonValue(inputObj);

            inputObj.SetValue("sign", inputObj.MakeSign(_options.WxApiKey));

            string xml = inputObj.ToXml();

            string url = _options.PayUrl.UrlCombine("pay/orderquery");
            string response = await HttpHelper.PostXmlAsync(url, xml);

            PayData result = new PayData();
            result.FromXml(response, _options.WxApiKey);

            Log(result, $"{xml}--{response}");

            var queryOrderResult = result.ToJson().JsonToObject<QueryPayResult>();

            return queryOrderResult;
        }

        public async Task<ClosePayResult> ClosePayAsync(PayData inputObj)
        {
            if (!inputObj.IsSet("out_trade_no"))
            {
                throw new ApiException("关闭订单接口中，out_trade_no必填");
            }

            SetCommonValue(inputObj);

            inputObj.SetValue("sign", inputObj.MakeSign(_options.WxApiKey));

            string xml = inputObj.ToXml();

            string url = _options.PayUrl.UrlCombine("pay/closeorder");
            string response = await HttpHelper.PostXmlAsync(url, xml);

            PayData result = new PayData();
            result.FromXml(response, _options.WxApiKey);

            Log(result, $"{xml}--{response}");

            var closeOrderResult = result.ToJson().JsonToObject<ClosePayResult>();

            return closeOrderResult;
        }

        public async Task<ReversePayResult> ReversePayAsync(PayData inputObj)
        {
            if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id"))
            {
                throw new ApiException("撤销订单API接口中，参数out_trade_no和transaction_id必须填写一个");
            }
            if (_options.SslCert == null)
            {
                throw new ApiException("证书加载失败");
            }

            SetCommonValue(inputObj);

            inputObj.SetValue("sign", inputObj.MakeSign(_options.WxApiKey));

            string xml = inputObj.ToXml();

            string url = _options.PayUrl.UrlCombine("secapi/pay/reverse");
            string response = await HttpHelper.PostXmlAsync(url, xml, _options.SslCert);

            PayData result = new PayData();
            result.FromXml(response, _options.WxApiKey);

            Log(result, $"{xml}--{response}");

            var reverseResult = result.ToJson().JsonToObject<ReversePayResult>();

            return reverseResult;
        }

        public async Task<RefundResult> RefundAsync(PayData inputObj)
        {
            if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id"))
            {
                throw new ApiException("退款申请接口中，out_trade_no、transaction_id至少填一个！");
            }
            else if (!inputObj.IsSet("out_refund_no"))
            {
                throw new ApiException("退款申请接口中，缺少必填参数out_refund_no！");
            }
            else if (!inputObj.IsSet("total_fee"))
            {
                throw new ApiException("退款申请接口中，缺少必填参数total_fee！");
            }
            else if (!inputObj.IsSet("refund_fee"))
            {
                throw new ApiException("退款申请接口中，缺少必填参数refund_fee！");
            }
            if (_options.SslCert == null)
            {
                throw new ApiException("证书加载失败");
            }

            SetCommonValue(inputObj);

            inputObj.SetValue("sign", inputObj.MakeSign(_options.WxApiKey));

            string xml = inputObj.ToXml();

            string url = _options.PayUrl.UrlCombine("secapi/pay/refund");
            string response = await HttpHelper.PostXmlAsync(url, xml, _options.SslCert);

            PayData result = new PayData();
            result.FromXml(response, _options.WxApiKey);

            Log(result, $"{xml}--{response}");

            var refundResult = result.ToJson().JsonToObject<RefundResult>();

            return refundResult;
        }

        public async Task<QueryRefundResult> QueryRefundAsync(PayData inputObj)
        {
            if (!inputObj.IsSet("out_refund_no") && !inputObj.IsSet("out_trade_no") &&
                !inputObj.IsSet("transaction_id") && !inputObj.IsSet("refund_id"))
            {
                throw new ApiException("退款查询接口中，out_refund_no、out_trade_no、transaction_id、refund_id四个参数必填一个！");
            }

            SetCommonValue(inputObj);

            inputObj.SetValue("sign", inputObj.MakeSign(_options.WxApiKey));

            string xml = inputObj.ToXml();

            string url = _options.PayUrl.UrlCombine("pay/refundquery");
            string response = await HttpHelper.PostXmlAsync(url, xml);

            PayData result = new PayData();
            result.FromXml(response, _options.WxApiKey);

            Log(result, $"{xml}--{response}");

            var queryResult = result.ToJson().JsonToObject<QueryRefundResult>();
            return queryResult;
        }

        public async Task<PayData> DownloadBillAsync(PayData inputObj)
        {
            if (!inputObj.IsSet("bill_date"))
            {
                throw new ApiException("对账单接口中，缺少必填参数bill_date！");
            }

            SetCommonValue(inputObj);

            inputObj.SetValue("sign", inputObj.MakeSign(_options.WxApiKey));

            string xml = inputObj.ToXml();

            string url = _options.PayUrl.UrlCombine("pay/downloadbill");
            string response = await HttpHelper.PostXmlAsync(url, xml);

            PayData result = new PayData();
            if (response.Substring(0, 5) == "<xml>")
            {
                result.FromXml(response, _options.WxApiKey);
            }
            else
            {
                result.SetValue("result", response);
            }

            Log(result, $"{xml}--{response}");

            return result;
        }

        private void SetCommonValue(PayData inputObj)
        {
            inputObj.SetValue("appid", _options.WxAppID);//公众账号ID
            inputObj.SetValue("mch_id", _options.WxMch_ID);//商户号
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
        }

        private void Log(PayData result, string originalData)
        {
            if (result.GetValue("return_code")?.ToUpper() != "SUCCESS")
            {
                _logger.LogError($"{result.GetValue("return_msg")}--{ originalData}");
            }
            else if (result.GetValue("result_code")?.ToUpper() != "SUCCESS")
            {
                _logger.LogError($"{result.GetValue("err_code_des")}--{ originalData}");
            }
            else
            {
                _logger.LogInformation(originalData);
            }
        }

        public string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        public string GenerateNonceStr()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
