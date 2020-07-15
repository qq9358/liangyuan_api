using Egoal.Domain.Entities;
using System;

namespace Egoal.Orders
{
    public class OrderPlan : Entity
    {
        public int? OrderPlanType { get; set; }
        public string Week { get; set; }
        public int MaxNum { get; set; }
        public bool Enabled { get; set; }
        public DateTime? Date { get; set; }
    }
}
