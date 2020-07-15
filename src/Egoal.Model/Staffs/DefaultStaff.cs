using Egoal.Reflection;

namespace Egoal.Staffs
{
    public class DefaultStaff
    {
        public const int 分销平台 = -100;
        public const int 微信购票 = -101;
        public const int 市民卡登记 = -102;

        public static string GetName(int value)
        {
            return ReflectionHelper.GetFieldNameByValue<DefaultStaff, int>(value);
        }
    }
}
