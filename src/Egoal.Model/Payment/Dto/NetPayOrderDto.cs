namespace Egoal.Payment.Dto
{
    public class NetPayOrderDto
    {
        public decimal PayMoney { get; set; }
        public long ExpireSeconds { get; set; }
        public bool PaySuccess { get; set; }
        public bool IsPaying { get; set; }
    }
}
