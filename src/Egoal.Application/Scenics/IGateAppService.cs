using Egoal.Application.Services.Dto;
using Egoal.Scenics.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.Scenics
{
    public interface IGateAppService
    {
        Task<RegistGateOutput> GetOrRegistGateAsync(RegistGateInput input, GateType gateType);
        Task ChangeLocationAsync(ChangeGateLocationInput input);
        Task<List<ComboboxItemDto<int>>> GetGateComboboxItemsAsync(int? gateGroupId);
    }
}
