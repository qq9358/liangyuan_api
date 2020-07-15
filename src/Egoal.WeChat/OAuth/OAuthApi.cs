using Egoal.Dependency;
using Egoal.Events.Bus;
using Egoal.Extensions;
using Egoal.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Egoal.WeChat.OAuth
{
    public class OAuthApi : ApiBase, ITransientDependency
    {
        private readonly WeChatOptions _options;

        public OAuthApi(
            ILogger<OAuthApi> logger,
            IEventBus eventBus,
            IOptions<WeChatOptions> options)
            : base(logger, eventBus)
        {
            _options = options.Value;
        }

        public async Task<AccessTokenResult> GetAccessTokenAsync()
        {
            string domainUrl = _options.WxSaleUrl.IsNullOrEmpty() ? "https://api.weixin.qq.com" : _options.WxSaleUrl.UrlCombine("wxapi");
            string url = domainUrl.UrlCombine($"/cgi-bin/token?grant_type=client_credential&appid={_options.WxAppID}&secret={_options.WxAppsecret}");

            string response = await HttpHelper.GetStringAsync(url);

            var result = response.JsonToObject<AccessTokenResult>();

            await EnsureSuccessAsync(result, $"{url}--{response}");

            return result;
        }

        public async Task<UserAccessTokenResult> GetUserAccessTokenAsync(string code)
        {
            string url = $"https://api.weixin.qq.com/sns/oauth2/access_token?appid={_options.WxAppID}&secret={_options.WxAppsecret}&code={code}&grant_type=authorization_code";

            string response = await HttpHelper.GetStringAsync(url);

            var result = response.JsonToObject<UserAccessTokenResult>();

            await EnsureSuccessAsync(result, $"{url}--{response}");

            return result;
        }

        public async Task<Code2SessionResult> Code2SessionAsync(string code)
        {
            string url = $"https://api.weixin.qq.com/sns/jscode2session?appid={_options.WxMiniprogramAppID}&secret={_options.WxMiniprogramAppSecret}&js_code={code}&grant_type=authorization_code";

            string response = await HttpHelper.GetStringAsync(url);

            var result = response.JsonToObject<Code2SessionResult>();
            await EnsureSuccessAsync(result, $"{url}--{response}");

            return result;
        }

        public async Task<UserAccessTokenResult> RefreshUserAccessTokenAsync(string refreshToken)
        {
            string url = $"https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={_options.WxAppID}&grant_type=refresh_token&refresh_token={refreshToken}";

            string response = await HttpHelper.GetStringAsync(url);

            var result = response.JsonToObject<UserAccessTokenResult>();

            await EnsureSuccessAsync(result, $"{url}--{response}");

            return result;
        }

        public async Task<UserInfoResult> GetUserInfoAsync(string accessToken, string openId)
        {
            string url = $"https://api.weixin.qq.com/sns/userinfo?access_token={accessToken}&openid={openId}&lang=zh_CN";

            string response = await HttpHelper.GetStringAsync(url);

            var result = response.JsonToObject<UserInfoResult>();

            await EnsureSuccessAsync(result, $"{url}--{response}");

            return result;
        }

        public async Task<JsApiTicketResult> GetJsApiTicketAsync(string accessToken)
        {
            string url = $"https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={accessToken}&type=jsapi";

            string response = await HttpHelper.GetStringAsync(url);

            var result = response.JsonToObject<JsApiTicketResult>();

            await EnsureSuccessAsync(result, $"{url}--{response}");

            return result;
        }
    }
}
