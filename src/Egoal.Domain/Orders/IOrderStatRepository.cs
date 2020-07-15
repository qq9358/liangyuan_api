using Egoal.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Egoal.Orders
{
    public interface IOrderStatRepository : IRepository<OrderStat>
    {
        Task<int> GetOrderQuantityAsync(string date);
    }
}
