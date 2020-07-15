using System.Text;

namespace Egoal
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendWhere(this StringBuilder sb, string where)
        {
            return sb.AppendWhereIf(true, where);
        }

        public static StringBuilder AppendWhereIf(this StringBuilder sb, bool condition, string where)
        {
            if (!condition)
            {
                return sb;
            }

            sb.Append(sb.Length == 0 ? "WHERE" : "AND").Append(" ");
            sb.Append(where).Append(" ");

            return sb;
        }
    }
}
