using Egoal.Extensions;

namespace Egoal.TicketTypes
{
    public static class TicketTypeExtensions
    {
        public static bool IsCheckByNum(this CheckType checkType)
        {
            return checkType.IsIn(CheckType.单_多次票, CheckType.多人票, CheckType.单次套票, CheckType.多次套票);
        }
    }
}
