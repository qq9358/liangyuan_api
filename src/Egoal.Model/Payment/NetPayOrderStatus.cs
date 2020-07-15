namespace Egoal.Payment
{
    public enum NetPayOrderStatus
    {
        支付成功 = 1,
        转入退款 = 2,
        未支付 = 3,
        已关闭 = 4,
        已撤销 = 5,
        用户支付中 = 6,
        支付失败 = 7,
        交易结束 = 8
    }
}
