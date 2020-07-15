using Egoal.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace Egoal.Scenics
{
    public interface IGroundDateChangCiSaleNumRepository : IRepository<GroundDateChangCiSaleNum>
    {
        Task<int> GetSaleQuantityAsync(int groundId, DateTime date, int changCiId);
        Task<bool> SaleAsync(GroundDateChangCiSaleNum groundDateChangCiSaleNum, int totalNum);
    }
}
