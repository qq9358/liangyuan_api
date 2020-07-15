using Dapper;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data.SqlClient;

namespace Egoal.EntityFrameworkCore
{
    public class OptionsBuilderHelper
    {
        public static void BuildSqlServerOptions(string connectionString, SqlServerDbContextOptionsBuilder builder)
        {
            int version = GetSqlServerVersion(connectionString);

            if (version < 2012)
            {
                builder.UseRowNumberForPaging();
            }
        }

        public static int GetSqlServerVersion(string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sql = "SELECT @@VERSION";
                    string version = connection.ExecuteScalar<string>(sql);

                    int.TryParse(version.Substring(21, 4), out int iVersion);

                    return iVersion;
                }
            }
            catch
            {
                return 0;
            }
        }
    }
}
