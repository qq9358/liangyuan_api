using Egoal.Application.Services;
using Egoal.Application.Services.Dto;
using Egoal.AutoMapper;
using Egoal.BackgroundJobs;
using Egoal.Caches;
using Egoal.Common;
using Egoal.Cryptography;
using Egoal.Customers.Dto;
using Egoal.Domain.Repositories;
using Egoal.Extensions;
using Egoal.Localization;
using Egoal.Members;
using Egoal.Members.Dto;
using Egoal.Orders;
using Egoal.Runtime.Session;
using Egoal.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Egoal.Customers
{
    public class CustomerAppService : ApplicationService, ICustomerAppService
    {
        private readonly ISession _session;
        private readonly IRepository<Member, Guid> _memberRepository;
        private readonly IMemberAppService _memberAppService;
        private readonly IMemberDomainService _memberDomainService;
        private readonly IRepository<CustomerType> _customerTypeRepository;
        private readonly IRepository<GuiderPhoto, Guid> _guiderPhotoRepository;
        private readonly IRepository<MemberGuider> _memberGuiderRepository;
        private readonly IBackgroundJobService _backgroundJobAppService;
        private readonly ICustomerDomainService _customerDomainService;
        private readonly INameCacheService _nameCacheService;
        private readonly ICommonAppService _commonAppService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CustomerAppService(
            ISession session,
            IRepository<Member, Guid> customerRepository,
            IMemberAppService memberAppService,
            IMemberDomainService memberDomainService,
            IRepository<CustomerType> customerTypeRepository,
            IRepository<GuiderPhoto, Guid> guiderPhotoRepository,
            IRepository<MemberGuider> memberGuiderRepository,
            IBackgroundJobService backgroundJobAppService,
            ICustomerDomainService customerDomainService,
            INameCacheService nameCacheService,
            ICommonAppService commonAppService,
            IStringLocalizer<SharedResource> localizer)
        {
            _session = session;
            _memberRepository = customerRepository;
            _memberAppService = memberAppService;
            _memberDomainService = memberDomainService;
            _customerTypeRepository = customerTypeRepository;
            _guiderPhotoRepository = guiderPhotoRepository;
            _memberGuiderRepository = memberGuiderRepository;
            _backgroundJobAppService = backgroundJobAppService;
            _customerDomainService = customerDomainService;
            _nameCacheService = nameCacheService;
            _commonAppService = commonAppService;
            _localizer = localizer;
        }

        public async Task RegistFromMobileAsync(MobileRegistInput input)
        {
            var customer = new Member();
            customer.RegTypeId = RegType.客户;
            customer.MemberStatusId = MemberStatus.审核中;
            customer.MemberStatusName = MemberStatus.审核中.ToString();
            customer.SourceType = ((int)SourceType.微信).ToString();
            customer.Name = input.Name;
            customer.MemberTypeId = input.CustomerTypeId;
            customer.BusinessLicense = input.Code;
            customer.Linkman = input.ContactName;
            customer.Mobile = input.ContactMobile;
            customer.CertTypeId = DefaultCertType.二代身份证;
            customer.CertTypeName = "二代身份证";
            customer.CertNo = input.ContactCertNo;
            customer.Uid = input.UserName;
            customer.Salt = DateTime.Now.Ticks.ToString();
            customer.Pwd = SHAHelper.SHA512Encrypt(input.Password, customer.Salt);
            customer.Code = DateTime.Now.Ticks.ToString();

            customer.AddPhoto(input.Photo);

            var member = await _memberRepository.GetAsync(_session.MemberId.Value);
            customer.WeChatNo = member.Uid;

            await _memberDomainService.CreateAsync(customer);
        }

        public async Task CreateAsync(EditDto input)
        {
            var customer = new Member();
            customer.RegTypeId = RegType.客户;
            customer.MemberStatusId = input.StatusId;
            customer.MemberStatusName = input.StatusId?.ToString();
            customer.SourceType = ((int)SourceType.售票).ToString();
            customer.Name = input.Name;
            customer.Zjf = input.Zjf;
            customer.BusinessLicense = input.BusinessLicense;
            customer.MemberTypeId = input.CustomerTypeId;
            customer.CertTypeId = input.CertTypeId;
            customer.CertNo = input.CertNo;
            customer.Linkman = input.Linkman;
            customer.LegalPerson = input.LegalPerson;
            customer.Mobile = input.Mobile;
            customer.Email = input.Email;
            customer.Tel = input.Tel;
            customer.Address = input.Address;
            customer.Memo = input.Memo;
            customer.Uid = input.Uid;
            customer.Salt = DateTime.Now.Ticks.ToString();
            customer.Pwd = SHAHelper.SHA512Encrypt(input.Pwd, customer.Salt);
            customer.Code = DateTime.Now.Ticks.ToString();

            customer.AddPhoto(input.Photo);

            await _memberDomainService.CreateAsync(customer);
        }

        public async Task<EditDto> GetForEditAsync(Guid id)
        {
            var customer = await _memberRepository.FirstOrDefaultAsync(_memberRepository.GetAllIncluding(m => m.MemberPhotos).Where(m => m.Id == id));
            var editDto = new EditDto();
            editDto.Id = customer.Id;
            editDto.Uid = customer.Uid;
            editDto.Name = customer.Name;
            editDto.Zjf = customer.Zjf;
            editDto.Mobile = customer.Mobile;
            editDto.CustomerTypeId = customer.MemberTypeId;
            editDto.CertTypeId = customer.CertTypeId;
            editDto.CertNo = customer.CertNo;
            editDto.StatusId = customer.MemberStatusId;
            editDto.BusinessLicense = customer.BusinessLicense;
            editDto.LegalPerson = customer.LegalPerson;
            editDto.Email = customer.Email;
            editDto.Tel = customer.Tel;
            editDto.Linkman = customer.Linkman;
            editDto.Address = customer.Address;
            editDto.Memo = customer.Memo;
            if (!customer.MemberPhotos.IsNullOrEmpty())
            {
                editDto.Photo = $"data:image/jpeg;base64,{Convert.ToBase64String(customer.MemberPhotos.First().Photo)}";
            }

            return editDto;
        }

        public async Task EditAsync(EditDto input)
        {
            var query = input.PhotoChanged ? _memberRepository.GetAllIncluding(m => m.MemberPhotos) : _memberRepository.GetAll();
            query = query.Where(m => m.Id == input.Id);
            var customer = await _memberRepository.FirstOrDefaultAsync(query);
            customer.MemberStatusId = input.StatusId;
            customer.MemberStatusName = input.StatusId?.ToString();
            customer.Name = input.Name;
            customer.Zjf = input.Zjf;
            customer.BusinessLicense = input.BusinessLicense;
            customer.MemberTypeId = input.CustomerTypeId;
            customer.CertTypeId = input.CertTypeId;
            customer.CertNo = input.CertNo;
            customer.Linkman = input.Linkman;
            customer.LegalPerson = input.LegalPerson;
            customer.Mobile = input.Mobile;
            customer.Email = input.Email;
            customer.Tel = input.Tel;
            customer.Address = input.Address;
            customer.Memo = input.Memo;
            if (!input.Pwd.IsNullOrEmpty())
            {
                customer.Pwd = SHAHelper.SHA512Encrypt(input.Pwd, customer.Salt);
            }
            if (input.PhotoChanged)
            {
                customer.MemberPhotos.Clear();
                customer.AddPhoto(input.Photo);
            }
        }

        public async Task AuditAsync(AuditInput input)
        {
            var customer = await _memberRepository.GetAsync(input.Id);
            customer.Audit(input.Agree, input.Memo);

            if (!customer.WeChatNo.IsNullOrEmpty())
            {
                var messageTitle = $"你的团队注册请求{(input.Agree ? "已审核通过" : "被拒绝")}";
                var messageRemark = input.Agree ? "感谢你的使用" : $"原因：{input.Memo}";
                var jobArgs = new SendAuditMessageInput();
                jobArgs.OpenId = customer.WeChatNo;
                jobArgs.Title = messageTitle;
                jobArgs.UserName = customer.Uid;
                jobArgs.Mobile = customer.Mobile;
                jobArgs.Date = DateTime.Now.ToDateString();
                jobArgs.Remark = messageRemark;

                await _backgroundJobAppService.EnqueueAsync<SendAuditMessageJob>(jobArgs.ToJson());
            }
        }

        public async Task ChangePasswordAsync(ChangePasswordInput input)
        {
            var customer = await _memberRepository.GetAsync(_session.CustomerId.Value);
            customer.ChangePassword(input.OriginalPassword, input.NewPassword);
        }

        public async Task DeleteAsync(Guid id)
        {
            var customer = await _memberRepository.FirstOrDefaultAsync(_memberRepository.GetAllIncluding(m => m.MemberPhotos).Where(m => m.Id == id));

            await _memberRepository.DeleteAsync(customer);
        }

        public async Task UnBindMemberAsync(UnBindMemberInput input)
        {
            await _customerDomainService.UnBindMemberAsync(input.CustomerId, input.MemberId);
        }

        public async Task<LoginOutput> LoginFromWeChatAsync(CustomerLoginInput input)
        {
            var customer = await _memberDomainService.LoginAsync(input.UserName, input.Password);

            var member = await _memberRepository.GetAsync(_session.MemberId.Value);

            if (input.ShouldBindMember)
            {
                await _customerDomainService.BindMemberAsync(customer.Id, member.Id);
            }

            var output = await _memberAppService.BuildLoginOutputAsync(member, "");

            return output;
        }

        public async Task<PagedResultDto<CustomerListDto>> GetCustomersAsync(GetCustomersInput input)
        {
            var query = _memberRepository.GetAll()
                .Where(m => m.RegTypeId == RegType.客户)
                .WhereIf(!input.Uid.IsNullOrEmpty(), m => m.Uid == input.Uid)
                .WhereIf(!input.Name.IsNullOrEmpty(), m => m.Name.Contains(input.Name))
                .WhereIf(!input.Mobile.IsNullOrEmpty(), m => m.Mobile == input.Mobile)
                .WhereIf(!input.CertNo.IsNullOrEmpty(), m => m.CertNo == input.CertNo)
                .WhereIf(input.CustomerTypeId.HasValue, m => m.MemberTypeId == input.CustomerTypeId)
                .WhereIf(input.StatusId.HasValue, m => m.MemberStatusId == input.StatusId);

            var count = await _memberRepository.CountAsync(query);

            query = query.OrderByDescending(m => m.CTime).PageBy(input);
            var resultQuery = query.Select(m => new CustomerListDto
            {
                Id = m.Id,
                Uid = m.Uid,
                Name = m.Name,
                Mobile = m.Mobile,
                CertTypeId = m.CertTypeId,
                CertNo = m.CertNo,
                CustomerTypeId = m.MemberTypeId,
                StatusId = m.MemberStatusId,
                BusinessLicense = m.BusinessLicense,
                LegalPerson = m.LegalPerson,
                Email = m.Email,
                Tel = m.Tel,
                Linkman = m.Linkman,
                CTime = m.CTime,
                Photo = m.MemberPhotos.FirstOrDefault().Photo
            });

            var customers = await _memberRepository.ToListAsync(resultQuery);
            foreach (var customer in customers)
            {
                if (customer.StatusId == MemberStatus.审核中)
                {
                    await AuditAsync(new AuditInput
                    {
                        Id = customer.Id,
                        Agree = true
                    });
                }
                customer.StatusName = customer.StatusId?.ToString();
                customer.ShouldAudit = customer.StatusId == MemberStatus.审核中;
                customer.CertTypeName = customer.CertTypeId.HasValue ? _nameCacheService.GetCertTypeName(customer.CertTypeId.Value) : string.Empty;
                customer.CustomerTypeName = customer.CustomerTypeId.HasValue ? _nameCacheService.GetCustomerTypeName(customer.CustomerTypeId.Value) : string.Empty;
                customer.MemberId = await _customerDomainService.GetBindingMemberIdAsync(customer.Id);
                customer.MemberName = customer.MemberId.HasValue ? _nameCacheService.GetMemberName(customer.MemberId.Value) : string.Empty;
            }

            return new PagedResultDto<CustomerListDto>(count, customers);
        }

        public async Task<List<ComboboxItemDto<int>>> GetCustomerTypeComboboxItemsAsync()
        {
            var query = _customerTypeRepository.GetAll()
                .OrderBy(c => c.SortCode)
                .Select(c => new ComboboxItemDto<int>
                {
                    DisplayText = c.Name,
                    Value = c.Id
                });

            return await _customerTypeRepository.ToListAsync(query);
        }

        public async Task<List<ComboboxItemDto<Guid>>> GetCustomerComboboxItemsAsync()
        {
            var query = _memberRepository.GetAll()
                .Where(m => m.RegTypeId == RegType.客户)
                .Select(m => new ComboboxItemDto<Guid> { DisplayText = m.Name, Value = m.Id });

            var items = await _memberRepository.ToListAsync(query);

            return items;
        }

        public async Task RegistGuiderFromMobileAsync(RegistGuiderInput input)
        {
            _commonAppService.ValidateVerificationCode(input.Mobile, input.VerificationCode);

            var exists = await _memberRepository.GetAll().AnyAsync(m => m.Mobile == input.Mobile && m.RegTypeId == RegType.导游);
            if (exists)
            {
                throw new UserFriendlyException(_localizer["PhoneNumRegistered"]);
            }
            if (!string.IsNullOrEmpty(input.Email))
            {
                exists = await _memberRepository.GetAll().AnyAsync(m => m.Email == input.Email && m.RegTypeId == RegType.导游);
                if (exists)
                {
                    throw new UserFriendlyException(_localizer["EmailRegistered"]);
                }
            }

            var guider = input.MapTo<Member>();
            guider.RegTypeId = RegType.导游;
            guider.CertTypeName = DefaultCertType.GetName(guider.CertTypeId.Value);
            guider.MemberStatusId = MemberStatus.正常;
            guider.MemberStatusName = guider.MemberStatusId?.ToString();
            guider.Uid = input.Mobile;
            guider.Salt = DateTime.Now.Ticks.ToString();
            guider.Pwd = SHAHelper.SHA512Encrypt(input.Pwd, guider.Salt);
            guider.Code = DateTime.Now.Ticks.ToString();

            if (_session.MemberId.HasValue)
            {
                MemberGuider memberGuider = new MemberGuider();
                memberGuider.MemberId = _session.MemberId.Value;
                memberGuider.GuiderId = guider.Id;
                await _memberGuiderRepository.InsertAsync(memberGuider);
            }

            if (!input.Photo.IsNullOrEmpty())
            {
                var photo = input.Photo.Split(',')[1];

                var guiderPhoto = new GuiderPhoto();
                guiderPhoto.GuiderId = guider.Id;
                guiderPhoto.Photo = Convert.FromBase64String(photo);

                await _guiderPhotoRepository.InsertAsync(guiderPhoto);
            }

            await _memberRepository.InsertAsync(guider);
        }

        public async Task<EditGuiderDto> GetGuiderForEditAsync(Guid id)
        {
            var guider = await _memberRepository.GetAll()
                .AsNoTracking()
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();

            var guiderDto = guider.MapTo<EditGuiderDto>();

            var photo = await _guiderPhotoRepository.GetAll()
                .Where(p => p.GuiderId == id)
                .Select(p => p.Photo)
                .FirstOrDefaultAsync();
            if (!photo.IsNullOrEmpty())
            {
                guiderDto.Photo = $"data:image/jpeg;base64,{Convert.ToBase64String(photo)}";
            }

            return guiderDto;
        }

        public async Task EditGuiderAsync(EditGuiderDto input)
        {
            var guider = await _memberRepository.FirstOrDefaultAsync(input.Id);
            if (guider == null)
            {
                throw new UserFriendlyException("导游不存在");
            }

            if (input.Mobile != guider.Mobile)
            {
                _commonAppService.ValidateVerificationCode(input.Mobile, input.VerificationCode);
            }

            input.MapTo(guider);
        }

        public async Task ChangeGuiderPasswordAsync(ChangePasswordInput input)
        {
            if (!_session.GuiderId.HasValue)
            {
                throw new UserFriendlyException("导游未登录");
            }

            var guider = await _memberRepository.FirstOrDefaultAsync(_session.GuiderId.Value);

            var pwd = SHAHelper.SHA512Encrypt(input.OriginalPassword, guider.Salt);
            if (guider.Pwd != pwd)
            {
                throw new UserFriendlyException("原密码不正确");
            }

            guider.Pwd = SHAHelper.SHA512Encrypt(input.NewPassword, guider.Salt);
        }

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task ForgetGuidePasswordAsync(ForgetPasswordInput input)
        {
            _commonAppService.ValidateVerificationCode(input.UserName, input.VerificationCode);
            Member guide = await GetGuideAsync(input.UserName);
            Member guideRepository = await _memberRepository.FirstOrDefaultAsync(guide.Id);
            guideRepository.Pwd = SHAHelper.SHA512Encrypt(input.NewPassword, guideRepository.Salt);
        }

        public async Task<LoginOutput> GuiderLoginAsync(GuiderLoginInput input)
        {
            if (!input.VerificationCode.IsNullOrEmpty())
            {
                _commonAppService.ValidateVerificationCode(input.UserName, input.VerificationCode);
            }

            Member guider = await GetGuideAsync(input.UserName);

            if (!input.Password.IsNullOrEmpty())
            {
                var pwd = SHAHelper.SHA512Encrypt(input.Password, guider.Salt);
                if (guider.Pwd != pwd)
                {
                    throw new UserFriendlyException("密码不正确");
                }
            }

            var memberIds = await _memberGuiderRepository.GetAll()
                .Where(m => m.GuiderId == guider.Id)
                .Select(m => m.MemberId)
                .ToListAsync();
            foreach(Guid memberId in memberIds)
            {
                var member = await _memberRepository.GetAll()
                    .AsNoTracking()
                    .Where(m => m.Id == memberId)
                    .FirstOrDefaultAsync();
                if(member != null)
                {
                    return await _memberAppService.BuildLoginOutputAsync(member, "");
                }
            }
            return await _memberAppService.BuildLoginOutputAsync(guider, "");
        }

        /// <summary>
        /// 判断导游名称是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<Member> GetGuideAsync(string userName)
        {
            var guide = await _memberRepository.GetAll()
                .AsNoTracking()
                .Where(m => m.RegTypeId == RegType.导游)
                .Where(m => m.Mobile == userName || m.Email == userName)
                .FirstOrDefaultAsync();
            if (guide == null)
            {
                throw new UserFriendlyException("用户名不正确");
            }

            if (guide.MemberStatusId != MemberStatus.正常)
            {
                throw new UserFriendlyException($"用户{guide.MemberStatusName}");
            }
            return guide;
        }

        public async Task<LoginOutput> GuiderLogoutAsync()
        {
            if (!_session.GuiderId.HasValue)
            {
                throw new UserFriendlyException("导游未登录");
            }

            var memberIds = await _memberGuiderRepository.GetAll()
                .Where(m => m.GuiderId == _session.GuiderId.Value)
                .Select(m => m.MemberId)
                .ToListAsync();
            foreach(var memberId in memberIds)
            {
                var member = await _memberRepository.GetAll()
                    .AsNoTracking()
                    .Where(m => m.Id == memberId)
                    .FirstOrDefaultAsync();
                if (member != null)
                {
                    return await _memberAppService.BuildLoginOutputAsync(member, "", null, false);
                }
            }

            return new LoginOutput();
        }

        /// <summary>
        /// 注册时发送验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public async Task RegisterSendVerificationCodeAsync(string phoneNum)
        {
            if(await _memberRepository.GetAll().AnyAsync(m => m.Mobile == phoneNum && m.RegTypeId == RegType.导游))
            {
                throw new UserFriendlyException(_localizer["PhoneNumRegistered"]);
            }
            else
            {
                await _commonAppService.SendVerificationCodeAsync(phoneNum);
            }
        }
    }
}
