using Egoal.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Egoal.Invoice
{
    public abstract class InvoiceServiceBase : IInvoiceService
    {
        private readonly InvoiceOptions _options;

        public InvoiceServiceBase(InvoiceOptions options)
        {
            _options = options;
        }

        public virtual async Task<InvoiceResponse> InvoiceAsync(InvoiceRequest request)
        {
            NormalizeInvoice(request);

            return await RequestInvoiceAsync(request);
        }

        protected virtual void NormalizeInvoice(InvoiceRequest request)
        {
            if (request.XSF_NSRSBH.IsNullOrEmpty())
            {
                request.XSF_NSRSBH = _options.XSF_NSRSBH;
            }
            if (request.XSF_MC.IsNullOrEmpty())
            {
                request.XSF_MC = _options.XSF_MC;
            }
            if (request.XSF_DZDH.IsNullOrEmpty())
            {
                request.XSF_DZDH = _options.XSF_DZDH;
            }
            if (request.XSF_YHZH.IsNullOrEmpty())
            {
                request.XSF_YHZH = _options.XSF_YHZH;
            }
            if (request.XSF_SJH.IsNullOrEmpty())
            {
                request.XSF_SJH = _options.XSF_SJH;
            }
            if (request.XSF_DZYX.IsNullOrEmpty())
            {
                request.XSF_DZYX = _options.XSF_Email;
            }
            if (request.KPR.IsNullOrEmpty())
            {
                request.KPR = _options.KPR;
            }
            if (request.FHR.IsNullOrEmpty())
            {
                request.FHR = _options.KPR;
            }

            foreach (var item in request.Items)
            {
                if (item.SPBM.IsNullOrEmpty())
                {
                    item.SPBM = _options.XSF_SPBM;
                }
                if (item.LSLBS == null)
                {
                    item.LSLBS = _options.SLBS;
                }
                if (!item.SL.HasValue)
                {
                    item.SL = item.LSLBS.IsNullOrEmpty() ? _options.XSF_SL : 0;
                    item.XMDJ = Math.Round(item.RealPrice / (1 + item.SL.Value), 6);
                    item.XMJE = Math.Round(item.XMDJ * item.XMSL, 2);
                    item.SE = Math.Round(item.XMJE * item.SL.Value, 2);
                }
            }

            request.HJJE = request.Items.Sum(i => i.XMJE);
            request.HJSE = request.Items.Sum(i => i.SE);
            request.JSHJ = request.HJJE + request.HJSE;
        }

        protected abstract Task<InvoiceResponse> RequestInvoiceAsync(InvoiceRequest request);

        public async Task<DownloadResponse> DownloadAsync(DownloadRequest request)
        {
            NormalizeDownload(request);

            return await RequestDownloadAsync(request);
        }

        protected virtual void NormalizeDownload(DownloadRequest request)
        {
            if (request.XSF_NSRSBH.IsNullOrEmpty())
            {
                request.XSF_NSRSBH = _options.XSF_NSRSBH;
            }
        }

        protected abstract Task<DownloadResponse> RequestDownloadAsync(DownloadRequest request);

        public async Task<QueryResponse> QueryAsync(QueryRequest request)
        {
            NormalizeQuery(request);

            return await RequestQueryAsync(request);
        }

        protected virtual void NormalizeQuery(QueryRequest request)
        {
            if (request.XSF_NSRSBH.IsNullOrEmpty())
            {
                request.XSF_NSRSBH = _options.XSF_NSRSBH;
            }
        }

        protected abstract Task<QueryResponse> RequestQueryAsync(QueryRequest request);
    }
}
