using Egoal.Domain.Entities.Auditing;
using System;

namespace Egoal.BackgroundJobs
{
    public class BackgroundJobInfo : CreationAuditedEntity<long>
    {
        public const int MaxJobTypeLength = 512;

        public static int DefaultFirstWaitDuration { get; set; } = 60;

        public static int DefaultTimeout { get; set; } = 172800;

        public static double DefaultWaitFactor { get; set; } = 2.0;

        public virtual string JobType { get; set; }

        public virtual string JobArgs { get; set; }

        public virtual short TryCount { get; set; }

        public virtual DateTime NextTryTime { get; set; }

        public virtual DateTime? LastTryTime { get; set; }

        public virtual bool IsAbandoned { get; set; }

        public virtual BackgroundJobPriority Priority { get; set; }

        public virtual bool IsLocked { get; set; }

        public virtual DateTime? LockEndTime { get; set; }

        public BackgroundJobInfo()
        {
            NextTryTime = DateTime.Now;
            Priority = BackgroundJobPriority.Normal;
        }

        public virtual DateTime? CalculateNextTryTime()
        {
            var nextWaitDuration = DefaultFirstWaitDuration * (Math.Pow(DefaultWaitFactor, TryCount - 1));
            var nextTryDate = LastTryTime.HasValue
                ? LastTryTime.Value.AddSeconds(nextWaitDuration)
                : DateTime.Now.AddSeconds(nextWaitDuration);

            if (nextTryDate.Subtract(CTime).TotalSeconds > DefaultTimeout)
            {
                return null;
            }

            return nextTryDate;
        }
    }
}
