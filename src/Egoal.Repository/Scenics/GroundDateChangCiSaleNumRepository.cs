using Dapper;
using Egoal.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Egoal.Scenics
{
    public class GroundDateChangCiSaleNumRepository : EfCoreRepositoryBase<GroundDateChangCiSaleNum>, IGroundDateChangCiSaleNumRepository
    {
        public GroundDateChangCiSaleNumRepository(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public async Task<int> GetSaleQuantityAsync(int groundId, DateTime date, int changCiId)
        {
            string sql = @"
SELECT
SUM(SaleNum)
FROM dbo.TM_GroundDateChangCiSaleNum WITH(NOLOCK)
WHERE GroundID=@groundId
AND [Date]=@date
AND ChangCiID=@changCiId
";
            return await Connection.ExecuteScalarAsync<int>(sql, new { groundId, date, changCiId }, Transaction);
        }

        public async Task<bool> SaleAsync(GroundDateChangCiSaleNum groundDateChangCiSaleNum, int totalNum)
        {
            string sql = @"
IF EXISTS(SELECT TOP 1 1 FROM TM_GroundDateChangCiSaleNum WITH(UPDLOCK) WHERE GroundID=@GroundID AND [Date]=@Date AND ChangCiID=@ChangCiID)
BEGIN
	UPDATE TM_GroundDateChangCiSaleNum SET
	SaleNum=SaleNum+@SaleNum
	WHERE GroundID=@GroundID AND [Date]=@Date AND ChangCiID=@ChangCiID AND SaleNum<=@totalNum-@SaleNum
END
ELSE
BEGIN
	INSERT INTO TM_GroundDateChangCiSaleNum
	(
	 GroundID
	,[Date]
	,ChangCiID
	,SaleNum
	)VALUES(
	 @GroundID
	,@Date
	,@ChangCiID
	,@SaleNum
	)
END
";
            var param = new
            {
                groundDateChangCiSaleNum.GroundId,
                groundDateChangCiSaleNum.Date,
                groundDateChangCiSaleNum.ChangCiId,
                groundDateChangCiSaleNum.SaleNum,
                totalNum
            };

            return (await Connection.ExecuteAsync(sql, param, Transaction)) > 0;
        }
    }
}
