using System;

namespace Egoal.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method)]
    public class DontWrapResultAttribute : WrapResultAttribute
    {
        public DontWrapResultAttribute()
            : base(false, false)
        {

        }
    }
}
