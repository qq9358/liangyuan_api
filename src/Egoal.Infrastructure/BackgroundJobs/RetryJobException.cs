namespace Egoal.BackgroundJobs
{
    public class RetryJobException : TmsException
    {
        public RetryJobException(string message) : base(message)
        { }
    }
}
