using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Egoal.Settings
{
    public class DbConfigurationProvider : ConfigurationProvider
    {
        private readonly string _connectionString;

        public DbConfigurationProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public override void Load()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    string sql = @"
SELECT
[Name] AS Id,
[Value]
FROM dbo.SM_SysPara";

                    var settings = connection.Query<Setting>(sql);
                    Data = settings.ToDictionary(s => s.Id, s => s.Value);

                    string invoiceSql = @"
IF EXISTS(SELECT 1 FROM sysobjects WHERE id=OBJECT_ID('dbo.SM_InvoicePara') AND type='U')
BEGIN
	SELECT
	[Name] AS Id,
	[Value]
	FROM dbo.SM_InvoicePara
END
";
                    var invoiceSettings = connection.Query<Setting>(invoiceSql);
                    foreach (var setting in invoiceSettings)
                    {
                        Data.Add(setting.Id, setting.Value);
                    }
                }
            }
            catch
            {
                Data = new Dictionary<string, string>();
            }
        }
    }
}
