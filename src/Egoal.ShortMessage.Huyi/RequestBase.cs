namespace Egoal.ShortMessage.Huyi
{
    public abstract class RequestBase
    {
        public RequestBase()
        {
            format = "json";
        }

        public string account { get; set; }
        public string password { get; set; }
        public string time { get; set; }
        public string format { get; set; }
    }
}
