using Egoal.Domain.Repositories;
using System.Threading.Tasks;

namespace Egoal.Payment
{
    public interface IPayTypeRepository : IRepository<PayType>
    {
        Task InsertSystemPayTypeAsync(PayType payType);
    }
}
