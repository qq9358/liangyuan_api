using Egoal.Dependency;
using Egoal.Payment;
using System;

namespace Egoal.Caches
{
    public interface INameCacheService : IScopedDependency
    {
        string GetMemberName(Guid? id);
        string GetCustomerName(Guid? id);
        string GetStaffName(int? id);
        string GetExplainerTimeslotName(int? id);
        string GetAgeRangeName(int? id);
        string GetKeYuanTypeName(int? id);
        string GetAreaName(int? id);
        string GetGateName(int? id);
        string GetGateGroupName(int? id);
        string GetGroundName(int? id);
        string GetParkName(int? id);
        string GetSalePointName(int? id);
        string GetPcName(int? id);
        string GetCertTypeName(int? id);
        string GetCustomerTypeName(int? id);
        string GetTicketTypeName(int? id);
        string GetTicketTypeDisplayName(int? id);
        string GetTicketTypeClassName(int? id);
        string GetTradeTypeName(int? id);
        string GetPayTypeName(int? id);
        PayType GetPayType(int? id);
        string GetChangCiName(int? id);
        string GetChangCiTimeRange(int? id);
        string GetSeatName(long? id);
    }
}
