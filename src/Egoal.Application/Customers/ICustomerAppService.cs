using Egoal.Application.Services.Dto;
using Egoal.Customers.Dto;
using Egoal.Members;
using Egoal.Members.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.Customers
{
    public interface ICustomerAppService
    {
        Task RegistFromMobileAsync(MobileRegistInput input);
        Task CreateAsync(EditDto input);
        Task<EditDto> GetForEditAsync(Guid id);
        Task EditAsync(EditDto input);
        Task AuditAsync(AuditInput input);
        Task ChangePasswordAsync(ChangePasswordInput input);

        Task UnBindMemberAsync(UnBindMemberInput input);
        Task DeleteAsync(Guid id);
        Task<LoginOutput> LoginFromWeChatAsync(CustomerLoginInput input);
        Task<PagedResultDto<CustomerListDto>> GetCustomersAsync(GetCustomersInput input);
        Task<List<ComboboxItemDto<int>>> GetCustomerTypeComboboxItemsAsync();
        Task<List<ComboboxItemDto<Guid>>> GetCustomerComboboxItemsAsync();
        Task RegistGuiderFromMobileAsync(RegistGuiderInput input);
        Task<EditGuiderDto> GetGuiderForEditAsync(Guid id);
        Task EditGuiderAsync(EditGuiderDto input);
        Task ChangeGuiderPasswordAsync(ChangePasswordInput input);

        /// <summary>
        /// 忘了密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ForgetGuidePasswordAsync(ForgetPasswordInput input);

        Task<LoginOutput> GuiderLoginAsync(GuiderLoginInput input);
        Task<LoginOutput> GuiderLogoutAsync();

        /// <summary>
        /// 判断导游用户名是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<Member> GetGuideAsync(string userName);

        /// <summary>
        /// 注册时发送验证码
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <returns></returns>
        Task RegisterSendVerificationCodeAsync(string phoneNum);
    }
}
