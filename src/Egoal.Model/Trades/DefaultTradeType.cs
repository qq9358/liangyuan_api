using Egoal.Reflection;

namespace Egoal.Trades
{
    public class DefaultTradeType
    {
        public const int 门票 = 10;
        public const int 门票_网 = 11;
        public const int 门票工本费 = 12;
        public const int 门票补卡收费 = 13;
        public const int 门票延期收费 = 14;
        public const int 门票超时收费 = 15;
        public const int 门票押金 = 16;
        public const int 锅底费 = 17;
        public const int 计时卡强制结算 = 18;
        public const int 门票_微信 = 21;
        public const int 计时卡计时收费 = 22;
        public const int 储值卡 = 31;
        public const int 储值卡工本费 = 32;
        public const int 储值卡补卡收费 = 33;
        public const int 储值卡延期收费 = 34;
        public const int 储值卡换卡收费 = 35;
        public const int 储值卡押金 = 36;
        public const int 储值卡强制结算 = 37;
        public const int 会员卡销售 = 41;
        public const int 会员卡续卡 = 42;
        public const int 会员卡补卡收费 = 43;
        public const int 会员卡换卡收费 = 44;
        public const int 会员卡工本费 = 45;
        public const int 积分卡工本费 = 50;
        public const int 积分卡换卡费 = 51;
        public const int 打折卡工本费 = 80;
        public const int 打折卡换卡费 = 81;
        public const int 商品调拨 = 60;
        public const int 商品销售 = 61;
        public const int 销售退货 = 62;
        public const int 商品出租 = 63;
        public const int 商品归还 = 64;
        public const int 采购进货 = 65;
        public const int 采购退货 = 66;
        public const int 商品报损 = 67;
        public const int 商品报溢 = 68;
        public const int 商品盘点 = 69;
        public const int 商品拆分 = 600;
        public const int 商品合并 = 601;
        public const int 商品押金 = 602;
        public const int 出租退租 = 603;
        public const int 居民登记 = 71;
        public const int 销账 = 72;
        public const int 录入交易 = 73;
        public const int 导服费 = 74;
        public const int 设备租用费 = 75;
        public const int 套餐费 = 76;
        public const int 保险费 = 77;
        public const int 水枪租金 = 78;
        public const int 水枪押金 = 79;

        public static string GetName(int value)
        {
            return ReflectionHelper.GetFieldNameByValue<DefaultTradeType, int>(value);
        }
    }
}
