using Egoal.Application.Services.Dto;
using Egoal.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.Scenics
{
    public interface IPcRepository : IRepository<Pc>
    {
        Task<List<ComboboxItemDto<int>>> GetCashPcComboboxItemsAsync();
    }
}
