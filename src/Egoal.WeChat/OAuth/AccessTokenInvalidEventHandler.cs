using Egoal.Dependency;
using Egoal.Events.Bus.Handlers;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Egoal.WeChat.OAuth
{
    public class AccessTokenInvalidEventHandler : IAsyncEventHandler<AccessTokenInvalidEventData>, IScopedDependency
    {
        private readonly WeChatOptions _options;
        private readonly OAuthApi _oAuthApi;

        public AccessTokenInvalidEventHandler(
            IOptions<WeChatOptions> options,
            OAuthApi oAuthApi)
        {
            _options = options.Value;
            _oAuthApi = oAuthApi;
        }

        public async Task HandleEventAsync(AccessTokenInvalidEventData eventData)
        {
            var token = await _oAuthApi.GetAccessTokenAsync();
            _options.AccessToken = token.access_token;
        }
    }
}
