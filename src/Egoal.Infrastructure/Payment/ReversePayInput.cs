using System;

namespace Egoal.Payment
{
    public class ReversePayInput
    {
        public string ListNo { get; set; }
        public string TransactionId { get; set; }
        public string SubPayTypeId { get; set; }
        public DateTime PayTime { get; set; }
    }
}
