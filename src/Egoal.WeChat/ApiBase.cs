using Egoal.Events.Bus;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Egoal.WeChat
{
    public class ApiBase
    {
        private readonly ILogger _logger;
        private readonly IEventBus _eventBus;

        public ApiBase(ILogger logger, IEventBus eventBus)
        {
            _logger = logger;
            _eventBus = eventBus;
        }

        public async Task EnsureSuccessAsync(ResultBase result, string originalData)
        {
            if (result.errcode == 0)
            {
                _logger.LogInformation(originalData);
            }
            else
            {
                _logger.LogError($"{result.errmsg}--{originalData}");

                var isGetAccessTokenRequest = originalData.Contains("/cgi-bin/token");
                var invalidAccessTokenCodes = new[] { 40001, 40014, 41001, 42001, 42007 };
                if (invalidAccessTokenCodes.Contains(result.errcode) && !isGetAccessTokenRequest)
                {
                    await _eventBus.TriggerAsync(new AccessTokenInvalidEventData());
                }

                throw new ApiException(result.errmsg);
            }
        }
    }
}
