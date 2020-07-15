using Egoal.Reflection;

namespace Egoal.Members
{
    public class DefaultCertType
    {
        public const int 一代身份证 = 0;
        public const int 护照 = 1;
        public const int 台胞证 = 2;
        public const int 驾驶证 = 3;
        public const int 回乡证 = 4;
        public const int 二代身份证 = 5;
        public const int 护照签证 = 6;
        public const int 港澳通行证 = 7;
        public const int 行驶证 = 8;
        public const int 电子港澳通行证 = 9;
        public const int 卡式台胞证 = 13;

        public static string GetName(int value)
        {
            return ReflectionHelper.GetFieldNameByValue<DefaultCertType, int>(value);
        }
    }
}
