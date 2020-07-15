using Egoal.Domain.Entities;
using System;

namespace Egoal.BackgroundJobs
{
    public class AbpBackgroundJob : Entity<long>
    {
        public AbpBackgroundJob()
        {
            Priority = 15;
            CreationTime = DateTime.Now;
            NextTryTime = DateTime.Now;
        }

        public string JobType { set; get; }
        public string JobArgs { set; get; }
        public int TryCount { set; get; }
        public DateTime NextTryTime { set; get; }
        public DateTime? LastTryTime { set; get; }
        public bool IsAbandoned { set; get; }
        public int Priority { set; get; }
        public DateTime CreationTime { set; get; }
        public long? CreatorUserId { set; get; }
    }
}
