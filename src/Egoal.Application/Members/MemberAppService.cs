using Egoal.Annotations;
using Egoal.Application.Services;
using Egoal.Application.Services.Dto;
using Egoal.Authorization;
using Egoal.AutoMapper;
using Egoal.Caches;
using Egoal.Common;
using Egoal.Cryptography;
using Egoal.Customers;
using Egoal.Domain.Repositories;
using Egoal.Extensions;
using Egoal.Localization;
using Egoal.Members.Dto;
using Egoal.Runtime.Security;
using Egoal.Runtime.Session;
using Egoal.Scenics;
using Egoal.Staffs;
using Egoal.Tickets;
using Egoal.TicketTypes;
using Egoal.UI;
using Egoal.WeChat.OAuth;
using Egoal.WeChat.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Egoal.Members
{
    public class MemberAppService : ApplicationService, IMemberAppService
    {
        private readonly IHostingEnvironment _environment;
        private readonly ISession _session;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRepository<Member, Guid> _memberRepository;
        private readonly IRepository<MemberCard> _memberCardRepository;
        private readonly IRepository<MemberGuider> _memberGuiderRepository;
        private readonly IRepository<UserWechat> _userWechatRepository;
        private readonly ISignInAppService _signInAppService;
        private readonly ITicketSaleDomainService _ticketSaleDomainService;
        private readonly IStaffDomainService _staffDomainService;
        private readonly ICommonAppService _commonAppService;
        private readonly IRightDomainService _rightDomainService;
        private readonly ICustomerDomainService _customerDomainService;
        private readonly INameCacheService _nameCacheService;

        public MemberAppService(
            IHostingEnvironment environment,
            ISession session,
            IStringLocalizer<SharedResource> localizer,
            IServiceProvider serviceProvider,
            IRepository<Member, Guid> memberRepository,
            IRepository<MemberCard> memberCardRepository,
            IRepository<MemberGuider> memberGuiderRepository,
            IRepository<UserWechat> userWechatRepository,
            ISignInAppService signInAppService,
            ITicketSaleDomainService ticketSaleDomainService,
            IStaffDomainService staffDomainService,
            ICommonAppService commonAppService,
            IRightDomainService rightDomainService,
            ICustomerDomainService customerDomainService,
            INameCacheService nameCacheService)
        {
            _environment = environment;
            _session = session;
            _localizer = localizer;
            _serviceProvider = serviceProvider;
            _memberRepository = memberRepository;
            _memberCardRepository = memberCardRepository;
            _memberGuiderRepository = memberGuiderRepository;
            _userWechatRepository = userWechatRepository;
            _signInAppService = signInAppService;
            _ticketSaleDomainService = ticketSaleDomainService;
            _staffDomainService = staffDomainService;
            _commonAppService = commonAppService;
            _rightDomainService = rightDomainService;
            _rightDomainService = rightDomainService;
            _customerDomainService = customerDomainService;
            _nameCacheService = nameCacheService;
        }

        public async Task<LoginOutput> LoginFromWechatOffiaccountAsync(WechatOffiaccountLoginInput input)
        {
            Member member = null;
            UserWechat user = null;

            if (_environment.IsProduction())
            {
                if (input.Code.IsNullOrEmpty())
                {
                    throw new ArgumentNullException(_localizer["CodeNotNull"]);
                }

                var oAuthApi = _serviceProvider.GetRequiredService<OAuthApi>();
                var result = await oAuthApi.GetUserAccessTokenAsync(input.Code);

                var userApi = _serviceProvider.GetRequiredService<UserApi>();
                var wxUser = await userApi.GetUserInfoAsync(result.openid);

                user = await _userWechatRepository.GetAll()
                    .WhereIf(wxUser.unionid.IsNullOrEmpty(), u => u.OffiaccountOpenId == result.openid)
                    .WhereIf(!wxUser.unionid.IsNullOrEmpty(), u => u.OffiaccountOpenId == result.openid || u.UnionId == wxUser.unionid)
                    .FirstOrDefaultAsync();
                if (user == null)
                {
                    user = new UserWechat();
                    user.Nickname = wxUser.nickname;
                    user.HeadImageUrl = wxUser.headimgurl;

                    member = await _memberRepository.FirstOrDefaultAsync(o => o.Uid == result.openid);
                    if (member == null)
                    {
                        member = await CreateWeChatMemberAsync(wxUser, user);
                    }
                    else
                    {
                        member.UserWechat = user;
                    }
                }
                else
                {
                    member = await _memberRepository.FirstOrDefaultAsync(user.UserId);
                }

                user.OffiaccountOpenId = result.openid;
                user.UnionId = wxUser.unionid;
                member.IsWeChatSubscribed = wxUser.subscribe == "1";
            }
            else
            {
                member = await _memberRepository.FirstOrDefaultAsync(Guid.Parse("44863232-372F-4D4A-A499-1600D6BF90CA"));
                user = await _userWechatRepository.FirstOrDefaultAsync(u => u.UserId == member.Id);
            }

            var output = await BuildLoginOutputAsync(member, user.OffiaccountOpenId);

            return output;
        }


        public async Task<LoginOutput> LoginFromWechatMiniProgramAsync(WechatMiniProgramLoginInput input)
        {
            if (input.Code.IsNullOrEmpty())
            {
                throw new ArgumentNullException("Code不能为空");
            }

            var oAuthApi = _serviceProvider.GetRequiredService<OAuthApi>();
            var result = await oAuthApi.Code2SessionAsync(input.Code);

            Member member = null;

            var user = await _userWechatRepository.GetAll()
                .WhereIf(result.unionid.IsNullOrEmpty(), u => u.MiniProgramOpenId == result.openid)
                .WhereIf(!result.unionid.IsNullOrEmpty(), u => u.MiniProgramOpenId == result.openid || u.UnionId == result.unionid)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                user = new UserWechat();
                user.HeadImageUrl = input.UserInfo.AvatarUrl;
                user.Nickname = input.UserInfo.NickName;

                member = await CreateWeChatMemberAsync(null, user);
            }
            else
            {
                member = await _memberRepository.FirstOrDefaultAsync(user.UserId);
            }

            user.MiniProgramOpenId = result.openid;
            user.UnionId = result.unionid;

            var output = await BuildLoginOutputAsync(member, result.openid);

            return output;
        }

        private async Task<Member> CreateWeChatMemberAsync(WeChat.User.UserInfoResult wxUser, UserWechat user)
        {
            var member = new Member();
            if (wxUser != null)
            {
                member.Name = member.PetName = wxUser.nickname;
                member.HeadImgUrl = wxUser.headimgurl;
                if (wxUser.sex == "1")
                {
                    member.Sex = "男";
                }
                else if (wxUser.sex == "2")
                {
                    member.Sex = "女";
                }
                member.Address = wxUser.Address;
            }
            else
            {
                member.Name = user.Nickname;
                member.HeadImgUrl = user.HeadImageUrl;
            }
            member.RegTypeId = RegType.会员;
            member.Uid = DateTime.Now.Ticks.ToString();
            member.Salt = DateTime.Now.AddMinutes(30).Ticks.ToString();
            member.Pwd = SHAHelper.SHA512Encrypt("000000", member.Salt);
            member.Code = DateTime.Now.AddHours(1).Ticks.ToString();
            member.MemberTypeId = (int)MemberType.微信会员;
            member.MemberTypeName = MemberType.微信会员.ToString();
            member.MemberStatusId = MemberStatus.正常;
            member.MemberStatusName = MemberStatus.正常.ToString();
            member.SourceType = ((int)SourceType.微信).ToString();

            member.UserWechat = user;

            return await _memberRepository.InsertAsync(member);
        }

        public async Task<LoginOutput> LoginOrRegistFromH5Async(H5LoginInput input)
        {
            _commonAppService.ValidateVerificationCode(input.UserName, input.VerificationCode);

            var member = await _memberRepository.GetAll()
                .AsNoTracking()
                .Where(m => m.RegTypeId == RegType.会员)
                .Where(m => m.Mobile == input.UserName || m.Email == input.UserName)
                .FirstOrDefaultAsync();

            if (member == null)
            {
                member = new Member();
                member.Uid = DateTime.Now.Ticks.ToString();
                member.RegTypeId = RegType.会员;
                member.Salt = DateTime.Now.Ticks.ToString();
                member.Pwd = SHAHelper.SHA512Encrypt("000000", member.Salt);
                member.Code = DateTime.Now.Ticks.ToString();
                member.MemberStatusId = MemberStatus.正常;
                member.MemberStatusName = MemberStatus.正常.ToString();
                member.SourceType = ((int)SourceType.手机网页).ToString();

                MobileNumberAttribute mobile = new MobileNumberAttribute();
                if (mobile.IsValid(input.UserName))
                {
                    member.Mobile = input.UserName;
                }
                else
                {
                    member.Email = input.UserName;
                }

                await _memberRepository.InsertAsync(member);
            }

            if (member.MemberStatusId != MemberStatus.正常)
            {
                throw new UserFriendlyException($"用户{member.MemberStatusName}");
            }

            return await BuildLoginOutputAsync(member, "");
        }

        public async Task<LoginOutput> BindStaffAsync(BindStaffInput input)
        {
            var staff = await _staffDomainService.LoginAsync(input.UserName, input.Password);

            var member = await _memberRepository.GetAsync(_session.MemberId.Value);
            member.BindStaff(staff.Id);

            var user = await _userWechatRepository.FirstOrDefaultAsync(u => u.UserId == member.Id);

            var output = await BuildLoginOutputAsync(member, user.OffiaccountOpenId);

            return output;
        }

        public async Task<LoginOutput> BuildLoginOutputAsync(Member member, string openId, Guid? customerId = null, bool withGuider = true)
        {
            var loginOutput = new LoginOutput();
            loginOutput.Member = member.MapTo<MemberDto>();
            loginOutput.Member.OpenId = openId;
            loginOutput.Member.LocalTicketEnrollFace = await _rightDomainService.HasFeatureAsync(Guid.Parse(Permissions.TMSWeChat_EnrollFace));

            var bindingCustomerId = await _customerDomainService.GetBindingCustomerIdAsync(member.Id);
            loginOutput.Member.HasBindCustomer = bindingCustomerId.HasValue;
            customerId = customerId ?? bindingCustomerId;
            if (customerId.HasValue)
            {
                loginOutput.Member.CustomerId = customerId;
                loginOutput.Member.CustomerName = _nameCacheService.GetCustomerName(customerId);
            }

            if (member.StaffId.HasValue)
            {
                var permissions = await _staffDomainService.GetPermissionsAsync(member.StaffId.Value, SystemType.微信购票系统);
                loginOutput.Permissions.AddRange(permissions);
            }

            if (member.SourceType != ((int)SourceType.售票).ToString())
            {
                loginOutput.Member.IsRegisted = false;
            }
            else
            {
                loginOutput.Member.IsRegisted = true;
            }

            if (member.RegTypeId == RegType.导游)
            {
                loginOutput.Member.GuiderId = member.Id;
            }
            else if (withGuider)
            {
                var guiderIds = await _memberGuiderRepository.GetAll()
                    .Where(m => m.MemberId == member.Id)
                    .Select(m => m.GuiderId)
                    .ToListAsync();

                foreach (Guid guiderId in guiderIds)
                {
                    var guider = await _memberRepository.GetAll()
                        .Where(m => m.Id == guiderId)
                        .Select(m => new { m.MemberStatusId, m.Name, m.Mobile, m.CompanyName })
                        .FirstOrDefaultAsync();
                    if (guider != null && guider.MemberStatusId == MemberStatus.正常)
                    {
                        loginOutput.Member.GuiderId = guiderId;
                        loginOutput.Member.Name = guider.Name;
                        loginOutput.Member.Mobile = guider.Mobile;
                        loginOutput.Member.CompanyName = guider.CompanyName;
                        break;
                    }
                }
            }

            var claims = await BuildMemberClaimsAsync(member, loginOutput.Member.GuiderId);
            loginOutput.Token = _signInAppService.CreateToken(claims);

            return loginOutput;
        }

        private async Task<List<Claim>> BuildMemberClaimsAsync(Member member, Guid? guiderId)
        {
            var claims = new List<Claim>();

            if (member.RegTypeId == RegType.会员)
            {
                claims.Add(new Claim(TmsClaimTypes.MemberId, member.Id.ToString()));
            }

            if (guiderId.HasValue)
            {
                claims.Add(new Claim(TmsClaimTypes.GuiderId, guiderId.Value.ToString()));
            }

            if (member.StaffId.HasValue)
            {
                claims.Add(new Claim(TmsClaimTypes.StaffId, member.StaffId.Value.ToString()));
                var roleId = await _staffDomainService.GetRoleIdAsync(member.StaffId.Value);
                if (roleId.HasValue)
                {
                    claims.Add(new Claim(TmsClaimTypes.RoleId, roleId.Value.ToString()));
                }
            }
            else
            {
                claims.Add(new Claim(TmsClaimTypes.StaffId, DefaultStaff.微信购票.ToString()));
            }

            claims.Add(new Claim(TmsClaimTypes.PcId, DefaultPc.微信购票.ToString()));
            claims.Add(new Claim(TmsClaimTypes.SalePointId, DefaultSalePoint.微信购票.ToString()));
            claims.Add(new Claim(TmsClaimTypes.ParkId, DefaultPark.微信购票.ToString()));

            return claims;
        }

        public async Task<MemberCardDto> GetElectronicMemberCardAsync(Guid memberId)
        {
            var memberCard = await _memberCardRepository.FirstOrDefaultAsync(m => m.MemberId == memberId && m.TicketTypeId == DefaultTicketType.电子会员卡);

            var memberCardDto = new MemberCardDto();
            memberCardDto.Id = memberCard.Id;
            memberCardDto.TicketTypeName = memberCard.TicketTypeName;
            memberCardDto.Etime = memberCard.Etime;
            memberCardDto.TicketCode = memberCard.TicketCode;
            memberCardDto.IsExpired = memberCard.Etime.To<DateTime>() < DateTime.Now.Date;
            if (memberCardDto.IsExpired)
            {
                var ticketTypeRepository = _serviceProvider.GetRequiredService<ITicketTypeRepository>();
                var ticketType = await ticketTypeRepository.GetAsync(memberCard.TicketTypeId.Value);

                memberCardDto.Days = ticketType.DelayDays ?? 0;
            }

            var member = await _memberRepository.GetAsync(memberId);

            memberCardDto.MemberName = member.Name;
            memberCardDto.Mobile = member.Mobile;
            memberCardDto.IdCard = member.CertNo;
            memberCardDto.Sex = member.Sex;
            memberCardDto.Nation = member.Nation;
            memberCardDto.Education = member.Education;
            memberCardDto.Address = member.Address;

            return memberCardDto;
        }

        public async Task RenewMemberCardAsync(int id)
        {
            var memberCard = await _memberCardRepository.FirstOrDefaultAsync(m => m.Id == id);
            if (memberCard == null)
            {
                throw new UserFriendlyException("会员卡不存在");
            }

            var ticketSale = await _ticketSaleDomainService.RenewAsync(memberCard.TicketId.Value);

            memberCard.Renew(ticketSale.Etime.Substring(0, 10), (int)ticketSale.TicketStatusId, ticketSale.TicketStatusName);
        }

        public async Task<List<ComboboxItemDto<Guid>>> GetMemberComboboxItemsAsync()
        {
            var sourceType = ((int)SourceType.售票).ToString();

            var query = _memberRepository.GetAll()
                .Where(m => m.RegTypeId == RegType.会员 && m.SourceType == sourceType)
                .Select(m => new ComboboxItemDto<Guid> { DisplayText = m.Name, Value = m.Id });

            var items = await _memberRepository.ToListAsync(query);

            return items;
        }

        public async Task<string> GetOpenId(Guid? id)
        {
            if (!id.HasValue) return string.Empty;

            var openId = await _memberRepository.GetAll()
                    .Where(m => m.Id == id.Value)
                    .Select(m => m.Uid)
                    .FirstOrDefaultAsync();

            if (!openId.IsNullOrEmpty() && openId.Length == Member.OpenIdLength)
            {
                return openId;
            }

            return string.Empty;
        }
    }
}
