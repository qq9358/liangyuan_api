using Egoal.Application.Services.Dto;
using Egoal.Scenics.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.Scenics
{
    public interface IPcAppService
    {
        Task<RegistPcOutput> RegistHandsetAsync(RegistPcInput input);
        Task<ChangePcLocationOutput> ChangeLocationAsync(ChangePcLocationInput input);
        Task<List<ComboboxItemDto<int>>> GetCashPcComboboxItemsAsync();
    }
}
