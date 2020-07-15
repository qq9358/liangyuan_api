using Egoal.Common.Dto;
using Egoal.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.Common
{
    public interface IChangCiRepository : IRepository<ChangCi>
    {
        Task<List<DateChangCiSaleDto>> GetChangCiForSaleAsync(string date);
    }
}
