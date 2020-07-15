using Egoal.Cryptography;
using Egoal.Domain.Repositories;
using Egoal.Domain.Services;
using Egoal.Extensions;
using Egoal.TicketTypes;
using Egoal.UI;
using System;
using System.Threading.Tasks;

namespace Egoal.Members
{
    public class MemberDomainService : DomainService, IMemberDomainService
    {
        private readonly IRepository<Member, Guid> _memberRepository;
        private readonly IRepository<MemberCard> _memberCardRepository;

        public MemberDomainService(
            IRepository<Member, Guid> memberRepository,
            IRepository<MemberCard> memberCardRepository)
        {
            _memberRepository = memberRepository;
            _memberCardRepository = memberCardRepository;
        }

        public async Task<bool> HasElectronicMemberCardAsync(Guid memberId)
        {
            return await _memberCardRepository.AnyAsync(m => m.MemberId == memberId && m.TicketTypeId == DefaultTicketType.电子会员卡);
        }

        public async Task CreateAsync(Member member)
        {
            var uidExists = await _memberRepository.AnyAsync(o => o.Uid == member.Uid);
            if (uidExists)
            {
                throw new UserFriendlyException("用户名已存在");
            }

            if (!member.CertNo.IsNullOrEmpty())
            {
                var certNoExists = await _memberRepository.AnyAsync(m => m.CertNo == member.CertNo);
                if (certNoExists)
                {
                    throw new UserFriendlyException("证件号码已存在");
                }
            }

            await _memberRepository.InsertAsync(member);
        }

        public async Task<Member> LoginAsync(string uid, string password)
        {
            var member = await ValidatePasswordAsync(uid, password);

            if (member.MemberStatusId != MemberStatus.正常)
            {
                throw new UserFriendlyException($"用户{member.MemberStatusName}");
            }

            return member;
        }

        private async Task<Member> ValidatePasswordAsync(string uid, string password)
        {
            var member = await _memberRepository.FirstOrDefaultAsync(m => m.Uid == uid);
            if (member == null)
            {
                throw new UserFriendlyException("用户名无效");
            }

            var pwd = SHAHelper.SHA512Encrypt(password, member.Salt);
            if (member.Pwd != pwd)
            {
                throw new UserFriendlyException("密码不正确");
            }

            return member;
        }
    }
}
