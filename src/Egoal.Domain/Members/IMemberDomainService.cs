using System;
using System.Threading.Tasks;

namespace Egoal.Members
{
    public interface IMemberDomainService
    {
        Task<bool> HasElectronicMemberCardAsync(Guid memberId);
        Task CreateAsync(Member member);
        Task<Member> LoginAsync(string uid, string password);
    }
}
