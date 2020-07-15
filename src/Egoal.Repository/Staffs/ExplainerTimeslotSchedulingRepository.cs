using Dapper;
using Egoal.EntityFrameworkCore;
using Egoal.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egoal.Staffs
{
    public class ExplainerTimeslotSchedulingRepository : EfCoreRepositoryBase<ExplainerTimeslotScheduling>, IExplainerTimeslotSchedulingRepository
    {
        public ExplainerTimeslotSchedulingRepository(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public async Task<List<ExplainerTimeslotScheduling>> GetSchedulingsAsync(string date, string time)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendWhere("a.Date=@date");
            sbWhere.AppendWhereIf(date == DateTime.Now.ToDateString(), "b.EndTime>@time");

            string sql = $@"
SELECT
a.*
FROM dbo.RM_ExplainerTimeslotScheduling a
JOIN dbo.RM_ExplainerTimeslot b ON b.Id=a.TimeslotId
{sbWhere.ToString()}
ORDER BY b.EndTime
";
            return (await Connection.QueryAsync<ExplainerTimeslotScheduling>(sql, new { date, time }, Transaction)).ToList();
        }

        public async Task<bool> BookPublicTimeslotAsync(string date, int timeslotId)
        {
            string sql = @"
UPDATE dbo.RM_ExplainerTimeslotScheduling SET
PublicBookedQuantity=PublicBookedQuantity+1
WHERE Date=@date
AND TimeslotId=@timeslotId
AND IsReserved=0
AND PublicQuantity>PublicBookedQuantity
";
            return await Connection.ExecuteAsync(sql, new { date, timeslotId }, Transaction) > 0;
        }

        public async Task CancelPublicTimeslotAsync(string date, int timeslotId)
        {
            string sql = @"
UPDATE dbo.RM_ExplainerTimeslotScheduling SET
PublicBookedQuantity=PublicBookedQuantity-1
WHERE Date=@date
AND TimeslotId=@timeslotId
";
            await Connection.ExecuteAsync(sql, new { date, timeslotId }, Transaction);
        }


        public async Task<bool> BookReservedTimeslotAsync(string date, int timeslotId)
        {
            string sql = @"
UPDATE dbo.RM_ExplainerTimeslotScheduling SET
ReservedBookedQuantity=ReservedBookedQuantity+1
WHERE Date=@date
AND TimeslotId=@timeslotId
AND ReservedQuantity>ReservedBookedQuantity
";
            return await Connection.ExecuteAsync(sql, new { date, timeslotId }, Transaction) > 0;
        }

        public async Task CancelReservedTimeslotAsync(string date, int timeslotId)
        {
            string sql = @"
UPDATE dbo.RM_ExplainerTimeslotScheduling SET
ReservedBookedQuantity=ReservedBookedQuantity-1
WHERE Date=@date
AND TimeslotId=@timeslotId
";
            await Connection.ExecuteAsync(sql, new { date, timeslotId }, Transaction);
        }
    }
}
