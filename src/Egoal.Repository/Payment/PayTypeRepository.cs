using Dapper;
using Egoal.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Egoal.Payment
{
    public class PayTypeRepository : EfCoreRepositoryBase<PayType>, IPayTypeRepository
    {
        public PayTypeRepository(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public async Task InsertSystemPayTypeAsync(PayType payType)
        {
            string sql = @"
SET IDENTITY_INSERT dbo.SM_PayType ON
INSERT INTO dbo.SM_PayType
(
ID,
[Name],
SortCode,
PayFlag,
CzkFlag,
Supplier,
ServicePhone
)
VALUES
(
@Id,
@Name,
@SortCode,
@PayFlag,
@CzkFlag,
@Supplier,
@ServicePhone
)
SET IDENTITY_INSERT dbo.SM_PayType OFF
";
            await Connection.ExecuteAsync(sql, payType, Transaction);
        }
    }
}
