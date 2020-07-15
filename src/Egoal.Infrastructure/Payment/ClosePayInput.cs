using System;

namespace Egoal.Payment
{
    public class ClosePayInput
    {
        public string ListNo { get; set; }
        public string TransactionId { get; set; }
        public DateTime PayTime { get; set; }
    }
}
