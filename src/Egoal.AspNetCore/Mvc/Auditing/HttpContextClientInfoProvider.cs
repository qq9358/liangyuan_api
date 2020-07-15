using Egoal.Auditing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

namespace Egoal.Mvc.Auditing
{
    public class HttpContextClientInfoProvider : IClientInfoProvider
    {
        public string BrowserInfo => GetBrowserInfo();

        public string ClientIpAddress => GetClientIpAddress();

        public string ComputerName => GetComputerName();

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly ILogger _logger;

        public HttpContextClientInfoProvider(
            IHttpContextAccessor httpContextAccessor,
            ILogger<HttpContextClientInfoProvider> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = httpContextAccessor.HttpContext;
            _logger = logger;
        }

        protected virtual string GetBrowserInfo()
        {
            var httpContext = _httpContextAccessor.HttpContext ?? _httpContext;
            return httpContext?.Request?.Headers?["User-Agent"];
        }

        protected virtual string GetClientIpAddress()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext ?? _httpContext;
                var ip = httpContext?.Connection?.RemoteIpAddress?.ToString();

                return ip == "::1" ? "127.0.0.1" : ip;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.ToString());
            }

            return null;
        }

        protected virtual string GetComputerName()
        {
            return null; //TODO: Implement!
        }
    }
}
