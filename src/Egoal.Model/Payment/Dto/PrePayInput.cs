using Egoal.AutoMapper;

namespace Egoal.Payment.Dto
{
    [AutoMapTo(typeof(NetPayInput))]
    public class PrePayInput
    {
        public string ListNo { get; set; }
        public decimal PayMoney { get; set; }
        public string ProductInfo { get; set; }
        public string ProductId { get; set; }
        public string OpenId { get; set; }
        public string ClientIp { get; set; }
        public string Attach { get; set; }
        public string SubPayTypeId { get; set; }
    }
}
