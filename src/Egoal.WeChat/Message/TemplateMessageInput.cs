using System.Collections.Generic;

namespace Egoal.WeChat.Message
{
    public class TemplateMessageInput
    {
        public TemplateMessageInput()
        {
            data = new Dictionary<string, TemplateMessageDataItem>();
        }

        public string touser { get; set; }
        public string template_id { get; set; }
        public string url { get; set; }
        public MiniProgramSetting miniprogram { get; set; }
        public Dictionary<string, TemplateMessageDataItem> data { get; set; }
    }
}
