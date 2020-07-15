using Dapper;
using Egoal.Application.Services.Dto;
using Egoal.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Egoal.Scenics
{
    public class PcRepository : EfCoreRepositoryBase<Pc>, IPcRepository
    {
        public PcRepository(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public async Task<List<ComboboxItemDto<int>>> GetCashPcComboboxItemsAsync()
        {
            string sql = @"
SELECT
a.PCID AS Value,
b.Name AS DisplayText
FROM dbo.RM_PCSale a
JOIN dbo.RM_PC b ON b.ID=a.PCID
";
            return (await Connection.QueryAsync<ComboboxItemDto<int>>(sql, null, Transaction)).ToList();
        }
    }
}
