using Egoal.Domain.Entities;
using System;

namespace Egoal.Common
{
    public class ApiLog : Entity<long>
    {
        public string ServiceName { get; set; }
        public string RequestContent { get; set; }
        public string ResponseContent { get; set; }
        public string Exception { get; set; }
        public string ClientIpAddress { get; set; }
        public string ClientName { get; set; }
        public DateTime Ctime { get; set; }
    }
}
