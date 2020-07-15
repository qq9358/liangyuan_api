using Egoal.Dependency;
using Egoal.Events.Bus;
using Egoal.Extensions;
using Egoal.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Egoal.WeChat.Message
{
    public class MessageApi : ApiBase, ITransientDependency
    {
        private readonly WeChatOptions _options;

        public MessageApi(
            ILogger<MessageApi> logger,
            IEventBus eventBus,
            IOptions<WeChatOptions> options)
            : base(logger, eventBus)
        {
            _options = options.Value;
        }

        public async Task<TemplateIdResult> GetTemplateIdAsync(string shortTemplateId)
        {
            string url = $"https://api.weixin.qq.com/cgi-bin/template/api_add_template?access_token={_options.AccessToken}";

            var json = new { template_id_short = shortTemplateId }.ToJson();

            string response = await HttpHelper.PostJsonAsync(url, json);

            var result = response.JsonToObject<TemplateIdResult>();

            await EnsureSuccessAsync(result, $"{json}--{response}");

            return result;
        }

        public async Task<TemplateMessageResult> SendTemplateMessageAsync(TemplateMessageInput input)
        {
            string url = $"https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={_options.AccessToken}";

            string json = input.ToJson();

            string response = await HttpHelper.PostJsonAsync(url, json);

            var result = response.JsonToObject<TemplateMessageResult>();

            await EnsureSuccessAsync(result, $"{json}--{response}");

            return result;
        }
    }
}
