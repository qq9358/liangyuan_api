using System;

namespace Egoal.Runtime.Session
{
    public interface ISession
    {
        int? StaffId { get; }
        int? RoleId { get; }
        int? SearchGroupId { get; }
        int? PcId { get; }
        int? SalePointId { get; }
        int? ParkId { get; }
        Guid? MemberId { get; }
        Guid? CustomerId { get; }
        Guid? GuiderId { get; }
    }
}
