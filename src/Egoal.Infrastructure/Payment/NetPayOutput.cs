namespace Egoal.Payment
{
    public class NetPayOutput : NotifyInput
    {
        public string ErrorMessage { get; set; }
        public bool IsPaid { get; set; }
        public bool IsPaying { get; set; }
    }
}
