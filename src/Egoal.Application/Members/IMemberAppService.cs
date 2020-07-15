using Egoal.Application.Services.Dto;
using Egoal.Members.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.Members
{
    public interface IMemberAppService
    {
        Task<LoginOutput> LoginFromWechatOffiaccountAsync(WechatOffiaccountLoginInput input);

        /// <summary>
        /// 小程序登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<LoginOutput> LoginFromWechatMiniProgramAsync(WechatMiniProgramLoginInput input);

        Task<LoginOutput> BindStaffAsync(BindStaffInput input);
        Task<LoginOutput> LoginOrRegistFromH5Async(H5LoginInput input);
        Task<LoginOutput> BuildLoginOutputAsync(Member member, string openId, Guid? customerId = null, bool withGuider = true);
        Task<MemberCardDto> GetElectronicMemberCardAsync(Guid memberId);
        Task RenewMemberCardAsync(int id);
        Task<List<ComboboxItemDto<Guid>>> GetMemberComboboxItemsAsync();
        Task<string> GetOpenId(Guid? id);
    }
}
