using Egoal.WeChat.Message;
using System.Threading.Tasks;

namespace Egoal.Messages
{
    public class WeChatMessageTemplateStore : ITemplateStore
    {
        private readonly IWeChatMessageTemplateRepository _weChatMessageTemplateRepository;

        public WeChatMessageTemplateStore(IWeChatMessageTemplateRepository weChatMessageTemplateRepository)
        {
            _weChatMessageTemplateRepository = weChatMessageTemplateRepository;
        }

        public async Task<string> GetTemplateIdAsync(string shortTemplateId)
        {
            return await _weChatMessageTemplateRepository.GetTemplateIdAsync(shortTemplateId);
        }

        public async Task SaveTemplateIdAsync(string shortTemplateId, string templateId)
        {
            var template = await _weChatMessageTemplateRepository.FirstOrDefaultAsync(t => t.ShortTemplateId == shortTemplateId);
            if (template == null)
            {
                template = new WeChatMessageTemplate();
                template.ShortTemplateId = shortTemplateId;
            }
            template.TemplateId = templateId;

            await _weChatMessageTemplateRepository.InsertOrUpdateAsync(template);
        }
    }
}
