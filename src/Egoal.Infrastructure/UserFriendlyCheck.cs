using Egoal.UI;
using JetBrains.Annotations;
using System.Diagnostics;

namespace Egoal
{
    [DebuggerStepThrough]
    public static class UserFriendlyCheck
    {
        [ContractAnnotation("value:null => halt")]
        public static T NotNull<T>(T value, [InvokerParameterName] [NotNull] string message)
        {
            if (value == null)
            {
                throw new UserFriendlyException(message);
            }

            return value;
        }
    }
}
