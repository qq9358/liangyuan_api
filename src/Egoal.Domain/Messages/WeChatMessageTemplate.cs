using Egoal.Domain.Entities;

namespace Egoal.Messages
{
    public class WeChatMessageTemplate : Entity
    {
        public string ShortTemplateId { get; set; }
        public string TemplateId { get; set; }
    }
}
