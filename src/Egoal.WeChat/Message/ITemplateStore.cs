using System.Threading.Tasks;

namespace Egoal.WeChat.Message
{
    public interface ITemplateStore
    {
        Task<string> GetTemplateIdAsync(string shortTemplateId);
        Task SaveTemplateIdAsync(string shortTemplateId, string templateId);
    }
}
