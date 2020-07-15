using Egoal.Threading.BackgroundWorkers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Egoal.WeChat.OAuth
{
    public class RefreshAccessTokenWorker : PeriodicBackgroundWorkerBase
    {
        private readonly WeChatOptions _options;
        private readonly OAuthApi _oAuthApi;

        public RefreshAccessTokenWorker(
            ILogger<RefreshAccessTokenWorker> logger,
            IOptions<WeChatOptions> options,
            OAuthApi oAuthApi)
            : base(logger)
        {
            _options = options.Value;
            _oAuthApi = oAuthApi;

            Period = TimeSpan.FromMinutes(10);
        }

        protected override async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            var token = await _oAuthApi.GetAccessTokenAsync();
            _options.AccessToken = token.access_token;

            var ticket = await _oAuthApi.GetJsApiTicketAsync(token.access_token);
            _options.JsApiTicket = ticket.ticket;

            Period = TimeSpan.FromSeconds(token.expires_in - 200);
        }
    }
}
