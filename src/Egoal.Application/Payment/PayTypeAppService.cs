using Egoal.Application.Services;
using Egoal.Application.Services.Dto;
using Egoal.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Egoal.WeChat;
using Egoal.Payment.Alipay;
using Egoal.Payment.IcbcPay;
using Microsoft.Extensions.Options;

namespace Egoal.Payment
{
    public class PayTypeAppService : ApplicationService, IPayTypeAppService
    {
        private readonly IPayTypeRepository _payTypeRepository;
        private readonly WeChatOptions _weChatOptions;
        private readonly AlipayOptions _alipayOptions;
        private readonly IcbcPayOptions _icbcPayOptions;

        public PayTypeAppService(
            IPayTypeRepository payTypeRepository,
            IOptions<WeChatOptions> weChatOptions,
            IOptions<AlipayOptions> alipayOptions,
            IOptions<IcbcPayOptions> icbcpayOptions)
        {
            _payTypeRepository = payTypeRepository;
            _weChatOptions = weChatOptions.Value;
            _alipayOptions = alipayOptions.Value;
            _icbcPayOptions = icbcpayOptions.Value;
        }

        public async Task EnsureSystemPayTypeAsync()
        {
            await EnsureSystemPayTypeAsync(DefaultPayType.微信支付, "WeChat", "95017");
            await EnsureSystemPayTypeAsync(DefaultPayType.支付宝付款, "Alipay", "95188");
            await EnsureSystemPayTypeAsync(DefaultPayType.工商银行, "Icbc", "95588");
        }

        private async Task EnsureSystemPayTypeAsync(int payTypeId, string supplier, string servicePhone)
        {
            var payType = await _payTypeRepository.FirstOrDefaultAsync(payTypeId);
            if (payType == null)
            {
                payType = new PayType();
                payType.Id = payTypeId;
                payType.Name = DefaultPayType.GetName(payType.Id);
                payType.SortCode = payType.Id.ToString().PadLeft(3, '0');
                payType.PayFlag = true;
                payType.CzkFlag = false;
                payType.Supplier = supplier;
                payType.ServicePhone = servicePhone;

                await _payTypeRepository.InsertSystemPayTypeAsync(payType);
            }
            else
            {
                if (payType.Supplier.IsNullOrEmpty())
                {
                    payType.Supplier = supplier;
                }
                if (payType.ServicePhone.IsNullOrEmpty())
                {
                    payType.ServicePhone = servicePhone;
                }
            }
        }

        public async Task<List<ComboboxItemDto<int>>> GetPayTypeComboboxItemsAsync()
        {
            var query = _payTypeRepository.GetAll()
                .OrderBy(p => p.SortCode)
                .Select(p => new ComboboxItemDto<int>
                {
                    Value = p.Id,
                    DisplayText = p.Name
                });

            return await _payTypeRepository.ToListAsync(query);
        }

        public async Task<List<ComboboxItemDto<int>>> GetNetPayTypeComboboxItemsAsync()
        {
            var result = new List<ComboboxItemDto<int>>();

            if (!_weChatOptions.WxApiKey.IsNullOrEmpty())
            {
                result.Add(new ComboboxItemDto<int> { Value = DefaultPayType.微信支付, DisplayText = "微信支付" });
            }
            if (!_alipayOptions.AliAppID.IsNullOrEmpty())
            {
                result.Add(new ComboboxItemDto<int> { Value = DefaultPayType.支付宝付款, DisplayText = "支付宝付款" });
            }
            if (!_icbcPayOptions.IcbcAppId.IsNullOrEmpty())
            {
                result.Add(new ComboboxItemDto<int> { Value = DefaultPayType.工商银行, DisplayText = "工商银行" });
            }

            return await Task.FromResult(result);
        }
    }
}
