using Egoal.Messages.Dto;
using System.Threading.Tasks;

namespace Egoal.Messages
{
    public interface IEmailAppService
    {
        Task SendVerificationCodeAsync(string address, string code);
        Task SendInvoiceMessageAsync(SendInvoiceMessageInput input);

        /// <summary>
        /// 预约成功通知
        /// </summary>
        /// <param name="address"></param>
        /// <param name="monthDay"></param>
        /// <param name="listNo"></param>
        /// <param name="entryTime"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        Task SendPersonalBookSuccessAsync(string address, string monthDay, string listNo, string entryTime, bool needActive, string qrCodeString);
    }
}
