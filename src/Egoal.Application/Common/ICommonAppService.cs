using Egoal.Application.Services.Dto;
using Egoal.Common.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.Common
{
    public interface ICommonAppService
    {
        List<ComboboxItemDto> GetEducationComboboxItems();
        List<ComboboxItemDto> GetNationComboboxItems();
        Task<List<ComboboxItemDto<int>>> GetAgeRangeComboboxItemsAsync();
        Task<TreeNodeDto> GetTouristOriginTreeAsync();
        Task<List<ComboboxItemDto<int>>> GetCertTypeComboboxItemsAsync();
        Task<List<DateChangCiSaleDto>> GetChangCiForSaleAsync(string date);
        Task SendVerificationCodeAsync(string address);
        void ValidateVerificationCode(string address, string code);

        /// <summary>
        /// 购买成功通知
        /// </summary>
        /// <param name="address"></param>
        /// <param name="monthDay"></param>
        /// <param name="listNo"></param>
        /// <param name="entryTime"></param>
        /// <param name="needActive"></param>
        /// <returns></returns>
        Task SendPersonalBookSuccessAsync(string address, string monthDay, string listNo, string entryTime, bool needActive, string qrCodeString);

        /// <summary>
        /// 预约成功通知
        /// </summary>
        /// <param name="address"></param>
        /// <param name="monthDay"></param>
        /// <param name="listNo"></param>
        /// <param name="entryTime"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        Task SendGroupBookSuccessAsync(string address, string monthDay, string listNo, string entryTime, int number);
    }
}
