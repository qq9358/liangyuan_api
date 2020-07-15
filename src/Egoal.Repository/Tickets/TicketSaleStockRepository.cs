using Dapper;
using Egoal.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Text;

namespace Egoal.Tickets
{
    public class TicketSaleStockRepository : EfCoreRepositoryBase<TicketSaleStock>, ITicketSaleStockRepository
    {
        public TicketSaleStockRepository(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public async Task<int> GetSaleQuantityAsync(int ticketTypeId, DateTime startDate, DateTime endDate)
        {
            string sql = @"
SELECT 
SUM(SaleNum) 
FROM dbo.TM_TicketSaleStock WITH(UPDLOCK)
WHERE TicketTypeID=@ticketTypeId
AND TravelDate>=@startDate
AND TravelDate<=@endDate
";
            return await Connection.ExecuteScalarAsync<int>(sql, new { ticketTypeId, startDate, endDate }, Transaction);
        }

        public async Task<bool> UpdateStockAsync(TicketSaleStock saleStock)
        {
            StringBuilder where = new StringBuilder();
            where.AppendWhere("TicketTypeID=@TicketTypeId");
            where.AppendWhere("TravelDate=@TravelDate");
            where.AppendWhere(saleStock.CustomerTypeId.HasValue ? "CustomerTypeId=@CustomerTypeId" : "CustomerTypeId IS NULL");
            where.AppendWhere(saleStock.CustomerId.HasValue ? "CustomerID=@CustomerId" : "CustomerID IS NULL");

            string sql = $@"
DECLARE @StockId INT
SELECT TOP 1 @StockId=ID 
FROM dbo.TM_TicketSaleStock 
{where}

IF ISNULL(@StockId,-1)<>-1
BEGIN
	UPDATE dbo.TM_TicketSaleStock SET
	SaleNum=SaleNum+@SaleNum
	WHERE ID=@StockId
END
ELSE
BEGIN
	INSERT INTO dbo.TM_TicketSaleStock
    ( 
    TicketTypeID ,
	CustomerTypeId ,
	CustomerID ,
	TravelDate ,
	SaleNum
    )VALUES( 
    @TicketTypeID ,
	@CustomerTypeId ,
	@CustomerID ,
	@TravelDate ,
	@SaleNum
	)
END
";
            return await Connection.ExecuteAsync(sql, saleStock, Transaction) > 0;
        }
    }
}
