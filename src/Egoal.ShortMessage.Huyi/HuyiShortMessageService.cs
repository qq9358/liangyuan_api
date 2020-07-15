using Egoal.Extensions;
using Egoal.Net.Http;
using Egoal.UI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Egoal.ShortMessage.Huyi
{
    public class HuyiShortMessageService : IShortMessageService
    {
        private readonly MessageOptions _options;
        private readonly ILogger _logger;

        public HuyiShortMessageService(
            IOptions<MessageOptions> options,
            ILogger<HuyiShortMessageService> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public string SupplierUrl { get; } = "http://www.ihuyi.com/";

        public int VariableMaxLength { get; } = 52;

        public int GetChargingQuantity(MessageInfo message)
        {
            int messageLength = message.Content.Length;
            if (!message.SignName.IsNullOrEmpty())
            {
                messageLength += message.SignName.Length + 2; //2代表签名两端的【】，签名由短信平台添加不包含在短信内容里面
            }

            if (messageLength <= 70)
            {
                return 1;
            }

            int chargingLength = 67;
            int chargingQuantity = messageLength / chargingLength;
            if (messageLength % chargingLength != 0)
            {
                chargingQuantity++;
            }

            return chargingQuantity;
        }

        public async Task SendAsync(MessageInfo message)
        {
            SendRequest request = new SendRequest();
            request.account = _options.ShortMessageUserName;
            request.password = _options.ShortMessagePassword;
            request.mobile = message.Mobile;
            request.content = message.Content;
            request.time = DateTime.Now.ToUnixTimestamp().ToString();

            string url = $"http://106.ihuyi.com/webservice/sms.php?method=Submit&{request.ToUrlArgs()}";

            var result = await HttpHelper.GetStringAsync(url);

            SendResponse response = result.JsonToObject<SendResponse>();

            if (response == null)
            {
                _logger.LogError($"短信发送失败：{url}");

                throw new UserFriendlyException("短信发送失败：返回结果异常");
            }

            if (response.code != 2)
            {
                _logger.LogError($"短信发送失败：{url}--{result}");

                throw new UserFriendlyException($"短信发送失败：{response.msg}");
            }
        }
    }
}
