using Egoal.Messages.Dto;
using System.Threading.Tasks;

namespace Egoal.Messages
{
    public interface IShortMessageAppService
    {
        Task SendVerificationCodeAsync(string mobile, string code);
        Task SendRefundMessageAsync(string mobile, string travelDate, string reason);

        /// <summary>
        /// 购买成功通知
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="monthDay"></param>
        /// <param name="listNo"></param>
        /// <param name="entryTime"></param>
        /// <param name="needActive"></param>
        /// <returns></returns>
        Task SendPersonalBookSuccessAsync(string mobile, string monthDay, string listNo, string entryTime, bool needActive, string qrCodeString);

        /// <summary>
        /// 预约成功通知
        /// </summary>
        /// <param name="address"></param>
        /// <param name="monthDay"></param>
        /// <param name="listNo"></param>
        /// <param name="entryTime"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        Task SendGroupBookSuccessAsync(string address, string monthDay, string listNo, string entryTime, int number);
    }
}
