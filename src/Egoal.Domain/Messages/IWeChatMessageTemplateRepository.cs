using Egoal.Domain.Repositories;
using System.Threading.Tasks;

namespace Egoal.Messages
{
    public interface IWeChatMessageTemplateRepository : IRepository<WeChatMessageTemplate>
    {
        Task<string> GetTemplateIdAsync(string shortTemplateId);
    }
}
