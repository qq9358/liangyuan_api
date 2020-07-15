using System.Collections.Generic;

namespace Egoal.Payment.Alipay
{
    public class SettleInfo
    {
        public List<SettleDetailInfo> settle_detail_infos { get; set; }
        public string merchant_type { get; set; }
    }

    public class SettleDetailInfo
    {
        public string trans_in_type { get; set; }
        public string trans_in { get; set; }
        public string summary_dimension { get; set; }
        public string settle_entity_id { get; set; }
        public string settle_entity_type { get; set; }
        public decimal amount { get; set; }
    }
}
