using Egoal.Dependency;
using System.Threading.Tasks;

namespace Egoal.DynamicCodes
{
    public interface IDynamicCodeService : IScopedDependency
    {
        Task<string> GenerateListNoAsync(string listNoType);
        Task<string> GenerateWxTicketCodeAsync();
        Task<string> GenerateParkTicketCodeAsync(int parkId);
        Task<string> GenerateTicketCodeAsync(string prefix);
    }
}
