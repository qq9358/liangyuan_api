using Egoal.Reflection;
using System.Linq;

namespace Egoal.Payment
{
    public class DefaultPayType
    {
        public const int 现金 = 1;
        public const int 银行卡 = 2;
        public const int 储值卡 = 3;
        public const int 预售券 = 4;
        public const int 挂账 = 5;
        public const int 银行转账 = 6;
        public const int 积分抵扣 = 7;
        public const int 微信支付 = 8;
        public const int 支付宝付款 = 9;
        public const int 网上支付 = 10;
        public const int 网银支付 = 11;
        public const int 账户扣款 = 12;
        public const int 公司赠送 = 13;
        public const int 第三方微信 = 14;
        public const int 网上充值 = 16;
        public const int 威富通支付 = 17;
        public const int 扫呗支付 = 18;
        public const int 收钱吧支付 = 19;
        public const int 第三方支付 = 20;
        public const int 兴业银行 = 21;
        public const int 青海农商 = 22;
        public const int 工商银行 = 23;

        public static string GetName(int value)
        {
            return ReflectionHelper.GetFieldNameByValue<DefaultPayType, int>(value);
        }

        public static bool IsNetPay(int payTypeId)
        {
            var netPayTypes = new int[]
            {
                微信支付,
                支付宝付款,
                第三方微信,
                威富通支付,
                扫呗支付,
                收钱吧支付,
                兴业银行,
                青海农商,
                工商银行
            };

            return netPayTypes.Any(p => p == payTypeId);
        }
    }
}
