using Egoal.Orders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Egoal.Staffs
{
    public interface IExplainerDomainService
    {
        Task BookTimeslotAsync(OrderType orderType, string date, int? timeslotId);
        Task CancelTimeslotAsync(OrderType orderType, string date, int? timeslotId);
        Task BeginExplainAsync(string listNo, int explainerId, int timeslotId);
        Task CompleteExplainAsync(string listNo, int explainerId);
    }
}
