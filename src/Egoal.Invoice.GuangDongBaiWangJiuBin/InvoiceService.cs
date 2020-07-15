using Egoal.AutoMapper;
using Egoal.Cryptography;
using Egoal.Extensions;
using Egoal.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Egoal.Invoice.GuangDongBaiWangJiuBin
{
    public class InvoiceService : InvoiceServiceBase
    {
        private readonly BaiWangOptions _options;
        private readonly ILogger _logger;

        public InvoiceService(
            IOptions<BaiWangOptions> options,
            ILogger<InvoiceService> logger)
            : base(options.Value)
        {
            _options = options.Value;
            _logger = logger;
        }

        protected override async Task<InvoiceResponse> RequestInvoiceAsync(InvoiceRequest input)
        {
            var body = input.MapTo<InvoiceInput>();
            body.DJBH = input.FPQQLSH;
            body.XSF_LXFS = input.XSF_DZYX ?? input.XSF_SJH;
            if (string.IsNullOrEmpty(input.GMF_SJH))
            {
                body.GMF_LXFS = input.GMF_DZYX;
            }
            else
            {
                body.GMF_LXFS = input.GMF_SJH;
            }

            var request = new Request();
            request.Id = "DZFPKJ";
            request.Comment = "电子发票开具";
            request.Body = body;

            var encoding = Encoding.UTF8;
            string xml = request.ToXml(encoding);
            string data = $"{input.XSF_NSRSBH},{_options.XSF_NSRMY},{Base64Helper.Encode(xml, encoding)}";

            string responseXml = string.Empty;
            try
            {
                responseXml = await HttpHelper.PostXmlAsync(_options.KP_ServiceUrl, data, encoding);

                responseXml = Base64Helper.Decode(responseXml, encoding);

                var output = InvoiceOutput.FromXml(responseXml);
                if (output.RETURNCODE == "0" || output.RETURNCODE == "0000")
                {
                    return output.ToResponse();
                }
                else
                {
                    _logger.LogError($"{output.RETURNMSG}--{request.ToXml(encoding, false)}--{responseXml}");

                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}--{request.ToXml(encoding, false)}--{responseXml}");

                throw;
            }
        }

        protected override async Task<DownloadResponse> RequestDownloadAsync(DownloadRequest input)
        {
            var body = input.MapTo<DownloadInput>();
            body.ACCESS_TOKEN = _options.ACCESS_TOKEN;
            body.KPRQ = input.KPRQ.ToString("yyyyMMddHHmmss");

            var request = new Request();
            request.Id = "FPCFDZ";
            request.Comment = "发票存放地址";
            request.Body = body;

            var encoding = Encoding.GetEncoding("gbk");
            string xml = request.ToXml(encoding);

            string responseXml = string.Empty;
            try
            {
                responseXml = await HttpHelper.PostXmlAsync(_options.KP_DownURL, xml, encoding);

                var output = DownloadOutput.FromXml(responseXml);
                if (output.RETURNCODE == "0" || output.RETURNCODE == "0000")
                {
                    return output.ToResponse();
                }
                else
                {
                    _logger.LogError($"{output.RETURNMSG}--{request.ToXml(encoding, false)}--{responseXml}");

                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}--{request.ToXml(encoding, false)}--{responseXml}");

                throw;
            }
        }

        protected override async Task<QueryResponse> RequestQueryAsync(QueryRequest input)
        {
            var body = new QueryInput();
            body.FPQQLSH = input.FPQQLSH;

            var request = new Request();
            request.Id = "FPCX";
            request.Comment = "发票查询";
            request.Body = body;

            var encoding = Encoding.UTF8;
            string xml = request.ToXml(encoding);
            string data = $"{input.XSF_NSRSBH},{_options.XSF_NSRMY},{Base64Helper.Encode(xml, encoding)}";

            string responseXml = string.Empty;
            try
            {
                responseXml = await HttpHelper.PostXmlAsync(_options.KP_ServiceUrl, data, encoding);
                if (responseXml.IsNullOrEmpty())
                {
                    _logger.LogError($"返回值为空--{request.ToXml(encoding, false)}--{responseXml}");

                    return null;
                }

                responseXml = Base64Helper.Decode(responseXml, encoding);

                var output = QueryOutput.FromXml(responseXml);
                if (output.RETURNCODE == "0" || output.RETURNCODE == "0000")
                {
                    return output.ToResponse();
                }
                else
                {
                    _logger.LogError($"{output.RETURNMSG}--{request.ToXml(encoding, false)}--{responseXml}");

                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}--{request.ToXml(encoding, false)}--{responseXml}");

                throw;
            }
        }

        public async Task<ValidateUserOutput> ValidateUserAsync()
        {
            if (_options.XSF_NSRSBH.IsNullOrEmpty() || _options.XSF_NSRMY.IsNullOrEmpty())
            {
                return null;
            }

            var body = new ValidateUserInput();
            body.XSF_NSRSBH = _options.XSF_NSRSBH;
            body.XSF_SN = _options.XSF_SN;

            var request = new Request();
            request.Id = "YHYZ";
            request.Comment = "用户验证";
            request.Body = body;

            var encoding = Encoding.GetEncoding("gbk");
            string xml = request.ToXml(encoding);

            string responseXml = string.Empty;
            try
            {
                responseXml = await HttpHelper.PostXmlAsync(_options.KP_DownURL, xml, encoding);

                var output = ValidateUserOutput.FromXml(responseXml);
                if (output.RETURNCODE == "0" || output.RETURNCODE == "0000")
                {
                    _options.ACCESS_TOKEN = output.ACCESS_TOKEN;

                    return output;
                }
                else
                {
                    _logger.LogError($"{output.RETURNMSG}--{request.ToXml(encoding, false)}--{responseXml}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}--{request.ToXml(encoding, false)}--{responseXml}");
            }

            return null;
        }
    }
}
