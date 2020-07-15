using System.Threading.Tasks;

namespace Egoal.Payment
{
    public interface INetPayService
    {
        Task<string> JsApiPayAsync(NetPayInput payInput);
        Task<NetPayOutput> MicroPayAsync(NetPayInput input);
        Task<string> NativePayAsync(NetPayInput input);
        Task<string> H5PayAsync(NetPayInput input);
        NotifyInput Notify(string data);
        NotifyOutput GenerateNotifyResponse(bool success, string message = "", NetPayInput input = null);
        Task<QueryPayOutput> QueryPayAsync(QueryPayInput input);
        Task<ClosePayOutput> ClosePayAsync(ClosePayInput input);
        Task<ReversePayOutput> ReversePayAsync(ReversePayInput input);
        Task<RefundOutput> RefundAsync(RefundInput input);
        Task<QueryRefundOutput> QueryRefundAsync(QueryRefundInput input);
    }
}
