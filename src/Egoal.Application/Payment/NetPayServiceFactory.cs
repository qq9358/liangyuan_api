using Egoal.Dependency;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Egoal.Payment
{
    public class NetPayServiceFactory : IScopedDependency
    {
        private IServiceProvider _serviceProvider;

        public NetPayServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public INetPayService GetPayService(int payTypeId)
        {
            switch (payTypeId)
            {
                case DefaultPayType.微信支付:
                    {
                        return _serviceProvider.GetRequiredService<WeChatPay.PayService>();
                    }
                case DefaultPayType.支付宝付款:
                    {
                        return _serviceProvider.GetRequiredService<Alipay.PayService>();
                    }
                case DefaultPayType.扫呗支付:
                    {
                        return _serviceProvider.GetRequiredService<SaobePay.PayService>();
                    }
                case DefaultPayType.工商银行:
                    {
                        return _serviceProvider.GetRequiredService<IcbcPay.PayService>();
                    }
                default:
                    {
                        throw new UnSupportedPayTypeException($"不支持付款方式{payTypeId}");
                    }
            }
        }
    }
}
