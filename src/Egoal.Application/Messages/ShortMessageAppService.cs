using Egoal.Application.Services;
using Egoal.Messages.Dto;
using Egoal.ShortMessage;
using System.Threading.Tasks;

namespace Egoal.Messages
{
    public class ShortMessageAppService : ApplicationService, IShortMessageAppService
    {
        private readonly IShortMessageService _shortMessageService;

        public ShortMessageAppService(IShortMessageService shortMessageService)
        {
            _shortMessageService = shortMessageService;
        }

        public async Task SendVerificationCodeAsync(string mobile, string code)
        {
            var messageInfo = new MessageInfo();
            messageInfo.Mobile = mobile;
            messageInfo.Content = $"您的验证码是：{code}。请不要把验证码泄露给其他人。";

            await _shortMessageService.SendAsync(messageInfo);
        }

        public async Task SendRefundMessageAsync(string mobile, string travelDate, string reason)
        {
            MessageInfo messageInfo = new MessageInfo();
            messageInfo.Mobile = mobile;
            messageInfo.Content = $"馆方由于人力不可抗因素决定{travelDate}闭馆一天，门票款项在1-3个工作日内退回原支付账户，敬请谅解！";

            await _shortMessageService.SendAsync(messageInfo);
        }

        /// <summary>
        /// 购买成功通知
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="listNo"></param>
        /// <param name="monthDay"></param>
        /// <param name="entryTime"></param>
        /// <returns></returns>
        public async Task SendPersonalBookSuccessAsync(string mobile, string monthDay, string listNo, string entryTime, bool needActive, string qrCodeString)
        {
            MessageInfo messageInfo = new MessageInfo();
            messageInfo.Mobile = mobile;
            if (needActive)
            {
                messageInfo.Content = $"您已购{monthDay}景区门票，订单号：{listNo}，入场时段{entryTime}，请携带相应证件到窗口激活，并在指定时段内检票入馆{qrCodeString}。";
            }
            else
            {
                messageInfo.Content = $"您已购{monthDay}景区门票，订单号：{listNo}，入场时段{entryTime}，请在指定时段内检票入馆{qrCodeString}。";
            }

            await _shortMessageService.SendAsync(messageInfo);
        }

        /// <summary>
        /// 预约成功通知
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="listNo"></param>
        /// <param name="monthDay"></param>
        /// <returns></returns>
        public async Task SendGroupBookSuccessAsync(string mobile, string monthDay, string listNo, string entryTime, int number)
        {
            MessageInfo messageInfo = new MessageInfo();
            messageInfo.Mobile = mobile;
            messageInfo.Content = $"您已预约{monthDay}景区门票，人数：{number}，订单号：{listNo}，入场时段：{entryTime}，凭订单号到窗口取票，并在指定时段内检票入馆。";

            await _shortMessageService.SendAsync(messageInfo);
        }
    }
}
