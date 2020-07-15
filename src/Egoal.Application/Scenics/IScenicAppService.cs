using Egoal.Application.Services.Dto;
using Egoal.Scenics.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.Scenics
{
    public interface IScenicAppService
    {
        Task<ScenicDto> GetScenicAsync(string language);
        Task EditScenicAsync(ScenicDto input);
        Task<List<BookGroundChangCiOutput>> BookGroundChangCiAsync(BookGroundChangCiInput input);
        Task<BookGroundChangCiOutput> BookGroundChangCiAsync(int groundId, string date, int changCiId, int quantity, string listNo, bool isRemote = false);
        Task CancelGroundChangCiAsync(CancelGroundChangCiInput input);
        ScenicOptions GetScenicOptions();
        Task<List<ComboboxItemDto<int>>> GetParkComboboxItemsAsync();
        Task<List<ComboboxItemDto<int>>> GetGroundComboboxItemsAsync();
        Task<List<ComboboxItemDto<int>>> GetGateGroupComboboxItemsAsync(int? groundId);
        Task<List<ComboboxItemDto<int>>> GetSalePointComboboxItemsAsync(int? parkId);
    }
}
