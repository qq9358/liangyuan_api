using Egoal.Dependency;
using Egoal.Extensions;
using System.Threading.Tasks;

namespace Egoal.WeChat.Message
{
    public class MessageService : ITransientDependency
    {
        private readonly MessageApi _messageApi;
        private readonly ITemplateStore _templateStore;

        public MessageService(MessageApi messageApi, ITemplateStore templateStore)
        {
            _messageApi = messageApi;
            _templateStore = templateStore;
        }

        /// <summary>
        /// 发送支付成功消息
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="listNo"></param>
        /// <param name="totalMoney"></param>
        /// <param name="productInfo"></param>
        /// <param name="templateUrl"></param>
        /// <returns></returns>
        public async Task<string> SendPaySuccessMessageAsync(string openId, string listNo, string totalMoney, string productInfo, string templateUrl)
        {
            string shortTemplateId = "OPENTM200921787";

            TemplateMessageInput input = new TemplateMessageInput();
            input.touser = openId;
            input.template_id = await GetTemplateIdAsync(shortTemplateId);
            input.url = templateUrl;
            input.data.Add("first", new TemplateMessageDataItem { value = "你已支付成功" });
            input.data.Add("keyword1", new TemplateMessageDataItem { value = listNo });
            input.data.Add("keyword2", new TemplateMessageDataItem { value = productInfo });
            input.data.Add("keyword3", new TemplateMessageDataItem { value = $"{totalMoney}元", color = "#008400" });
            input.data.Add("remark", new TemplateMessageDataItem { value = "你购买的门票已支付成功，查看详情了解更多信息。" });

            var result = await _messageApi.SendTemplateMessageAsync(input);

            return result.msgid;
        }

        /// <summary>
        /// 发送审核通知消息
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="title"></param>
        /// <param name="userName"></param>
        /// <param name="mobile"></param>
        /// <param name="date"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public async Task<string> SendAuditMessageAsync(string openId, string title, string userName, string mobile, string date, string remark)
        {
            string shortTemplateId = "OPENTM407362300";

            TemplateMessageInput input = new TemplateMessageInput();
            input.touser = openId;
            input.template_id = await GetTemplateIdAsync(shortTemplateId);
            input.data.Add("first", new TemplateMessageDataItem { value = title });
            input.data.Add("keyword1", new TemplateMessageDataItem { value = userName });
            input.data.Add("keyword2", new TemplateMessageDataItem { value = mobile });
            input.data.Add("keyword3", new TemplateMessageDataItem { value = date });
            input.data.Add("remark", new TemplateMessageDataItem { value = remark });

            var result = await _messageApi.SendTemplateMessageAsync(input);

            return result.msgid;
        }

        /// <summary>
        /// 发送验证码消息
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="number"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public async Task<string> SendVerificationCodeMessageAsync(string openId, string number, string remark)
        {
            string shortTemplateId = "TM00178";

            TemplateMessageInput input = new TemplateMessageInput();
            input.touser = openId;
            input.template_id = await GetTemplateIdAsync(shortTemplateId);
            input.data.Add("first", new TemplateMessageDataItem { value = "尊敬的客户" });
            input.data.Add("number", new TemplateMessageDataItem { value = number });
            input.data.Add("remark", new TemplateMessageDataItem { value = remark });

            var result = await _messageApi.SendTemplateMessageAsync(input);

            return result.msgid;
        }

        private async Task<string> GetTemplateIdAsync(string shortTemplateId)
        {
            string templateId = await _templateStore.GetTemplateIdAsync(shortTemplateId);
            if (templateId.IsNullOrEmpty())
            {
                var templateIdResult = await _messageApi.GetTemplateIdAsync(shortTemplateId);

                templateId = templateIdResult.template_id;

                await _templateStore.SaveTemplateIdAsync(shortTemplateId, templateId);
            }

            return templateId;
        }
    }
}
