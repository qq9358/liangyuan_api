using Egoal.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Egoal.Payment.Alipay
{
    public class PayService : INetPayService
    {
        private readonly AlipayOptions _options;
        private readonly AlipayApi _alipayApi;
        private readonly string _notifyUrl;

        public PayService(
            IOptions<AlipayOptions> options,
            AlipayApi alipayApi)
        {
            _options = options.Value;
            _alipayApi = alipayApi;
            _notifyUrl = _options.WebApiUrl.UrlCombine("/Payment/AlipayNotify");
        }

        public Task<string> JsApiPayAsync(NetPayInput payInput)
        {
            throw new NotSupportedException("支付宝不支持微信JsApi支付");
        }

        public async Task<NetPayOutput> MicroPayAsync(NetPayInput input)
        {
            PayRequest payRequest = new PayRequest();
            payRequest.out_trade_no = input.ListNo;
            payRequest.scene = "bar_code";
            payRequest.auth_code = input.AuthCode;
            payRequest.subject = input.ProductInfo;
            payRequest.body = input.Attach;
            payRequest.total_amount = input.PayMoney;
            payRequest.timeout_express = GetTimeoutExpress(input);

            AlipayRequest alipayRequest = new AlipayRequest();
            alipayRequest.method = "alipay.trade.pay";
            alipayRequest.notify_url = _notifyUrl;
            alipayRequest.biz_content = payRequest.ToJson(false);

            PayResponse payResponse = await _alipayApi.ExecuteAsync<PayResponse>(alipayRequest);

            return payResponse.ToNetPayOutput();
        }

        public async Task<string> NativePayAsync(NetPayInput input)
        {
            PrecreateRequest precreateRequest = new PrecreateRequest();
            precreateRequest.out_trade_no = input.ListNo;
            precreateRequest.total_amount = input.PayMoney;
            precreateRequest.subject = input.ProductInfo;
            precreateRequest.body = input.Attach;
            precreateRequest.timeout_express = GetTimeoutExpress(input);

            AlipayRequest alipayRequest = new AlipayRequest();
            alipayRequest.method = "alipay.trade.precreate";
            alipayRequest.notify_url = _notifyUrl;
            alipayRequest.biz_content = precreateRequest.ToJson(false);

            PrecreateResponse precreateResponse = await _alipayApi.ExecuteAsync<PrecreateResponse>(alipayRequest);

            return precreateResponse.qr_code;
        }

        public async Task<string> H5PayAsync(NetPayInput input)
        {
            WapPayRequest wapPayRequest = new WapPayRequest();
            wapPayRequest.out_trade_no = input.ListNo;
            wapPayRequest.total_amount = input.PayMoney;
            wapPayRequest.subject = input.ProductInfo;
            wapPayRequest.body = input.Attach;
            wapPayRequest.product_code = "QUICK_WAP_WAY";
            wapPayRequest.timeout_express = GetTimeoutExpress(input);

            AlipayRequest alipayRequest = new AlipayRequest();
            alipayRequest.method = "alipay.trade.wap.pay";
            alipayRequest.return_url = input.ReturnUrl;
            alipayRequest.notify_url = _notifyUrl;
            alipayRequest.biz_content = wapPayRequest.ToJson(false);

            return await _alipayApi.PageExecuteAsync(alipayRequest);
        }

        private string GetTimeoutExpress(NetPayInput input)
        {
            var minutes = Math.Ceiling((input.PayExpireTime - DateTime.Now).TotalMinutes);

            return $"{Math.Max(minutes, 1).To<int>()}m";
        }

        public NotifyInput Notify(string data)
        {
            var request = _alipayApi.Notify(data);

            return request.ToNotifyInput();
        }

        public NotifyOutput GenerateNotifyResponse(bool success, string message = "", NetPayInput input = null)
        {
            return new NotifyOutput { Data = success ? "success" : message };
        }

        public async Task<QueryPayOutput> QueryPayAsync(QueryPayInput input)
        {
            QueryRequest queryRequest = new QueryRequest();
            queryRequest.out_trade_no = input.ListNo;
            queryRequest.trade_no = input.TransactionId;

            AlipayRequest alipayRequest = new AlipayRequest();
            alipayRequest.method = "alipay.trade.query";
            alipayRequest.biz_content = queryRequest.ToJson(false);

            QueryResponse queryResponse = await _alipayApi.ExecuteAsync<QueryResponse>(alipayRequest);

            return queryResponse.ToQueryPayOutput();
        }

        public async Task<ClosePayOutput> ClosePayAsync(ClosePayInput input)
        {
            CloseRequest closeRequest = new CloseRequest();
            closeRequest.out_trade_no = input.ListNo;
            closeRequest.trade_no = input.TransactionId;

            AlipayRequest alipayRequest = new AlipayRequest();
            alipayRequest.method = "alipay.trade.close";
            alipayRequest.biz_content = closeRequest.ToJson(false);

            CloseResponse closeResponse = await _alipayApi.ExecuteAsync<CloseResponse>(alipayRequest);

            return closeResponse.ToClosePayOutput();
        }

        public async Task<ReversePayOutput> ReversePayAsync(ReversePayInput input)
        {
            CancelRequest cancelRequest = new CancelRequest();
            cancelRequest.out_trade_no = input.ListNo;
            cancelRequest.trade_no = input.TransactionId;

            AlipayRequest alipayRequest = new AlipayRequest();
            alipayRequest.method = "alipay.trade.cancel";
            alipayRequest.biz_content = cancelRequest.ToJson(false);

            CancelResponse cancelResponse = await _alipayApi.ExecuteAsync<CancelResponse>(alipayRequest);

            return cancelResponse.ToReversePayOutput();
        }

        public async Task<RefundOutput> RefundAsync(RefundInput input)
        {
            RefundRequest refundRequest = new RefundRequest();
            refundRequest.out_trade_no = input.ListNo;
            refundRequest.trade_no = input.TransactionId;
            refundRequest.out_request_no = input.RefundListNo;
            refundRequest.refund_amount = input.RefundFee;

            AlipayRequest alipayRequest = new AlipayRequest();
            alipayRequest.method = "alipay.trade.refund";
            alipayRequest.biz_content = refundRequest.ToJson(false);

            RefundResponse refundResponse = await _alipayApi.ExecuteAsync<RefundResponse>(alipayRequest);

            var output = refundResponse.ToRefundOutput();
            if (output.RefundId.IsNullOrEmpty())
            {
                output.RefundId = input.RefundListNo;
            }

            return output;
        }

        public async Task<QueryRefundOutput> QueryRefundAsync(QueryRefundInput input)
        {
            QueryRefundRequest queryRefundRequest = new QueryRefundRequest();
            queryRefundRequest.out_trade_no = input.ListNo;
            queryRefundRequest.trade_no = input.TransactionId;
            queryRefundRequest.out_request_no = input.RefundListNo;

            AlipayRequest alipayRequest = new AlipayRequest();
            alipayRequest.method = "alipay.trade.fastpay.refund.query";
            alipayRequest.biz_content = queryRefundRequest.ToJson(false);

            QueryRefundResponse queryRefundResponse = await _alipayApi.ExecuteAsync<QueryRefundResponse>(alipayRequest);

            return queryRefundResponse.ToQueryRefundOutput();
        }
    }
}
