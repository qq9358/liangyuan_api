using Dapper;
using Egoal.Common.Dto;
using Egoal.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Egoal.Common
{
    public class ChangCiRepository : EfCoreRepositoryBase<ChangCi>, IChangCiRepository
    {
        public ChangCiRepository(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public async Task<List<DateChangCiSaleDto>> GetChangCiForSaleAsync(string date)
        {
            string sql = @"
SELECT
a.[Date],
a.GroundID,
c.ID AS ChangCiId,
c.[Name] AS ChangCiName,
c.STime,
c.ETime,
c.ChangCiNum,
ISNULL(d.SaleNum,0) AS SaleNum
FROM dbo.TM_ChangCiPlan a
JOIN dbo.TM_ChangCiGroupDetail b ON b.ChangCiGroupID=a.ChangCiGroupID
JOIN dbo.TM_ChangCi c ON c.ID=b.ChangCiID
LEFT JOIN dbo.TM_GroundDateChangCiSaleNum d ON d.[Date]=a.[Date] AND d.ChangCiID=c.ID
WHERE a.[Date]=@date
ORDER BY c.STime
";
            var items = await Connection.QueryAsync<DateChangCiSaleDto>(sql, new { date }, Transaction);

            return items.ToList();
        }
    }
}
