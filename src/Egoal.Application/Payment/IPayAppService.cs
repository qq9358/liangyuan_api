using Egoal.Payment.Dto;
using System.Threading.Tasks;

namespace Egoal.Payment
{
    public interface IPayAppService
    {
        Task PrePayAsync(PrePayInput payInput);
        Task<NetPayOrderDto> GetNetPayOrderAsync(string listNo);
        Task<string> JsApiPayAsync(JsApiPayInput input);
        Task<PayOutput> MicroPayAsync(MicroPayInput payInput);
        Task<string> NativePayAsync(NativePayInput input);
        Task<string> H5PayAsync(H5PayInput input);
        Task<PayOutput> CashPayAsync(string listNo);
        Task ClosePayAsync(string listNo);
        Task<NotifyOutput> HandlePayNotifyAsync(string data, int payTypeId);
        Task LoopQueryNetPayAsync(QueryNetPayJobArgs input);
        Task ConfirmPayStatusAsync(ConfirmPayStatusJobArgs input);
        Task ApplyRefundAsync(RefundMoneyApply refundMoneyApply);
        Task RefundAsync(long id);
    }
}
