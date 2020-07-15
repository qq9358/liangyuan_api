using Egoal.Reflection;

namespace Egoal.TicketTypes
{
    public class DefaultTicketType
    {
        public const int 套票 = -1;
        public const int 储值卡出园 = -2;
        public const int 二代证免费入园 = -3;
        public const int 预约成人票 = -4;
        public const int 预约儿童票 = -5;
        public const int 预约团体票 = -6;
        public const int 网上储值卡 = -7;
        public const int 电子会员卡 = -8;
		public const int 学生二代证免票 = -9;

        public static string GetName(int value)
        {
            return ReflectionHelper.GetFieldNameByValue<DefaultTicketType, int>(value);
        }
    }
}
