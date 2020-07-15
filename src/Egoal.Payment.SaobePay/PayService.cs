using Egoal.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Egoal.Payment.SaobePay
{
    public class PayService : INetPayService
    {
        private readonly SaobePayOptions _options;
        private readonly SaobePayApi _payApi;

        public PayService(
            IOptions<SaobePayOptions> options,
            SaobePayApi payApi)
        {
            _options = options.Value;
            _payApi = payApi;
        }

        public async Task<string> JsApiPayAsync(NetPayInput input)
        {
            var request = new JsApiPayRequest();
            request.pay_type = "010";
            request.terminal_trace = input.ListNo;
            request.terminal_time = input.PayStartTime.ToString(SaobePayOptions.DateTimeFormat);
            request.total_fee = (input.PayMoney * 100).ToString("F0");
            request.open_id = input.OpenId;
            request.order_body = input.ProductInfo;
            request.attach = input.Attach;

            var result = await _payApi.JsApiPayAsync(request);

            return new { result.appId, result.timeStamp, result.nonceStr, package = result.package_str, result.signType, result.paySign }.ToJson();
        }

        public async Task<NetPayOutput> MicroPayAsync(NetPayInput input)
        {
            var request = new MicroPayRequest();
            request.pay_type = GetPayType(input.SubPayTypeId);
            request.terminal_trace = input.ListNo;
            request.terminal_time = input.PayStartTime.ToString(SaobePayOptions.DateTimeFormat);
            request.auth_no = input.AuthCode;
            request.total_fee = (input.PayMoney * 100).ToString("F0");
            request.order_body = input.ProductInfo;
            request.attach = input.Attach;

            var result = await _payApi.MicroPayAsync(request);

            return result.ToPayOutput();
        }

        public async Task<string> NativePayAsync(NetPayInput input)
        {
            var request = new NativePayRequest();
            request.pay_type = input.SubPayTypeId;
            request.terminal_trace = input.ListNo;
            request.terminal_time = input.PayStartTime.ToString(SaobePayOptions.DateTimeFormat);
            request.total_fee = (input.PayMoney * 100).ToString("F0");
            request.order_body = input.ProductInfo;
            request.attach = input.Attach;

            var result = await _payApi.NativePayAsync(request);

            return result.qr_code;
        }

        public Task<string> H5PayAsync(NetPayInput input)
        {
            throw new NotImplementedException();
        }

        public NotifyInput Notify(string data)
        {
            var result = _payApi.Notify(data);

            return result.ToNotifyInput();
        }

        public NotifyOutput GenerateNotifyResponse(bool success, string message = "", NetPayInput input = null)
        {
            var output = new NotifyOutput();
            output.ContentType = "application/json";
            output.Data = new { return_code = success ? "01" : "02", return_msg = success ? "成功" : message }.ToJson();

            return output;
        }

        public async Task<QueryPayOutput> QueryPayAsync(QueryPayInput input)
        {
            var request = new QueryOrderRequest();
            request.pay_type = GetPayType(input.SubPayTypeId);
            request.terminal_trace = Guid.NewGuid().ToString().Replace("-", string.Empty);
            request.terminal_time = DateTime.Now.ToString(SaobePayOptions.DateTimeFormat);
            request.pay_trace = input.ListNo;
            request.pay_time = input.PayTime.ToString(SaobePayOptions.DateTimeFormat);
            request.out_trade_no = input.TransactionId;

            var result = await _payApi.QueryPayAsync(request);

            return result.ToQueryPayOutput();
        }

        public async Task<ClosePayOutput> ClosePayAsync(ClosePayInput input)
        {
            var request = new CloseOrderRequest();
            request.pay_type = "010";
            request.terminal_trace = Guid.NewGuid().ToString().Replace("-", string.Empty);
            request.terminal_time = DateTime.Now.ToString(SaobePayOptions.DateTimeFormat);
            request.pay_trace = input.ListNo;
            request.pay_time = input.PayTime.ToString(SaobePayOptions.DateTimeFormat);
            request.out_trade_no = input.TransactionId;

            var result = await _payApi.ClosePayAsync(request);

            return result.ToCloseOrderOutput();
        }

        public async Task<ReversePayOutput> ReversePayAsync(ReversePayInput input)
        {
            var request = new ReverseRequest();
            request.pay_type = GetPayType(input.SubPayTypeId);
            request.terminal_trace = Guid.NewGuid().ToString().Replace("-", string.Empty);
            request.terminal_time = DateTime.Now.ToString(SaobePayOptions.DateTimeFormat);
            request.out_trade_no = input.TransactionId;
            request.pay_trace = input.ListNo;
            request.pay_time = input.PayTime.ToString(SaobePayOptions.DateTimeFormat);

            var result = await _payApi.ReversePayAsync(request);

            return result.ToReverseOutput();
        }

        public async Task<RefundOutput> RefundAsync(RefundInput input)
        {
            var request = new RefundRequest();
            request.pay_type = GetPayType(input.SubPayTypeId);
            request.terminal_trace = input.RefundListNo;
            request.terminal_time = DateTime.Now.ToString(SaobePayOptions.DateTimeFormat);
            request.refund_fee = (input.RefundFee * 100).ToString("F0");
            request.out_trade_no = input.TransactionId;
            request.pay_trace = input.ListNo;
            request.pay_time = input.PayTime.ToString(SaobePayOptions.DateTimeFormat);

            var result = await _payApi.RefundAsync(request);

            return result.ToRefundOutput();
        }

        public async Task<QueryRefundOutput> QueryRefundAsync(QueryRefundInput input)
        {
            var request = new QueryRefundRequest();
            request.pay_type = GetPayType(input.SubPayTypeId);
            request.terminal_trace = Guid.NewGuid().ToString().Replace("-", string.Empty);
            request.terminal_time = DateTime.Now.ToString(SaobePayOptions.DateTimeFormat);
            request.pay_trace = input.ListNo;
            request.pay_time = input.PayTime.ToString(SaobePayOptions.DateTimeFormat);
            request.out_refund_no = input.RefundListNo;

            var result = await _payApi.QueryRefundAsync(request);

            return result.ToQueryRefundOutput();

            //var request = new RefundInput();
            //request.SubPayTypeId = input.SubPayTypeId;
            //request.RefundListNo = input.RefundListNo;
            //request.RefundFee = input.RefundFee;
            //request.TransactionId = input.TransactionId;
            //request.ListNo = input.ListNo;
            //request.PayTime = input.PayTime;

            //var result = await RefundAsync(request);

            //var output = new QueryRefundOutput();
            //output.ListNo = result.ListNo;
            //output.RefundListNo = result.RefundListNo;
            //output.RefundFee = result.RefundFee;
            //output.RefundTime = result.RefundTime;
            //output.ErrorMessage = result.ErrorMessage;
            //output.Success = result.Success;
            //output.ShouldRetry = result.ShouldRetry;
            //output.IsExist = true;

            //return output;
        }

        public async Task<string> RegisterAsync()
        {
            var request = new RegisterRequest();
            request.terminal_trace = Guid.NewGuid().ToString().Replace("-", string.Empty);
            request.terminal_time = DateTime.Now.ToString(SaobePayOptions.DateTimeFormat);

            var result = await _payApi.RegisterAsync(request);

            return result.access_token;
        }

        private string GetPayType(string subPayTypeId)
        {
            return subPayTypeId.IsNullOrEmpty() ? "000" : subPayTypeId;
        }
    }
}
