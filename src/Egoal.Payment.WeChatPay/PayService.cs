using Egoal.Extensions;
using Egoal.WeChat;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Egoal.Payment.WeChatPay
{
    public class PayService : INetPayService
    {
        private readonly WeChatOptions _options;
        private readonly WeChatPayApi _wxPayApi;

        public PayService(
            IOptions<WeChatOptions> options,
            WeChatPayApi payApi)
        {
            _options = options.Value;
            _wxPayApi = payApi;
        }

        public async Task<string> JsApiPayAsync(NetPayInput input)
        {
            PayData data = new PayData();
            data.SetValue("body", input.ProductInfo);
            data.SetValue("out_trade_no", input.ListNo);
            data.SetValue("total_fee", (input.PayMoney * 100).ToString("F0"));
            data.SetValue("trade_type", "JSAPI");
            data.SetValue("openid", input.OpenId);
            data.SetValue("spbill_create_ip", input.ClientIp);

            SetAttach(data, input);

            SetPayTime(data, input);

            var result = await _wxPayApi.UnifiedOrderAsync(data);

            PayData jsApiParam = new PayData();
            jsApiParam.SetValue("appId", _options.WxAppID);
            jsApiParam.SetValue("timeStamp", _wxPayApi.GenerateTimeStamp());
            jsApiParam.SetValue("nonceStr", _wxPayApi.GenerateNonceStr());
            jsApiParam.SetValue("package", $"prepay_id={result.prepay_id}");
            jsApiParam.SetValue("signType", "MD5");
            jsApiParam.SetValue("paySign", jsApiParam.MakeSign(_options.WxApiKey));

            string parameters = jsApiParam.ToJson();

            return parameters;
        }

        public async Task<NetPayOutput> MicroPayAsync(NetPayInput input)
        {
            PayData data = new PayData();
            data.SetValue("auth_code", input.AuthCode);
            data.SetValue("body", input.ProductInfo);
            data.SetValue("total_fee", (input.PayMoney * 100).ToString("F0"));
            data.SetValue("out_trade_no", input.ListNo);
            data.SetValue("spbill_create_ip", input.ClientIp);

            SetAttach(data, input);

            SetPayTime(data, input);

            var result = await _wxPayApi.MicroPayAsync(data);

            return result.ToPayOutput();
        }

        public async Task<string> NativePayAsync(NetPayInput input)
        {
            PayData data = new PayData();
            data.SetValue("body", input.ProductInfo);
            data.SetValue("out_trade_no", input.ListNo);
            data.SetValue("total_fee", (input.PayMoney * 100).ToString("F0"));
            data.SetValue("trade_type", "NATIVE");
            data.SetValue("product_id", input.ProductId);
            data.SetValue("spbill_create_ip", input.ClientIp);

            SetAttach(data, input);

            SetPayTime(data, input);

            var result = await _wxPayApi.UnifiedOrderAsync(data);

            return result.code_url;
        }

        public async Task<string> H5PayAsync(NetPayInput input)
        {
            PayData data = new PayData();
            data.SetValue("body", input.ProductInfo);
            data.SetValue("out_trade_no", input.ListNo);
            data.SetValue("total_fee", (input.PayMoney * 100).ToString("F0"));
            data.SetValue("trade_type", "MWEB");
            data.SetValue("spbill_create_ip", input.ClientIp);

            var h5Info = new
            {
                h5_info = new H5Info
                {
                    type = "Wap",
                    wap_url = input.ReturnUrl,
                    wap_name = input.WapName
                }
            };
            data.SetValue("scene_info", h5Info.ToJson());

            SetAttach(data, input);

            SetPayTime(data, input);

            var result = await _wxPayApi.UnifiedOrderAsync(data);

            return result.mweb_url;
        }

        private void SetAttach(PayData data, NetPayInput input)
        {
            if (input.Attach.IsNullOrEmpty()) return;

            data.SetValue("attach", input.Attach);
        }

        private void SetPayTime(PayData data, NetPayInput input)
        {
            var now = DateTime.Now;
            var minExpireTime = now.AddMinutes(_options.MinExpireMinutes);
            if (input.PayExpireTime < minExpireTime)
            {
                data.SetValue("time_start", now.ToString(WeChatOptions.DateTimeFormat));
                data.SetValue("time_expire", minExpireTime.ToString(WeChatOptions.DateTimeFormat));
            }
            else
            {
                data.SetValue("time_start", input.PayStartTime.ToString(WeChatOptions.DateTimeFormat));
                data.SetValue("time_expire", input.PayExpireTime.ToString(WeChatOptions.DateTimeFormat));
            }
        }

        public NotifyInput Notify(string xml)
        {
            var result = _wxPayApi.Notify(xml);

            return result.ToNotifyInput();
        }

        public NotifyOutput GenerateNotifyResponse(bool success, string message = "", NetPayInput input = null)
        {
            var data = new PayData();
            data.SetValue("return_code", success ? "SUCCESS" : "FAIL");
            data.SetValue("return_msg", success ? "OK" : message);

            var output = new NotifyOutput();
            output.ContentType = "text/xml";
            output.Data = data.ToXml();

            return output;
        }

        public async Task<QueryPayOutput> QueryPayAsync(QueryPayInput input)
        {
            PayData data = new PayData();
            if (!input.ListNo.IsNullOrEmpty())
            {
                data.SetValue("out_trade_no", input.ListNo);
            }
            if (!input.TransactionId.IsNullOrEmpty())
            {
                data.SetValue("transaction_id", input.TransactionId);
            }

            var result = await _wxPayApi.QueryPayAsync(data);

            return result.ToQueryPayOutput();
        }

        public async Task<ClosePayOutput> ClosePayAsync(ClosePayInput input)
        {
            PayData data = new PayData();
            data.SetValue("out_trade_no", input.ListNo);

            var result = await _wxPayApi.ClosePayAsync(data);
            return result.ToClosePayOutput();
        }

        public async Task<ReversePayOutput> ReversePayAsync(ReversePayInput input)
        {
            PayData data = new PayData();
            if (!input.ListNo.IsNullOrEmpty())
            {
                data.SetValue("out_trade_no", input.ListNo);
            }
            if (!input.TransactionId.IsNullOrEmpty())
            {
                data.SetValue("transaction_id", input.TransactionId);
            }

            var result = await _wxPayApi.ReversePayAsync(data);

            return result.ToReversePayOutput();
        }

        public async Task<RefundOutput> RefundAsync(RefundInput input)
        {
            PayData data = new PayData();
            data.SetValue("out_trade_no", input.ListNo);
            data.SetValue("out_refund_no", input.RefundListNo);
            data.SetValue("total_fee", (input.TotalFee * 100).ToString("F0"));
            data.SetValue("refund_fee", (input.RefundFee * 100).ToString("F0"));

            var result = await _wxPayApi.RefundAsync(data);

            return result.ToRefundOutput();
        }

        public async Task<QueryRefundOutput> QueryRefundAsync(QueryRefundInput input)
        {
            PayData data = new PayData();
            data.SetValue("out_trade_no", input.ListNo);
            data.SetValue("out_refund_no", input.RefundListNo);

            var result = await _wxPayApi.QueryRefundAsync(data);

            return result.ToQueryRefundOutput();
        }
    }
}
