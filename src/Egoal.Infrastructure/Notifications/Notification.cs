using System.Collections.Generic;

namespace Egoal.Notifications
{
    public class Notification
    {
        public string MethodName { get; set; }
        public List<string> Clients { get; set; }
        public List<string> Groups { get; set; }
        public List<string> Users { get; set; }
        public object Data { get; set; }
    }
}
