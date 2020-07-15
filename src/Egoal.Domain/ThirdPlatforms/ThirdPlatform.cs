using Egoal.Domain.Entities;
using System;

namespace Egoal.ThirdPlatforms
{
    public class ThirdPlatform : Entity<string>
    {
        public string Name { get; set; }
        public string Uid { get; set; }
        public string Pwd { get; set; }
        public string OrderCheckUrl { get; set; }
        public PlatformType PlatformType { get; set; }
        public DateTime? CTime { get; set; } = DateTime.Now;
    }
}
