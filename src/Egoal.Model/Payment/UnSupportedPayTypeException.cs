namespace Egoal.Payment
{
    public class UnSupportedPayTypeException : TmsException
    {
        public UnSupportedPayTypeException(string message)
            : base(message)
        { }
    }
}
