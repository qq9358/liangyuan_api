using Egoal.Extensions;
using Egoal.Threading.BackgroundWorkers;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Egoal.Invoice.GuangDongBaiWangJiuBin
{
    public class RefreshAccessTokenWorker : PeriodicBackgroundWorkerBase
    {
        private readonly InvoiceService _invoiceService;

        public RefreshAccessTokenWorker(
            ILogger<RefreshAccessTokenWorker> logger,
            InvoiceService invoiceService)
            : base(logger)
        {
            _invoiceService = invoiceService;

            Period = TimeSpan.FromMinutes(10);
        }

        protected override async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            var output = await _invoiceService.ValidateUserAsync();
            if (output != null)
            {
                var endTime = output.ACCESS_TOKEN_TIME.ToDateTime("yyyyMMdd HH:mm:ss");

                Period = TimeSpan.FromSeconds((endTime - DateTime.Now).TotalSeconds - 200);
            }
        }
    }
}
