using Dapper;
using Egoal.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egoal.Authorization
{
    public class RoleRepository : EfCoreRepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public async Task<List<string>> GetPermissionsAsync(int roleId, SystemType? systemType)
        {
            StringBuilder where = new StringBuilder();
            where.AppendWhere("a.RoleID=@roleId");
            where.AppendWhere("b.Value <> ''");
            where.AppendWhereIf(systemType.HasValue, "b.SystemTypeID=@systemType");

            string sql = $@"
SELECT
a.RightUniqueCode
FROM dbo.RM_RoleRight a
JOIN dbo.RM_Right b ON b.UniqueCode=a.RightUniqueCode
{where.ToString()}
";
            var permissions = await Connection.QueryAsync<Guid>(sql, new { roleId, systemType }, Transaction);

            return permissions.Select(p => p.ToString()).ToList();
        }

        public async Task<bool> IsGrantedAsync(int roleId, string permission)
        {
            string sql = @"
SELECT TOP 1 1 
FROM dbo.RM_RoleRight 
WHERE RoleID=@roleId
AND RightUniqueCode=@permission
";
            return await Connection.ExecuteScalarAsync<string>(sql, new { roleId, permission }, Transaction) == "1";
        }
    }
}
