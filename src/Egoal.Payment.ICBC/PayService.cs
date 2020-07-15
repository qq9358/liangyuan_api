using Egoal.Cryptography;
using Egoal.Extensions;
using Egoal.Payment.IcbcPay.Request;
using Egoal.Payment.IcbcPay.Response;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Egoal.Payment.IcbcPay
{
    public class PayService : INetPayService
    {
        private readonly ILogger _logger;
        private readonly IcbcPayOptions _options;
        private readonly IcbcPayApi _icbcPayApi;
        private readonly IcbcSignature _icbcSignature;

        private readonly string _notifyUrl;

        public PayService(
            ILogger<PayService> logger,
            IOptions<IcbcPayOptions> options,
            IcbcPayApi icbcPayApi,
            IcbcSignature icbcSignature)
        {
            _logger = logger;
            _options = options.Value;
            _icbcPayApi = icbcPayApi;
            _icbcSignature = icbcSignature;
            _notifyUrl = _options.WebApiUrl.UrlCombine("/Payment/IcbcPayNotify");
        }

        /// <summary>
        /// 聚合二维码
        /// </summary>
        /// <param name="payInput"></param>
        /// <returns></returns>
        public async Task<string> JsApiPayAsync(NetPayInput payInput)
        {
            JsApiPayRequest jsApiPayRequest = new JsApiPayRequest();
            jsApiPayRequest.interface_version = "1.0.0.1";
            jsApiPayRequest.mer_id = _options.IcbcMerchantId;
            jsApiPayRequest.tp_app_id = _options.WxAppID;
            jsApiPayRequest.tp_open_id = payInput.OpenId;
            jsApiPayRequest.out_trade_no = payInput.ListNo;
            jsApiPayRequest.tran_type = "OfflinePay";
            jsApiPayRequest.order_date = payInput.PayStartTime.ToString("yyyyMMddHHmmss");
            jsApiPayRequest.end_time = payInput.PayExpireTime.ToString("yyyyMMddHHmmss");
            jsApiPayRequest.goods_body = payInput.ProductInfo;
            jsApiPayRequest.attach = payInput.Attach;
            jsApiPayRequest.order_amount = (payInput.PayMoney * 100).ToString("F0");
            jsApiPayRequest.spbill_create_ip = payInput.ClientIp;
            jsApiPayRequest.install_times = "1";
            jsApiPayRequest.return_url = payInput.ReturnUrl;
            jsApiPayRequest.notify_url = _notifyUrl;
            jsApiPayRequest.notify_type = "HS";
            jsApiPayRequest.result_type = "1";

            var goodDetail = new
            {
                good_name = payInput.ProductInfo,
                good_id = "1001",
                good_num = "1"
            };
            jsApiPayRequest.goods_detail = goodDetail.ToJson();

            IcbcPayRequest icbcPayRequest = new IcbcPayRequest();
            icbcPayRequest.app_id = _options.IcbcAppId;
            icbcPayRequest.encrypt_type = "AES";

            var content = jsApiPayRequest.ToJson();
            _logger.LogInformation(content);
            icbcPayRequest.biz_content = _icbcSignature.EncryptContent(content, icbcPayRequest.encrypt_type, _options.IcbcAESKey);

            return await _icbcPayApi.PageExecuteAsync(icbcPayRequest.ToJson().JsonToObject<Dictionary<string, string>>(), "/ui/aggregate/payment/request/V2");
        }

        /// <summary>
        /// 二维码被扫支付
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<NetPayOutput> MicroPayAsync(NetPayInput input)
        {
            PayRequest payRequest = new PayRequest();
            payRequest.qr_code = input.AuthCode;
            payRequest.mer_id = _options.IcbcMerchantId;
            payRequest.out_trade_no = input.ListNo;
            payRequest.order_amt = (input.PayMoney * 100).ToString("F0");
            payRequest.trade_date = input.PayStartTime.ToString("yyyyMMdd");
            payRequest.trade_time = input.PayStartTime.ToString("HHmmss");

            IcbcPayRequest icbcPayRequest = new IcbcPayRequest();
            icbcPayRequest.biz_content = payRequest.ToJson();

            PayResponse payResponse = await _icbcPayApi.ExecuteAsync<PayResponse>(icbcPayRequest, "/api/qrcode/V2/pay");

            return payResponse.ToNetPayOutput();
        }

        /// <summary>
        /// 二维码生成
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<string> NativePayAsync(NetPayInput input)
        {
            QrcodeGenerateRequest generateRequest = new QrcodeGenerateRequest();
            generateRequest.mer_id = _options.IcbcMerchantId;
            generateRequest.store_code = _options.IcbcEMerchantId;
            generateRequest.out_trade_no = input.ListNo;
            generateRequest.order_amt = (input.PayMoney * 100).ToString("F0");
            generateRequest.trade_date = input.PayStartTime.ToString("yyyyMMdd");
            generateRequest.trade_time = input.PayStartTime.ToString("HHmmss");
            generateRequest.attach = input.Attach;
            generateRequest.pay_expire = GetPayExpire(input).ToString();
            generateRequest.notify_url = _notifyUrl;
            generateRequest.tporder_create_ip = input.ClientIp;
            generateRequest.sp_flag = "0";
            generateRequest.notify_flag = "1";

            IcbcPayRequest icbcPayRequest = new IcbcPayRequest();
            icbcPayRequest.biz_content = generateRequest.ToJson();

            QrcodeGenerateResponse generateResponse = await _icbcPayApi.ExecuteAsync<QrcodeGenerateResponse>(icbcPayRequest, "/api/qrcode/V2/generate");

            return generateResponse.qrcode;
        }

        /// <summary>
        /// H5移动在线支付
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<string> H5PayAsync(NetPayInput input)
        {
            OrderInfo orderInfo = new OrderInfo();
            orderInfo.order_date = input.PayStartTime.ToString("yyyyMMddHHmmss");
            orderInfo.order_id = input.ListNo;
            orderInfo.amount = (input.PayMoney * 100).ToString("F0");
            orderInfo.installment_times = "1";
            orderInfo.cur_type = "001";
            orderInfo.mer_id = _options.IcbcH5MallCode;
            orderInfo.mer_acct = _options.IcbcH5ClearingAccount;

            Custom custom = new Custom();
            custom.verify_join_flag = "0";
            custom.language = "zh-CN";

            var encoding = Encoding.GetEncoding("GBK");

            Message message = new Message();
            message.goods_name = Base64Helper.Encode(input.ProductInfo, encoding);
            message.goods_num = "1";
            message.carriage_amt = "0";
            message.remark1 = input.PayExpireTime.ToString("yyyyMMddHHmmss");
            message.credit_type = "2";
            message.mer_reference = new Uri(input.ReturnUrl).Host;
            message.mer_var = Base64Helper.Encode(input.Attach, encoding);
            message.notify_type = "HS";
            message.result_type = "1";
            message.return_url = input.ReturnUrl;
            message.auto_refer_sec = "5";

            H5PayRequest h5PayRequest = new H5PayRequest();
            h5PayRequest.app_id = _options.IcbcH5AppId;
            h5PayRequest.sign_type = "CA";
            h5PayRequest.notify_url = _notifyUrl;
            h5PayRequest.ca = _options.IcbcCAPublicKey;
            h5PayRequest.interfaceName = "ICBC_WAPB_B2C";
            h5PayRequest.interfaceVersion = "1.0.0.6";
            h5PayRequest.clientType = "0";

            var content = new
            {
                order_info = orderInfo,
                custom,
                message
            };
            h5PayRequest.biz_content = content.ToJson();

            return await _icbcPayApi.PageExecuteAsync(h5PayRequest.ToJson().JsonToObject<Dictionary<string, string>>(), "/ui/b2c/pay/transfer/V2");
        }

        /// <summary>
        /// 二维码异步商户通知接口
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public NotifyInput Notify(string data)
        {
            NotifyRequest notifyRequest = _icbcPayApi.Notify(data, _notifyUrl);

            return notifyRequest.ToNotifyInput();
        }

        /// <summary>
        /// 通知返回结果处理
        /// </summary>
        /// <param name="success"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public NotifyOutput GenerateNotifyResponse(bool success, string message = "", NetPayInput input = null)
        {
            return new NotifyOutput { Data = _icbcPayApi.GenerateNotifyResponse(success, message, input) };
        }

        /// <summary>
        /// 二维码查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<QueryPayOutput> QueryPayAsync(QueryPayInput input)
        {
            if (input.TradeType == OnlinePayTradeType.MWEB)
            {
                return await QueryB2COrderAsync(input);
            }

            QueryRequest queryRequest = new QueryRequest();
            queryRequest.mer_id = _options.IcbcMerchantId;
            queryRequest.out_trade_no = input.ListNo;
            queryRequest.order_id = input.TransactionId;

            IcbcPayRequest icbcPayRequest = new IcbcPayRequest();
            icbcPayRequest.biz_content = queryRequest.ToJson();

            QueryResponse queryResponse = await _icbcPayApi.ExecuteAsync<QueryResponse>(icbcPayRequest, "/api/qrcode/V2/query");

            return queryResponse.ToQueryPayOutput();
        }

        private async Task<QueryPayOutput> QueryB2COrderAsync(QueryPayInput input)
        {
            QueryB2CRequest requestContent = new QueryB2CRequest();
            requestContent.consumer_id = "行外商户_查询支付订单";
            requestContent.merid = _options.IcbcH5MallCode;
            requestContent.orderid = input.ListNo;
            requestContent.tdate = input.PayTime.ToString("yyyy-MM-dd");
            requestContent.order_status = "0";
            requestContent.orderid_type = "0";

            IcbcPayRequest icbcPayRequest = new IcbcPayRequest();
            icbcPayRequest.app_id = _options.IcbcH5AppId;
            icbcPayRequest.sign_type = "CA";
            icbcPayRequest.ca = _options.IcbcCAPublicKey;
            icbcPayRequest.biz_content = requestContent.ToJson();

            var queryResponse = await _icbcPayApi.ExecuteAsync<QueryB2CResponse>(icbcPayRequest, "/api/b2c/orderqry/search/V1");

            return queryResponse.ToQueryPayOutput();
        }

        /// <summary>
        /// 二维码关闭交易
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ClosePayOutput> ClosePayAsync(ClosePayInput input)
        {
            ReversePayInput reversePayInput = new ReversePayInput();
            reversePayInput.ListNo = input.ListNo;
            reversePayInput.TransactionId = input.TransactionId;
            reversePayInput.PayTime = input.PayTime;

            ReversePayOutput reversePayOutput = await ReversePayAsync(reversePayInput);

            ClosePayOutput closePayOutput = new ClosePayOutput();
            closePayOutput.Success = reversePayOutput.Success;
            closePayOutput.IsPaid = false;

            return closePayOutput;
        }

        /// <summary>
        /// 二维码冲正
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ReversePayOutput> ReversePayAsync(ReversePayInput input)
        {
            ReverseRequest reverseRequest = new ReverseRequest();
            reverseRequest.mer_id = _options.IcbcMerchantId;
            reverseRequest.out_trade_no = input.ListNo;
            reverseRequest.order_id = input.TransactionId;

            IcbcPayRequest icbcPayRequest = new IcbcPayRequest();
            icbcPayRequest.biz_content = reverseRequest.ToJson();

            ReverseResponse reverseResponse = await _icbcPayApi.ExecuteAsync<ReverseResponse>(icbcPayRequest, "/api/qrcode/V2/reverse");

            return reverseResponse.ToReversePayOutput();
        }

        /// <summary>
        /// 二维码退款
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<RefundOutput> RefundAsync(RefundInput input)
        {
            if (input.TradeType == OnlinePayTradeType.MWEB)
            {
                return await RefundB2CAsync(input);
            }

            RejectRequest rejectRequest = new RejectRequest();
            rejectRequest.mer_id = _options.IcbcMerchantId;
            rejectRequest.out_trade_no = input.ListNo;
            rejectRequest.order_id = input.TransactionId;
            rejectRequest.reject_no = input.RefundListNo;
            rejectRequest.reject_amt = (input.RefundFee * 100).ToString("F0");

            IcbcPayRequest icbcPayRequest = new IcbcPayRequest();
            icbcPayRequest.biz_content = rejectRequest.ToJson();

            RejectResponse rejectResponse = await _icbcPayApi.ExecuteAsync<RejectResponse>(icbcPayRequest, "/api/qrcode/V2/reject");

            return rejectResponse.ToRefundOutput();
        }

        private async Task<RefundOutput> RefundB2CAsync(RefundInput input)
        {
            RefundB2CRequest refundRequest = new RefundB2CRequest();
            refundRequest.functionID = $"{_options.IcbcH5AppId}-B2CRefund";
            refundRequest.onLine_merID = _options.IcbcH5MallCode;
            refundRequest.payDate = input.PayTime.ToString("yyyyMMdd");
            refundRequest.orderNum = input.ListNo;
            refundRequest.emallRejectId = input.RefundListNo;
            refundRequest.rejectAmt = (input.RefundFee * 100).ToString("F0");
            refundRequest.orderNumType = "0";

            IcbcPayRequest icbcPayRequest = new IcbcPayRequest();
            icbcPayRequest.app_id = _options.IcbcH5AppId;
            icbcPayRequest.sign_type = "CA";
            icbcPayRequest.ca = _options.IcbcCAPublicKey;
            icbcPayRequest.biz_content = refundRequest.ToJson();

            var refundResponse = await _icbcPayApi.ExecuteAsync<RefundB2CResponse>(icbcPayRequest, "/api/enterprise/merctserv/refund/V1");

            return refundResponse.ToRefundOutput();
        }

        /// <summary>
        /// 二维码退款查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<QueryRefundOutput> QueryRefundAsync(QueryRefundInput input)
        {
            if (input.TradeType == OnlinePayTradeType.MWEB)
            {
                return await QueryB2CRefundAsync(input);
            }

            RejectQueryRequest rejectQueryRequest = new RejectQueryRequest();
            rejectQueryRequest.mer_id = _options.IcbcMerchantId;
            rejectQueryRequest.out_trade_no = input.ListNo;
            rejectQueryRequest.order_id = input.TransactionId;
            rejectQueryRequest.reject_no = input.RefundListNo;

            IcbcPayRequest icbcPayRequest = new IcbcPayRequest();
            icbcPayRequest.biz_content = rejectQueryRequest.ToJson();

            RejectQueryResponse rejectQueryResponse = await _icbcPayApi.ExecuteAsync<RejectQueryResponse>(icbcPayRequest, "/api/qrcode/reject/query/V3");

            return rejectQueryResponse.ToQueryRefundOutput(input.RefundListNo);
        }

        private async Task<QueryRefundOutput> QueryB2CRefundAsync(QueryRefundInput input)
        {
            QueryB2CRefundRequest queryRequest = new QueryB2CRefundRequest();
            queryRequest.functionID = $"{_options.IcbcH5AppId}-B2CRefund";
            queryRequest.onLine_merID = _options.IcbcH5MallCode;
            queryRequest.orderNum = input.ListNo;
            queryRequest.emallRejectId = input.RefundListNo;
            queryRequest.serialNo = input.RefundId;
            queryRequest.rejectType = "2";

            IcbcPayRequest icbcPayRequest = new IcbcPayRequest();
            icbcPayRequest.app_id = _options.IcbcH5AppId;
            icbcPayRequest.sign_type = "CA";
            icbcPayRequest.ca = _options.IcbcCAPublicKey;
            icbcPayRequest.biz_content = queryRequest.ToJson();

            var queryResponse = await _icbcPayApi.ExecuteAsync<QueryB2CRefundResponse>(icbcPayRequest, "/api/enterprise/merctserv/qryrefund/V1");

            return queryResponse.ToQueryRefundOutput();
        }

        private int GetPayExpire(NetPayInput input)
        {
            var expire = (input.PayExpireTime - input.PayStartTime).TotalSeconds;
            if (expire <= 0)
            {
                expire = 60;
            }

            return (int)expire;
        }
    }
}
