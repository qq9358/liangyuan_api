using Egoal.Application.Services;
using Egoal.Messages.Dto;
using Egoal.Net.Mail;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Egoal.Messages
{
    public class EmailAppService : ApplicationService, IEmailAppService
    {
        private readonly IEmailSender _emailSender;

        public EmailAppService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task SendVerificationCodeAsync(string address, string code)
        {
            await _emailSender.SendAsync(address, "验证码", $"您的验证码是：{code}。请不要把验证码泄露给其他人。", false);
        }

        public async Task SendInvoiceMessageAsync(SendInvoiceMessageInput input)
        {
            var template = await GetTemplateAsync("InvoiceMessage.html");
            var message = string.Format(template, input.InvoiceDate, input.SellerName, input.TotalMoney, input.InvoiceCode, input.InvoiceNo, input.InvoiceUrl, input.InvoiceUrl);

            await _emailSender.SendAsync(input.Email, "您有新发票，请注意查收", message, true);
        }

        private async Task<string> GetTemplateAsync(string name)
        {
            using (StreamReader streamReader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream($"Egoal.Messages.Templates.{name}")))
            {
                return await streamReader.ReadToEndAsync();
            }
        }

        /// <summary>
        /// 购票成功通知
        /// </summary>
        /// <param name="address"></param>
        /// <param name="monthDay"></param>
        /// <param name="listNo"></param>
        /// <param name="entryTime"></param>
        /// <returns></returns>
        public async Task SendPersonalBookSuccessAsync(string address, string monthDay, string listNo, string entryTime, bool needActive, string qrCodeString)
        {
            string content = "";
            if (needActive)
            {
                content = $"您已购{monthDay}景区门票，订单号：{listNo}，入场时段{entryTime}，请携带相应证件到窗口激活，并在指定时段内检票入馆{qrCodeString}。";
            }
            else
            {
                content = $"您已购{monthDay}景区门票，订单号：{listNo}，入场时段{entryTime}，请在指定时段内检票入馆{qrCodeString}。";
            }
            await _emailSender.SendAsync(address, "购票成功通知", content, false);
        }
    }
}
