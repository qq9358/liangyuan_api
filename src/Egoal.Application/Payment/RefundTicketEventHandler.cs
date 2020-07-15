using Egoal.Dependency;
using Egoal.Events.Bus.Handlers;
using Egoal.Tickets;
using System;
using System.Threading.Tasks;

namespace Egoal.Payment
{
    public class RefundTicketEventHandler : IAsyncEventHandler<RefundTicketEventData>, IScopedDependency
    {
        private readonly IPayAppService _payAppService;

        public RefundTicketEventHandler(IPayAppService payAppService)
        {
            _payAppService = payAppService;
        }

        public async Task HandleEventAsync(RefundTicketEventData eventData)
        {
            if (!DefaultPayType.IsNetPay(eventData.PayTypeId)) return;

            var refundMoney = Math.Abs(eventData.TotalMoney);
            if (refundMoney == 0) return;

            var refundMoneyApply = new RefundMoneyApply();
            refundMoneyApply.RefundListNo = eventData.RefundListNo;
            refundMoneyApply.PayListNo = eventData.PayListNo;
            refundMoneyApply.RefundMoney = refundMoney;
            refundMoneyApply.Reason = eventData.RefundReason;
            await _payAppService.ApplyRefundAsync(refundMoneyApply);
        }
    }
}
