using System;
using System.Threading.Tasks;

namespace Egoal.Scenics
{
    public interface IScenicDomainService
    {
        Task<int> GetGroundSeatSurplusQuantityAsync(Ground ground, DateTime date, int changCiId);
    }
}
